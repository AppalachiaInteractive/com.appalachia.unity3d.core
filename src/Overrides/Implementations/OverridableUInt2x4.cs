#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableUInt2x4 : Overridable<uint2x4, OverridableUInt2x4>
    {
        public OverridableUInt2x4() : base(false, default)
        {
        }

        public OverridableUInt2x4(bool overrideEnabled, uint2x4 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableUInt2x4(Overridable<uint2x4, OverridableUInt2x4> value) : base(value)
        {
        }
    }
}
