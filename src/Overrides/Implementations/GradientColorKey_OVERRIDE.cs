#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class GradientColorKey_OVERRIDE : Overridable<GradientColorKey, GradientColorKey_OVERRIDE>
    { public GradientColorKey_OVERRIDE() : base(false, default){}
        public GradientColorKey_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, GradientColorKey value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public GradientColorKey_OVERRIDE(Overridable<GradientColorKey, GradientColorKey_OVERRIDE> value) : base(value)
        {
        }
    }
}
