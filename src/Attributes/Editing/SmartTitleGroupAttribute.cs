#region

using System;
using System.Diagnostics;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;

#endregion

namespace Appalachia.Core.Attributes.Editing
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public sealed class SmartTitleGroupAttribute : PropertyGroupAttribute
    {
        public SmartTitleGroupAttribute(string groupName) : base(groupName, 0)
        {
        }

        public SmartTitleGroupAttribute(
            string groupName,
            string title,
            string subtitle = null,
            string fallbackTitle = null,
            string fallbackSubtitle = null,
            bool horizontalLine = true,
            bool bold = true,
            bool reversed = false,
            bool indent = true,
            int order = 0,
            string titleColor = null,
            string titleFont = null,
            string subtitleColor = null,
            string subtitleFont = null,
            string backgroundColor = null,
            int titleSize = 0,
            int subtitleSize = 0,
            int titleHeight = 18) : base(groupName, order)
        {
            Title = title;
            Subtitle = subtitle;
            FallbackTitle = fallbackTitle;
            FallbackSubtitle = fallbackSubtitle;
            HorizontalLine = horizontalLine;
            Bold = bold;
            Reversed = reversed;
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
        public bool HorizontalLine;
        public bool Reversed;
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

        protected override void CombineValuesWith(PropertyGroupAttribute other)
        {
            if (other is SmartTitleGroupAttribute otherGroup)
            {
                if (Title != null)
                {
                    otherGroup.Title = Title;
                }
                else
                {
                    Title = otherGroup.Title;
                }

                if (Subtitle != null)
                {
                    otherGroup.Subtitle = Subtitle;
                }
                else
                {
                    Subtitle = otherGroup.Subtitle;
                }

                if (!HorizontalLine)
                {
                    otherGroup.HorizontalLine = HorizontalLine;
                }
                else
                {
                    HorizontalLine = otherGroup.HorizontalLine;
                }

                if (!Bold)
                {
                    otherGroup.Bold = Bold;
                }
                else
                {
                    Bold = otherGroup.Bold;
                }

                if (Reversed)
                {
                    otherGroup.Reversed = Reversed;
                }
                else
                {
                    Reversed = otherGroup.Reversed;
                }

                if (TitleColor != null)
                {
                    otherGroup.TitleColor = TitleColor;
                }
                else
                {
                    TitleColor = otherGroup.TitleColor;
                }

                if (TitleFont != null)
                {
                    otherGroup.TitleFont = TitleFont;
                }
                else
                {
                    TitleFont = otherGroup.TitleFont;
                }

                if (SubtitleColor != null)
                {
                    otherGroup.SubtitleColor = SubtitleColor;
                }
                else
                {
                    SubtitleColor = otherGroup.SubtitleColor;
                }

                if (SubtitleFont != null)
                {
                    otherGroup.SubtitleFont = SubtitleFont;
                }
                else
                {
                    SubtitleFont = otherGroup.SubtitleFont;
                }

                if (TitleSize != 0)
                {
                    otherGroup.TitleSize = TitleSize;
                }
                else
                {
                    TitleSize = otherGroup.TitleSize;
                }

                if (SubtitleSize != 0)
                {
                    otherGroup.SubtitleSize = SubtitleSize;
                }
                else
                {
                    SubtitleSize = otherGroup.SubtitleSize;
                }
            }
        }
    }
}
