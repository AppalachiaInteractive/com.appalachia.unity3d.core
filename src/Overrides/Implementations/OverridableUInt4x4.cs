#region

using System;
using Appalachia.Core.Objects.Models;
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

        public OverridableUInt4x4(bool overriding, uint4x4 value) : base(overriding, value)
        {
        }

        public OverridableUInt4x4(Overridable<uint4x4, OverridableUInt4x4> value) : base(value)
        {
        }
    }
}
