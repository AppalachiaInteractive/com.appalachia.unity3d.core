#region

using System;
using Appalachia.Core.Objects.Models;
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

        public OverridableBoundingSphere(bool isOverridingAllowed, bool overriding, BoundingSphere value) :
            base(overriding, value)
        {
        }

        public OverridableBoundingSphere(Overridable<BoundingSphere, OverridableBoundingSphere> value) : base(
            value
        )
        {
        }
    }
}
