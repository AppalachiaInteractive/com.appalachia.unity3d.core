#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableUInt3x4 : Overridable<uint3x4, OverridableUInt3x4>
    {
        public OverridableUInt3x4() : base(false, default)
        {
        }

        public OverridableUInt3x4(bool overrideEnabled, uint3x4 value) : base(overrideEnabled, value)
        {
        }

        public OverridableUInt3x4(Overridable<uint3x4, OverridableUInt3x4> value) : base(value)
        {
        }
    }
}
