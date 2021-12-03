#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableBoundingSphere : Overridable<BoundingSphere, OverridableBoundingSphere>
    {
        public OverridableBoundingSphere() : base(false, default)
        {
        }

        public OverridableBoundingSphere(
            bool isOverridingAllowed,
            bool overrideEnabled,
            BoundingSphere value) : base(overrideEnabled, value)
        {
        }

        public OverridableBoundingSphere(Overridable<BoundingSphere, OverridableBoundingSphere> value) : base(
            value
        )
        {
        }
    }
}
