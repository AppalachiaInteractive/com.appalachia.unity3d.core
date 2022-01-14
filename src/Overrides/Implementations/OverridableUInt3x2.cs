#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableUInt3x2 : Overridable<uint3x2, OverridableUInt3x2>
    {
        public OverridableUInt3x2() : base(false, default)
        {
        }

        public OverridableUInt3x2(bool overrideEnabled, uint3x2 value) : base(overrideEnabled, value)
        {
        }

        public OverridableUInt3x2(Overridable<uint3x2, OverridableUInt3x2> value) : base(value)
        {
        }
    }
}
