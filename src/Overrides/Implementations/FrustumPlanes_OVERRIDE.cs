#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class FrustumPlanes_OVERRIDE : Overridable<FrustumPlanes, FrustumPlanes_OVERRIDE>
    { public FrustumPlanes_OVERRIDE() : base(false, default){}
        public FrustumPlanes_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, FrustumPlanes value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public FrustumPlanes_OVERRIDE(Overridable<FrustumPlanes, FrustumPlanes_OVERRIDE> value) : base(value)
        {
        }
    }
}
