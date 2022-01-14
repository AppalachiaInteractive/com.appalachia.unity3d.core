#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableUInt2x2 : Overridable<uint2x2, OverridableUInt2x2>
    {
        public OverridableUInt2x2() : base(false, default)
        {
        }

        public OverridableUInt2x2(bool overrideEnabled, uint2x2 value) : base(overrideEnabled, value)
        {
        }

        public OverridableUInt2x2(Overridable<uint2x2, OverridableUInt2x2> value) : base(value)
        {
        }
    }
}
