#region

using System;
using Appalachia.Core.Objects.Models;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableRangeInt : Overridable<RangeInt, OverridableRangeInt>
    {
        public OverridableRangeInt() : base(false, default)
        {
        }

        public OverridableRangeInt(bool overriding, RangeInt value) : base(overriding, value)
        {
        }

        public OverridableRangeInt(Overridable<RangeInt, OverridableRangeInt> value) : base(value)
        {
        }
    }
}
