#region

using System;
using Appalachia.Core.Objects.Models;
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

        public OverridableBoundsInt(bool overriding, BoundsInt value) : base(overriding, value)
        {
        }

        public OverridableBoundsInt(Overridable<BoundsInt, OverridableBoundsInt> value) : base(value)
        {
        }
    }
}
