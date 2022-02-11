#region

using System;
using Appalachia.Core.Objects.Models;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableColor : Overridable<Color, OverridableColor>
    {
        public OverridableColor() : base(false, default)
        {
        }

        public OverridableColor(bool overriding, Color value) : base(overriding, value)
        {
        }

        public OverridableColor(Overridable<Color, OverridableColor> value) : base(value)
        {
        }
    }
}
