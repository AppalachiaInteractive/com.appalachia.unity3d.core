#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableBoundsInt : Overridable<BoundsInt, OverridableBoundsInt>
    {
        public OverridableBoundsInt() : base(false, default)
        {
        }

        public OverridableBoundsInt(bool overrideEnabled, BoundsInt value) : base(overrideEnabled, value)
        {
        }

        public OverridableBoundsInt(Overridable<BoundsInt, OverridableBoundsInt> value) : base(value)
        {
        }
    }
}
