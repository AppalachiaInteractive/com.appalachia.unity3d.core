#region

using System;
using Appalachia.Core.Objects.Models;
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

        public OverridableFrustumPlanes(bool overriding, FrustumPlanes value) : base(overriding, value)
        {
        }

        public OverridableFrustumPlanes(Overridable<FrustumPlanes, OverridableFrustumPlanes> value) : base(
            value
        )
        {
        }
    }
}
