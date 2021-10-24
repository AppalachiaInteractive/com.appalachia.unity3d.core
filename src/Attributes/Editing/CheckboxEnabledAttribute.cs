using System;

namespace Appalachia.Core.Attributes.Editing
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class CheckboxEnabledAttribute : Attribute
    {
        public bool EnableIf { get; set; }
        public bool PreviewField { get; set; }
        public bool ShowIf { get; set; }
        public bool ShowReferencePicker { get; set; }
        public double Max { get; set; }

        public double Min { get; set; }
        public int PreviewHeight { get; set; }
        public string MaxMember { get; set; }
        public string MinMember { get; set; }
    }
}
