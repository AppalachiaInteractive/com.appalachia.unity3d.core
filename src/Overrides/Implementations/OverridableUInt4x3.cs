#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableUInt4x3 : Overridable<uint4x3, OverridableUInt4x3>
    {
        public OverridableUInt4x3() : base(false, default)
        {
        }

        public OverridableUInt4x3(bool overrideEnabled, uint4x3 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableUInt4x3(Overridable<uint4x3, OverridableUInt4x3> value) : base(value)
        {
        }
    }
}
