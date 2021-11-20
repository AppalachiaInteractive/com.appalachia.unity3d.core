#region

using System;
using System.Diagnostics;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;

#endregion

namespace Appalachia.Core.Attributes.Editing
{
    [DontApplyToListElements]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public class SmartTitleAttribute : Attribute
    {
        public SmartTitleAttribute(
            string title,
            string subtitle = null,
            bool horizontalLine = true,
            bool bold = true,
            bool reversed = false,
            string titleColor = null,
            string titleFont = null,
            string subtitleColor = null,
            string subtitleFont = null,
            string hideIfMemberName = null,
            bool below = false)
        {
            Title = title ?? "null";
            Subtitle = subtitle;
            HorizontalLine = horizontalLine;
            Bold = bold;
            Reversed = reversed;
            TitleColor = titleColor;
            TitleFont = titleFont;
            SubtitleColor = subtitleColor;
            SubtitleFont = subtitleFont;
            HideIfMemberName = hideIfMemberName;
            Below = below;
        }

        #region Fields and Autoproperties

        public bool Below;
        public bool Bold;
        public bool Reversed;
        public bool HorizontalLine;
        public bool Indent;
        public string Subtitle;
        public string SubtitleColor;
        public string SubtitleFont;
        public string Title;
        public string TitleColor;
        public string TitleFont;
        public string HideIfMemberName;

        public bool HasTitleFont => SubtitleFont.IsNotNullOrWhiteSpace();
        public bool HasSubtitleFont => TitleFont.IsNotNullOrWhiteSpace();
        public bool HasTitleColor => TitleColor.IsNotNullOrWhiteSpace();
        public bool HasSubtitleColor => SubtitleColor.IsNotNullOrWhiteSpace();

        #endregion
    }
}
