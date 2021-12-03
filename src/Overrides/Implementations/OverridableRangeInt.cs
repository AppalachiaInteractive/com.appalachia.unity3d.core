#region

using System;
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

        public OverridableRangeInt(bool overrideEnabled, RangeInt value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableRangeInt(Overridable<RangeInt, OverridableRangeInt> value) : base(value)
        {
        }
    }
}
