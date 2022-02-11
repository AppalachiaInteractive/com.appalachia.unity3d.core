#region

using System;
using Appalachia.Core.Objects.Models;
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

        public OverridableUInt2x4(bool overriding, uint2x4 value) : base(overriding, value)
        {
        }

        public OverridableUInt2x4(Overridable<uint2x4, OverridableUInt2x4> value) : base(value)
        {
        }
    }
}
