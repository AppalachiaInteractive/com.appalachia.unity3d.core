using Appalachia.CI.Integration.Assets;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Reflection.Extensions;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Root
{
    public sealed class BannerArgs
    {
        public BannerArgs(
            string title,
            string subtitle = null,
            string fallbackTitle = null,
            string fallbackSubtitle = null,
            bool bold = true,
            string titleColor = null,
            string titleFont = null,
            string subtitleColor = null,
            string subtitleFont = null,
            string backgroundColor = null,
            int titleSize = 0,
            int subtitleSize = 0,
            int titleHeight = 18)
        {
            Title = title;
            Subtitle = subtitle;
            FallbackTitle = fallbackTitle;
            FallbackSubtitle = fallbackSubtitle;
            Bold = bold;
            TitleColor = titleColor;
            TitleFont = titleFont;
            BackgroundColor = backgroundColor;
            SubtitleColor = subtitleColor;
            SubtitleFont = subtitleFont;
            TitleSize = titleSize;
            SubtitleSize = subtitleSize;
            TitleHeight = titleHeight;
        }

        #region Fields and Autoproperties

        public bool Bold;
        public int SubtitleSize;
        public int TitleHeight;
        public int TitleSize;
        public string BackgroundColor;
        public string FallbackSubtitle;
        public string FallbackTitle;
        public string Subtitle;
        public string SubtitleColor;
        public string SubtitleFont;
        public string Title;
        public string TitleColor;
        public string TitleFont;

        #endregion

        public bool HasBackgroundColor => BackgroundColor.IsNotNullOrWhiteSpace();
        public bool HasSubtitleColor => SubtitleColor.IsNotNullOrWhiteSpace();
        public bool HasSubtitleFont => TitleFont.IsNotNullOrWhiteSpace();
        public bool HasTitleColor => TitleColor.IsNotNullOrWhiteSpace();
        public bool HasTitleFont => SubtitleFont.IsNotNullOrWhiteSpace();
    }

    [InspectorIcon(Brand.AppalachiaObject.Icon)]
    [AssetLabel(Brand.AppalachiaObject.Label)]
    public partial class AppalachiaObject : IBranded
    {
        protected virtual string GetBackgroundColor()
        {
            return Brand.AppalachiaObject.Banner;
        }

        protected virtual string GetFallbackSubtitle()
        {
            return Brand.Subtitle.Fallback;
        }

        protected virtual string GetFallbackTitle()
        {
            return Brand.AppalachiaObject.Fallback;
        }

        protected virtual string GetSubtitle()
        {
            return Brand.Subtitle.Text;
        }

        protected virtual string GetTitle()
        {
            return Brand.AppalachiaObject.Text;
        }

        protected virtual string GetTitleColor()
        {
            return Brand.AppalachiaObject.Color;
        }

        private void DrawBanner()
        {
            /*_bannerArgs ??= new BannerArgs(
                "$" + nameof(GetTitle),
                "$" + nameof(GetSubtitle),
                "$" + nameof(GetFallbackTitle),
                "$" + nameof(GetFallbackSubtitle),
                Brand.Title.IsBold,
                "$" + nameof(GetTitleColor),
                Brand.Font.ObjectFont,
                "$" + nameof(GetTitleColor),
                Brand.Font.ObjectFont,
                titleSize: Brand.Title.Size,
                subtitleSize: Brand.Subtitle.Size,
                backgroundColor: "$" + nameof(GetBackgroundColor),
                titleHeight: Brand.Title.Height
            );*/
        }

        #region IBranded Members

        string IBranded.GetBackgroundColor()
        {
            return GetBackgroundColor();
        }

        string IBranded.GetFallbackSubtitle()
        {
            return GetFallbackSubtitle();
        }

        string IBranded.GetFallbackTitle()
        {
            return GetFallbackTitle();
        }

        string IBranded.GetSubtitle()
        {
            return GetSubtitle();
        }

        string IBranded.GetTitle()
        {
            return GetTitle();
        }

        string IBranded.GetTitleColor()
        {
            return GetTitleColor();
        }

        #endregion
    }

    [InspectorIcon(Brand.AppalachiaRepository.Icon)]
    [AssetLabel(Brand.AppalachiaRepository.Label)]
    public partial class AppalachiaRepository
    {
        /// <inheritdoc />
        protected override string GetBackgroundColor()
        {
            return Brand.AppalachiaRepository.Banner;
        }

        /// <inheritdoc />
        protected override string GetFallbackTitle()
        {
            return Brand.AppalachiaRepository.Fallback;
        }

        /// <inheritdoc />
        protected override string GetTitle()
        {
            return Brand.AppalachiaRepository.Text;
        }

        /// <inheritdoc />
        protected override string GetTitleColor()
        {
            return Brand.AppalachiaRepository.Color;
        }
    }

    public partial class AppalachiaObject<T>
    {
    }

    [InspectorIcon(Brand.SingletonAppalachiaObject.Icon)]
    [AssetLabel(Brand.SingletonAppalachiaObject.Label)]
    public partial class SingletonAppalachiaObject<T>
    {
        /// <inheritdoc />
        protected override string GetBackgroundColor()
        {
            return Brand.SingletonAppalachiaObject.Banner;
        }

        /// <inheritdoc />
        protected override string GetFallbackTitle()
        {
            return Brand.SingletonAppalachiaObject.Fallback;
        }

        /// <inheritdoc />
        protected override string GetTitle()
        {
            return Brand.SingletonAppalachiaObject.Text;
        }

        /// <inheritdoc />
        protected override string GetTitleColor()
        {
            return Brand.SingletonAppalachiaObject.Color;
        }
    }

    [InspectorIcon(Brand.AppalachiaBehaviour.Icon)]
    public partial class AppalachiaBehaviour : IBranded
    {
        #region Constants and Static Readonly

        public const string GAME_OBJECT_ICON = Brand.Squirrel.Outline;

        #endregion

        #region Static Fields and Autoproperties

        private static Texture2D _icon;

        #endregion

        protected virtual string GetBackgroundColor()
        {
            return Brand.AppalachiaBehaviour.Banner;
        }

        protected virtual string GetFallbackSubtitle()
        {
            return Brand.Subtitle.Fallback;
        }

        protected virtual string GetFallbackTitle()
        {
            return Brand.AppalachiaBehaviour.Fallback;
        }

        protected virtual Texture2D GetGameObjectIcon()
        {
#if UNITY_EDITOR
            if (_icon == null)
            {
                _icon = AssetDatabaseManager.FindFirstAssetMatch<Texture2D>(GAME_OBJECT_ICON);
            }
#endif

            return _icon;
        }

        protected virtual string GetSubtitle()
        {
            return Brand.Subtitle.Text;
        }

        protected virtual string GetTitle()
        {
            return Brand.AppalachiaBehaviour.Text;
        }

        protected virtual string GetTitleColor()
        {
            return Brand.AppalachiaBehaviour.Color;
        }

        #region IBranded Members

        string IBranded.GetBackgroundColor()
        {
            return GetBackgroundColor();
        }

        string IBranded.GetFallbackSubtitle()
        {
            return GetFallbackSubtitle();
        }

        string IBranded.GetFallbackTitle()
        {
            return GetFallbackTitle();
        }

        string IBranded.GetSubtitle()
        {
            return GetSubtitle();
        }

        string IBranded.GetTitle()
        {
            return GetTitle();
        }

        string IBranded.GetTitleColor()
        {
            return GetTitleColor();
        }

        #endregion
    }

    public partial class AppalachiaBehaviour<T>
    {
    }

    [InspectorIcon(Brand.SingletonAppalachiaBehaviour.Icon)]
    public partial class SingletonAppalachiaBehaviour<T>
    {
        /// <inheritdoc />
        protected override string GetBackgroundColor()
        {
            return Brand.SingletonAppalachiaBehaviour.Banner;
        }

        /// <inheritdoc />
        protected override string GetFallbackTitle()
        {
            return Brand.SingletonAppalachiaBehaviour.Fallback;
        }

        /// <inheritdoc />
        protected override string GetTitle()
        {
            return Brand.SingletonAppalachiaBehaviour.Text;
        }

        /// <inheritdoc />
        protected override string GetTitleColor()
        {
            return Brand.SingletonAppalachiaBehaviour.Color;
        }
    }

    [InspectorIcon(Brand.AppalachiaBase.Icon)]
    public partial class AppalachiaSimpleBase : IBranded
    {
        #region Fields and Autoproperties

        private string _displayTypeName;

        #endregion

        protected virtual string GetBackgroundColor()
        {
            return Brand.AppalachiaBase.Banner;
        }

        protected virtual string GetFallbackSubtitle()
        {
            return Brand.AppalachiaBase.SubtitleText;
        }

        protected virtual string GetFallbackTitle()
        {
            using (_PRF_GetFallbackTitle.Auto())
            {
                _displayTypeName ??= GetType().GetSimpleReadableName();

                return _displayTypeName;
            }
        }

        protected virtual string GetSubtitle()
        {
            return Brand.AppalachiaBase.SubtitleText;
        }

        protected virtual string GetTitle()
        {
            using (_PRF_GetTitle.Auto())
            {
                _displayTypeName ??= GetType().GetSimpleReadableName();

                return _displayTypeName;
            }
        }

        protected virtual string GetTitleColor()
        {
            return Brand.AppalachiaBase.Color;
        }

        #region IBranded Members

        string IBranded.GetBackgroundColor()
        {
            return GetBackgroundColor();
        }

        string IBranded.GetFallbackSubtitle()
        {
            return GetFallbackSubtitle();
        }

        string IBranded.GetFallbackTitle()
        {
            return GetFallbackTitle();
        }

        string IBranded.GetSubtitle()
        {
            return GetSubtitle();
        }

        string IBranded.GetTitle()
        {
            return GetTitle();
        }

        string IBranded.GetTitleColor()
        {
            return GetTitleColor();
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetFallbackTitle =
            new ProfilerMarker(_PRF_PFX + nameof(GetFallbackTitle));

        private static readonly ProfilerMarker _PRF_GetTitle =
            new ProfilerMarker(_PRF_PFX + nameof(GetTitle));

        #endregion
    }

    public partial class AppalachiaBase
    {
    }

    public partial class AppalachiaBase<T>
    {
    }

    [InspectorIcon(Brand.AppalachiaPlayable.Icon)]
    public partial class AppalachiaSimplePlayable : IBranded
    {
        protected virtual string GetBackgroundColor()
        {
            return Brand.AppalachiaPlayable.Banner;
        }

        protected virtual string GetFallbackSubtitle()
        {
            return Brand.Subtitle.Fallback;
        }

        protected virtual string GetFallbackTitle()
        {
            return Brand.AppalachiaPlayable.Fallback;
        }

        protected virtual string GetSubtitle()
        {
            return Brand.Subtitle.Text;
        }

        protected virtual string GetTitle()
        {
            return Brand.AppalachiaPlayable.Text;
        }

        protected virtual string GetTitleColor()
        {
            return Brand.AppalachiaPlayable.Color;
        }

        #region IBranded Members

        string IBranded.GetBackgroundColor()
        {
            return GetBackgroundColor();
        }

        string IBranded.GetFallbackSubtitle()
        {
            return GetFallbackSubtitle();
        }

        string IBranded.GetFallbackTitle()
        {
            return GetFallbackTitle();
        }

        string IBranded.GetSubtitle()
        {
            return GetSubtitle();
        }

        string IBranded.GetTitle()
        {
            return GetTitle();
        }

        string IBranded.GetTitleColor()
        {
            return GetTitleColor();
        }

        #endregion
    }

    public partial class AppalachiaPlayable
    {
    }

    public partial class AppalachiaPlayable<T>
    {
    }

    [InspectorIcon(Brand.AppalachiaSelectable.Icon)]
    public partial class AppalachiaSelectable<T> : IBranded
    {
        #region Constants and Static Readonly

        public const string GAME_OBJECT_ICON = Brand.Squirrel.Outline;

        #endregion

        protected virtual string GetBackgroundColor()
        {
            return Brand.AppalachiaSelectable.Banner;
        }

        protected virtual string GetFallbackSubtitle()
        {
            return Brand.Subtitle.Fallback;
        }

        protected virtual string GetFallbackTitle()
        {
            return Brand.AppalachiaSelectable.Fallback;
        }

        protected virtual string GetGameObjectIcon()
        {
            return GAME_OBJECT_ICON;
        }

        protected virtual string GetSubtitle()
        {
            return Brand.Subtitle.Text;
        }

        protected virtual string GetTitle()
        {
            return Brand.AppalachiaSelectable.Text;
        }

        protected virtual string GetTitleColor()
        {
            return Brand.AppalachiaSelectable.Color;
        }

        #region IBranded Members

        string IBranded.GetBackgroundColor()
        {
            return GetBackgroundColor();
        }

        string IBranded.GetFallbackSubtitle()
        {
            return GetFallbackSubtitle();
        }

        string IBranded.GetFallbackTitle()
        {
            return GetFallbackTitle();
        }

        string IBranded.GetSubtitle()
        {
            return GetSubtitle();
        }

        string IBranded.GetTitle()
        {
            return GetTitle();
        }

        string IBranded.GetTitleColor()
        {
            return GetTitleColor();
        }

        #endregion
    }
}
