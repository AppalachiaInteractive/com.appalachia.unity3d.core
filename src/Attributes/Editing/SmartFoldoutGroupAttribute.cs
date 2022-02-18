using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;

namespace Appalachia.Core.Attributes.Editing
{
    public class SmartFoldoutGroupAttribute : FoldoutGroupAttribute
    {
        public SmartFoldoutGroupAttribute(string groupName) : base(groupName, false)
        {
        }

        public SmartFoldoutGroupAttribute(
            string groupName,
            bool expanded,
            string backgroundColor,
            bool colorLabel = false,
            string labelColor = null,
            bool colorChildren = false,
            string childColor = null,
            float order = 0) : base(groupName, expanded, order)
        {
            BackgroundColor = backgroundColor;
            ColorLabel = colorLabel;
            LabelColor = labelColor;
            ColorChildren = colorChildren;
            ChildColor = childColor;
        }

        #region Fields and Autoproperties

        public bool ColorChildren;
        public bool ColorLabel;

        public string BackgroundColor;
        public string ChildColor;
        public string LabelColor;

        #endregion

        public bool HasChildColor => ChildColor.IsNotNullOrWhiteSpace();
        public bool HasLabelColor => LabelColor.IsNotNullOrWhiteSpace();

        /// <inheritdoc />
        protected override void CombineValuesWith(PropertyGroupAttribute other)
        {
            if (other is SmartFoldoutGroupAttribute cfga)
            {
                if (ColorLabel)
                {
                    cfga.ColorLabel = ColorLabel;
                }
                else
                {
                    ColorLabel = cfga.ColorLabel;
                }

                if (ColorChildren)
                {
                    cfga.ColorChildren = ColorChildren;
                }
                else
                {
                    ColorChildren = cfga.ColorChildren;
                }

                if (BackgroundColor != null)
                {
                    cfga.BackgroundColor = BackgroundColor;
                }
                else
                {
                    BackgroundColor = cfga.BackgroundColor;
                }

                if (ChildColor != null)
                {
                    cfga.ChildColor = ChildColor;
                }
                else
                {
                    ChildColor = cfga.ChildColor;
                }

                if (LabelColor != null)
                {
                    cfga.LabelColor = LabelColor;
                }
                else
                {
                    LabelColor = cfga.LabelColor;
                }
            }

            if (other is FoldoutGroupAttribute fga)
            {
                if (Expanded)
                {
                    fga.Expanded = Expanded;
                }
                else
                {
                    Expanded = fga.Expanded;
                }
            }
        }
    }
}
