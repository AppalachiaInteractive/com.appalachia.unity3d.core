#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableUInt3 : Overridable<uint3, OverridableUInt3>
    {
        public OverridableUInt3() : base(false, default)
        {
        }

        public OverridableUInt3(bool overrideEnabled, uint3 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableUInt3(Overridable<uint3, OverridableUInt3> value) : base(value)
        {
        }
    }
}
