#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class GradientAlphaKey_OVERRIDE : Overridable<GradientAlphaKey, GradientAlphaKey_OVERRIDE>
    { public GradientAlphaKey_OVERRIDE() : base(false, default){}
        public GradientAlphaKey_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, GradientAlphaKey value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public GradientAlphaKey_OVERRIDE(Overridable<GradientAlphaKey, GradientAlphaKey_OVERRIDE> value) : base(value)
        {
        }
    }
}
