#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableUInt2x3 : Overridable<uint2x3, OverridableUInt2x3>
    {
        public OverridableUInt2x3() : base(false, default)
        {
        }

        public OverridableUInt2x3(bool overrideEnabled, uint2x3 value) : base(overrideEnabled, value)
        {
        }

        public OverridableUInt2x3(Overridable<uint2x3, OverridableUInt2x3> value) : base(value)
        {
        }
    }
}
