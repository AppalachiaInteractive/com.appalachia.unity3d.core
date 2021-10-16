using System;

namespace Appalachia.Core.Attributes.Editing
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CheckboxEnabledAttribute : Attribute
    {
        public bool PreviewField { get; set; }
        public int PreviewHeight { get; set; }
        public bool EnableIf { get; set; }
        public bool ShowIf { get; set; }
        public bool ShowReferencePicker { get; set; }

        public double Min { get; set; }
        public double Max { get; set; }
        public string MinMember { get; set; }
        public string MaxMember { get; set; }
    }
}