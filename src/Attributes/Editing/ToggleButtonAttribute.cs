#region

using System;
using System.Diagnostics;
using Appalachia.Utility.Colors;
using Sirenix.OdinInspector;

#endregion

namespace Appalachia.Core.Attributes.Editing
{
    [DontApplyToListElements]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public sealed class ToggleButtonAttribute : Attribute
    {
        public ToggleButtonAttribute(
            string memberMethod,
            string label = null,
            bool bold = true,
            Colors.Enum falseColor = Colors.Enum.IndianRed1,
            Colors.Enum trueColor = Colors.Enum.ForestGreen)
        {
            Bold = bold;
            False = falseColor;
            True = trueColor;
            MemberMethod = memberMethod;
            Label = label;
        }

        #region Fields and Autoproperties

        public bool Bold;

        public Colors.Enum False;

        public Colors.Enum True;

        public string Label;
        public string MemberMethod;

        #endregion
    }
}
