#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class Color_OVERRIDE : Overridable<Color, Color_OVERRIDE>
    { public Color_OVERRIDE() : base(false, default){}
        public Color_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, Color value) : base(overrideEnabled, value)
        {
        }

        public Color_OVERRIDE(Overridable<Color, Color_OVERRIDE> value) : base(value)
        {
        }
    }
}
