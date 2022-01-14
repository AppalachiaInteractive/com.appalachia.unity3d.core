#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableUInt4x2 : Overridable<uint4x2, OverridableUInt4x2>
    {
        public OverridableUInt4x2() : base(false, default)
        {
        }

        public OverridableUInt4x2(bool overrideEnabled, uint4x2 value) : base(overrideEnabled, value)
        {
        }

        public OverridableUInt4x2(Overridable<uint4x2, OverridableUInt4x2> value) : base(value)
        {
        }
    }
}
