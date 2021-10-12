#region

using System;
using System.Diagnostics;
using Sirenix.OdinInspector;

#endregion

namespace Appalachia.Core.Attributes.Editing
{
    [DontApplyToListElements]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public class SmartTitleAttribute : Attribute
    {
        public TitleAlignments Alignment;

        public bool Below;

        public bool Bold;

        public string Color;

        public string HideIfMemberName;

        public bool HorizontalLine;

        public string Subtitle;
        public string Title;

        public SmartTitleAttribute(
            string title,
            string subtitle = null,
            TitleAlignments alignment = TitleAlignments.Left,
            bool horizontalLine = true,
            bool bold = true,
            string color = null,
            string hideIfMemberName = null,
            bool below = false)
        {
            Title = title ?? "null";
            Subtitle = subtitle;
            Bold = bold;
            Alignment = alignment;
            HorizontalLine = horizontalLine;
            Color = color;
            HideIfMemberName = hideIfMemberName;
            Below = below;
        }
    }
}
