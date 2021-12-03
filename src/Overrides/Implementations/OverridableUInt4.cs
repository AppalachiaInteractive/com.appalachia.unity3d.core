#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableUInt4 : Overridable<uint4, OverridableUInt4>
    {
        public OverridableUInt4() : base(false, default)
        {
        }

        public OverridableUInt4(bool overrideEnabled, uint4 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableUInt4(Overridable<uint4, OverridableUInt4> value) : base(value)
        {
        }
    }
}
