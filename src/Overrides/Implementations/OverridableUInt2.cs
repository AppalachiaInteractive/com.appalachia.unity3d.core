#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableUInt2 : Overridable<uint2, OverridableUInt2>
    {
        public OverridableUInt2() : base(false, default)
        {
        }

        public OverridableUInt2(bool overrideEnabled, uint2 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableUInt2(Overridable<uint2, OverridableUInt2> value) : base(value)
        {
        }
    }
}
