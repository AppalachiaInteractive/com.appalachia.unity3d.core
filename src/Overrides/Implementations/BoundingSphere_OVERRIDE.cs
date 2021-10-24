#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class BoundingSphere_OVERRIDE : Overridable<BoundingSphere, BoundingSphere_OVERRIDE>
    {
        public BoundingSphere_OVERRIDE() : base(false, default)
        {
        }

        public BoundingSphere_OVERRIDE(
            bool isOverridingAllowed,
            bool overrideEnabled,
            BoundingSphere value) : base(overrideEnabled, value)
        {
        }

        public BoundingSphere_OVERRIDE(Overridable<BoundingSphere, BoundingSphere_OVERRIDE> value) : base(
            value
        )
        {
        }
    }
}
