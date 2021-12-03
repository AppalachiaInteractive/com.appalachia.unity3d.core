#region

using System;
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

        public OverridableColor(bool overrideEnabled, Color value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableColor(Overridable<Color, OverridableColor> value) : base(value)
        {
        }
    }
}
