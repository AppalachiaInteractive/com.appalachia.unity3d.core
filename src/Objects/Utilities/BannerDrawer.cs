#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Scriptables.AssetTypes;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Utilities
{
    [CallStaticConstructorInEditor]
    public static class BannerDrawer
    {
        static BannerDrawer()
        {
            RegisterInstanceCallbacks.WithoutSorting()
                                     .When.Object<MainFontCollection>()
                                     .IsAvailableThen(i => _mainFontCollection = i);
        }

        #region Static Fields and Autoproperties

        private static Dictionary<BannerData, BannerData> _existingBannerDatas;

        private static Dictionary<Color, Texture2D> _backgrounds;

        private static MainFontCollection _mainFontCollection;

        private static object _object;

        #endregion

        private static Texture2D CreateSolidTexture(int width, int height, Color col)
        {
            var result = new Texture2D(width, height);

            return FillTextureWithColor(result, col);
        }

        private static void DrawTitleGroup(BannerData titleData)
        {
            using (_PRF_DrawTitleGroup.Auto())
            {
                DrawTitleGroup_Split(titleData);

                GUILayout.Space(1f);
            }
        }

        private static void DrawTitleGroup_Split(BannerData d)
        {
            using (_PRF_DrawTitleGroup_Split.Auto())
            {
                var firstContent = d.reversed ? d.subtitleContent : d.titleContent;
                var firstStyle = d.reversed ? d.subtitleStyle : d.titleStyle;
                var fallbackFirstContent = d.reversed ? d.fallbackSubtitleContent : d.fallbackTitleContent;

                var secondContent = !d.reversed ? d.subtitleContent : d.titleContent;
                var secondStyle = !d.reversed ? d.subtitleStyle : d.titleStyle;
                var fallbackSecondContent = !d.reversed ? d.fallbackSubtitleContent : d.fallbackTitleContent;

                var rect = GUILayoutUtility.GetRect(
                    0.0f,
                    d.titleHeight,
                    firstStyle,
                    GUILayoutOptions.ExpandWidth()
                );

                if (Event.current.type != EventType.Repaint)
                {
                    return;
                }

                var thicknessX = 2;
                var thicknessY = 1;

                var backgroundRect = new Rect
                {
                    x = rect.x - thicknessX,
                    y = rect.y,
                    width = rect.width + (2 * thicknessX),

                    // ReSharper disable once UselessBinaryOperation
                    height = rect.height + (2 * thicknessY),
                };

                if (d.backgroundColor != default)
                {
                    var background = GetBackground(d.backgroundColor);
                    GUI.DrawTexture(backgroundRect, background);
                }

                var cachedY = rect.y;

                float firstSize;
                float secondSize;
                float fallbackFirstSize;
                float fallbackSecondSize;
                var buffer = 14f;
                var minSpacing = 14f;

                var first = firstContent;
                var second = secondContent;

                firstSize = firstStyle.CalcSize(firstContent).x;
                secondSize = secondStyle.CalcSize(secondContent).x;
                fallbackFirstSize = firstStyle.CalcSize(fallbackFirstContent).x;
                fallbackSecondSize = secondStyle.CalcSize(fallbackSecondContent).x;

                var requiredSpace = buffer + minSpacing;

                var minFullSize = firstSize + secondSize + requiredSpace;
                var minFallback1Size = firstSize + fallbackSecondSize + requiredSpace;
                var minFallback2Size = fallbackFirstSize + fallbackSecondSize + requiredSpace;
                var minFallback3Size = fallbackFirstSize + requiredSpace;

                if (rect.width < minFallback3Size)
                {
                    first = null;
                    second = null;
                }
                else if (rect.width < minFallback2Size)
                {
                    first = fallbackFirstContent;
                    second = null;
                }
                else if (rect.width < minFallback1Size)
                {
                    first = fallbackFirstContent;
                    second = fallbackSecondContent;
                }
                else if (rect.width < minFullSize)
                {
                    second = fallbackSecondContent;
                }

                rect.x += buffer / 2f;

                if (second != null)
                {
                    GUI.Label(rect, first, firstStyle);
                }

                rect.y = cachedY;
                rect.x -= buffer;

                if (second != null)
                {
                    GUI.Label(rect, second, secondStyle);
                }
            }
        }

        private static Texture2D FillTextureWithColor(Texture2D texture, Color col)
        {
            if (texture == null)
            {
                texture = new Texture2D(1, 1);
            }

            var pix = texture.GetPixels();

            for (var i = 0; i < pix.Length; i++)
            {
                pix[i] = col;
            }

            texture.SetPixels(pix);
            texture.Apply();

            return texture;
        }

        private static Texture2D GetBackground(Color color)
        {
            _backgrounds ??= new Dictionary<Color, Texture2D>();

            if (_backgrounds.ContainsKey(color))
            {
                return _backgrounds[color];
            }

            var result = CreateSolidTexture(1, 1, color);
            _backgrounds.Add(color, result);

            return result;
        }

        private static BannerData GetBannerData(
            string title,
            string subtitle,
            string fallbackTitle,
            string fallbackSubtitle,
            bool horizontalLine,
            bool boldLabel,
            bool reversed,
            Color titleColor,
            Color subtitleColor,
            Color backgroundColor,
            string titleFont,
            string subtitleFont,
            int titleSize,
            int subtitleSize,
            int titleHeight)
        {
            using (_PRF_GetBannerData.Auto())
            {
                var titleData = new BannerData(
                    title,
                    subtitle,
                    fallbackTitle,
                    fallbackSubtitle,
                    horizontalLine,
                    boldLabel,
                    reversed,
                    titleColor,
                    subtitleColor,
                    backgroundColor,
                    titleFont,
                    subtitleFont,
                    titleSize,
                    subtitleSize,
                    titleHeight
                );

                _existingBannerDatas ??= new Dictionary<BannerData, BannerData>();

                if (_existingBannerDatas.ContainsKey(titleData))
                {
                    return _existingBannerDatas[titleData];
                }

                var referenceTitleStyle = boldLabel ? SirenixGUIStyles.BoldTitle : SirenixGUIStyles.Title;
                var referenceSubtitleStyle =
                    boldLabel ? SirenixGUIStyles.BoldTitleRight : SirenixGUIStyles.TitleRight;

                var titleStyle = new GUIStyle(referenceTitleStyle);
                var subtitleStyle = new GUIStyle(referenceSubtitleStyle);

                if (titleData.titleColor != default)
                {
                    titleStyle.normal.textColor = titleData.titleColor;
                    subtitleStyle.normal.textColor = titleData.titleColor;
                }

                if (titleData.subtitleColor != default)
                {
                    subtitleStyle.normal.textColor = titleData.subtitleColor;
                }

                if (titleFont != null)
                {
                    var tf = GetFont(titleFont);

                    if (tf != null)
                    {
                        titleStyle.font = tf;
                        subtitleStyle.font = tf;
                    }
                }

                if (subtitleFont != null)
                {
                    var sf = GetFont(subtitleFont);

                    if (sf != null)
                    {
                        subtitleStyle.font = sf;
                    }
                }

                if (titleSize != 0)
                {
                    titleStyle.fontSize = titleSize;
                }

                if (subtitleSize != 0)
                {
                    subtitleStyle.fontSize = subtitleSize;
                }

                titleData.titleStyle = titleStyle;
                titleData.subtitleStyle = subtitleStyle;

                _existingBannerDatas.Add(titleData, titleData);

                return titleData;
            }
        }

        private static Font GetFont(string fontName)
        {
            using (_PRF_GetFont.Auto())
            {
                if (_mainFontCollection == null)
                {
                    return null;
                }

                if (_mainFontCollection.Lookup.Items.ContainsKey(fontName))
                {
                    return _mainFontCollection.Lookup.Items[fontName];
                }

                var newFont = _mainFontCollection.Lookup.GetOrLoadOrCreateNew(fontName, fontName);

                return newFont;
            }
        }

        private static void Title(
            string title,
            string subtitle,
            string fallbackTitle,
            string fallbackSubtitle,
            bool horizontalLine,
            bool boldLabel = true,
            bool reversed = false,
            Color titleColor = default,
            Color subtitleColor = default,
            Color backgroundColor = default,
            string titleFont = null,
            string subtitleFont = null,
            int titleSize = 0,
            int subtitleSize = 0,
            int titleHeight = 18)
        {
            using (_PRF_Title.Auto())
            {
                var titleData = GetBannerData(
                    title,
                    subtitle,
                    fallbackTitle,
                    fallbackSubtitle,
                    horizontalLine,
                    boldLabel,
                    reversed,
                    titleColor,
                    subtitleColor,
                    backgroundColor,
                    titleFont,
                    subtitleFont,
                    titleSize,
                    subtitleSize,
                    titleHeight
                );

                DrawTitleGroup(titleData);
            }
        }

        #region Nested type: BannerData

        internal struct BannerData : IEquatable<BannerData>
        {
            public BannerData(
                string title,
                string subtitle,
                string fallbackTitle,
                string fallbackSubtitle,
                bool horizontalLine,
                bool boldLabel,
                bool reversed,
                Color titleColor,
                Color subtitleColor,
                Color backgroundColor,
                string titleFont,
                string subtitleFont,
                int titleSize,
                int subtitleSize,
                int titleHeight)
            {
                this.title = title;
                this.subtitle = subtitle;
                this.fallbackTitle = fallbackTitle;
                this.fallbackSubtitle = fallbackSubtitle;
                this.horizontalLine = horizontalLine;
                this.boldLabel = boldLabel;
                this.reversed = reversed;
                this.titleColor = titleColor;
                this.subtitleColor = subtitleColor;
                this.backgroundColor = backgroundColor;
                this.titleFont = titleFont;
                this.subtitleFont = subtitleFont;
                this.titleSize = titleSize;
                this.subtitleSize = subtitleSize;
                this.titleHeight = titleHeight;
                titleStyle = SirenixGUIStyles.BoldTitle;
                subtitleStyle = SirenixGUIStyles.Subtitle;

                titleContent = new GUIContent(title);

                subtitleContent = new GUIContent(subtitle);

                fallbackSubtitleContent = new GUIContent(fallbackSubtitle ?? subtitle);
                fallbackTitleContent = new GUIContent(fallbackTitle ?? title);
            }

            #region Fields and Autoproperties

            public bool boldLabel;
            public bool horizontalLine;
            public bool reversed;
            public Color backgroundColor;
            public Color subtitleColor;
            public Color titleColor;
            public GUIContent fallbackSubtitleContent;
            public GUIContent fallbackTitleContent;
            public GUIContent subtitleContent;
            public GUIContent titleContent;

            public GUIStyle subtitleStyle;
            public GUIStyle titleStyle;
            public int subtitleSize;
            public int titleHeight;
            public int titleSize;
            public string fallbackSubtitle;
            public string fallbackTitle;
            public string subtitle;
            public string subtitleFont;
            public string title;
            public string titleFont;

            #endregion

            public static bool operator ==(BannerData left, BannerData right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(BannerData left, BannerData right)
            {
                return !left.Equals(right);
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                return obj is BannerData other && Equals(other);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                var hashCode = new HashCode();
                hashCode.Add(boldLabel);
                hashCode.Add(horizontalLine);
                hashCode.Add(reversed);
                hashCode.Add(subtitleColor);
                hashCode.Add(titleColor);
                hashCode.Add(backgroundColor);
                hashCode.Add(subtitle);
                hashCode.Add(subtitleFont);

                hashCode.Add(subtitleSize);
                hashCode.Add(title);
                hashCode.Add(titleFont);

                hashCode.Add(titleSize);
                hashCode.Add(titleHeight);
                hashCode.Add(fallbackSubtitle);
                hashCode.Add(fallbackTitle);
                return hashCode.ToHashCode();
            }

            #region IEquatable<BannerData> Members

            public bool Equals(BannerData other)
            {
                return (boldLabel == other.boldLabel) &&
                       (horizontalLine == other.horizontalLine) &&
                       (reversed == other.reversed) &&
                       subtitleColor.Equals(other.subtitleColor) &&
                       titleColor.Equals(other.titleColor) &&
                       backgroundColor.Equals(other.backgroundColor) &&
                       (subtitle == other.subtitle) &&
                       (subtitleFont == other.subtitleFont) &&
                       (subtitleSize == other.subtitleSize) &&
                       (title == other.title) &&
                       (titleFont == other.titleFont) &&
                       (titleSize == other.titleSize) &&
                       (titleHeight == other.titleHeight);
            }

            #endregion
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(BannerDrawer) + ".";

        private static readonly ProfilerMarker _PRF_GetBannerData =
            new ProfilerMarker(_PRF_PFX + nameof(GetBannerData));

        private static readonly ProfilerMarker _PRF_DrawTitleGroup_Split =
            new ProfilerMarker(_PRF_PFX + nameof(DrawTitleGroup_Split));

        private static readonly ProfilerMarker _PRF_Title = new ProfilerMarker(_PRF_PFX + nameof(Title));

        private static readonly ProfilerMarker _PRF_DrawTitleGroup =
            new ProfilerMarker(_PRF_PFX + nameof(DrawTitleGroup));

        private static readonly ProfilerMarker _PRF_GetFont = new ProfilerMarker(_PRF_PFX + nameof(GetFont));

        #endregion
    }
}

#endif
