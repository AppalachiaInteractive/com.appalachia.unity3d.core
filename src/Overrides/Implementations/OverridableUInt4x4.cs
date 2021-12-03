#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableUInt4x4 : Overridable<uint4x4, OverridableUInt4x4>
    {
        public OverridableUInt4x4() : base(false, default)
        {
        }

        public OverridableUInt4x4(bool overrideEnabled, uint4x4 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableUInt4x4(Overridable<uint4x4, OverridableUInt4x4> value) : base(value)
        {
        }
    }
}
