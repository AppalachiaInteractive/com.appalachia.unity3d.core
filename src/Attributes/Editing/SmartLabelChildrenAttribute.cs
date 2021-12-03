using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Attributes.Editing
{
    [AttributeUsage(AttributeTargets.All)]
    [Conditional("UNITY_EDITOR")]
    public class SmartLabelChildrenAttribute : Attribute
    {
        public SmartLabelChildrenAttribute()
        {
        }

        public SmartLabelChildrenAttribute(SmartLabelChildrenAttribute other)
        {
            AlignWith = other.AlignWith;
            Padding = other.Padding;
            PixelsPerCharacter = other.PixelsPerCharacter;
            Bold = other.Bold;
            Suffix = other.Suffix;
            Hue = other.Hue;
            Saturation = other.Saturation;
            Value = other.Value;
            Color = other.Color;
            HasHue = other.HasHue;
            HasPropertyColor = other.HasPropertyColor;
            HasSaturation = other.HasSaturation;
            HasValue = other.HasValue;
        }

        #region Fields and Autoproperties

        public bool HasHue { get; private set; }

        public bool HasPropertyColor { get; private set; }

        public bool HasSaturation { get; private set; }

        public bool HasValue { get; private set; }

        public float Padding { get; set; } = 4f;

        public float PixelsPerCharacter { get; set; } = 8.25f;
        public string AlignWith { get; set; }

        internal bool _bold;
        internal bool _suffix;
        internal int _hue;
        internal int _saturation;
        internal int _value;
        internal string _propertyColor;

        #endregion

        public bool Bold
        {
            get => _bold;
            set => _bold = value;
        }

        public bool Suffix
        {
            get => _suffix;
            set => _suffix = value;
        }

        public int Hue
        {
            get => _hue;
            set
            {
                _hue = Mathf.Clamp(value, 0, 359);
                HasHue = true;
            }
        }

        public int Saturation
        {
            get => _saturation;
            set
            {
                _saturation = Mathf.Clamp(value, 0, 100);
                HasSaturation = true;
            }
        }

        public int Value
        {
            get => _value;
            set
            {
                _value = Mathf.Clamp(value, 0, 100);
                HasValue = true;
            }
        }

        public string Color
        {
            get => _propertyColor;
            set
            {
                _propertyColor = value;
                HasPropertyColor = true;
            }
        }
    }
}
