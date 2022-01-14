#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableFrustumPlanes : Overridable<FrustumPlanes, OverridableFrustumPlanes>
    {
        public OverridableFrustumPlanes() : base(false, default)
        {
        }

        public OverridableFrustumPlanes(bool overrideEnabled, FrustumPlanes value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableFrustumPlanes(Overridable<FrustumPlanes, OverridableFrustumPlanes> value) : base(
            value
        )
        {
        }
    }
}
