#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableUInt3x3 : Overridable<uint3x3, OverridableUInt3x3>
    {
        public OverridableUInt3x3() : base(false, default)
        {
        }

        public OverridableUInt3x3(bool overrideEnabled, uint3x3 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableUInt3x3(Overridable<uint3x3, OverridableUInt3x3> value) : base(value)
        {
        }
    }
}
