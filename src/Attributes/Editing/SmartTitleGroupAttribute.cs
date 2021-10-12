#region

using System;
using System.Diagnostics;
using Sirenix.OdinInspector;

#endregion

namespace Appalachia.Core.Attributes.Editing
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public sealed class SmartTitleGroupAttribute : PropertyGroupAttribute
    {
        public TitleAlignments Alignment;

        public bool Bold;

        public string Color;

        public bool HorizontalLine;

        public bool Indent;
        public string Subtitle;

        public SmartTitleGroupAttribute(
            string title,
            string subtitle = null,
            TitleAlignments alignment = TitleAlignments.Left,
            bool horizontalLine = true,
            bool bold = true,
            bool indent = false,
            int order = 0,
            string color = null) : base(title, order)
        {
            Subtitle = subtitle;
            Alignment = alignment;
            HorizontalLine = horizontalLine;
            Bold = bold;
            Indent = indent;
            Color = color;
        }

        protected override void CombineValuesWith(PropertyGroupAttribute other)
        {
            var otherGroup = other as SmartTitleGroupAttribute;

            if (Subtitle != null)
            {
                otherGroup.Subtitle = Subtitle;
            }
            else
            {
                Subtitle = otherGroup.Subtitle;
            }

            if (Alignment != TitleAlignments.Left)
            {
                otherGroup.Alignment = Alignment;
            }
            else
            {
                Alignment = otherGroup.Alignment;
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

            if (Indent)
            {
                otherGroup.Indent = Indent;
            }
            else
            {
                Indent = otherGroup.Indent;
            }

            if (Color != null)
            {
                otherGroup.Color = Color;
            }
            else
            {
                Color = otherGroup.Color;
            }
        }
    }
}
