#region

using System;
using Appalachia.Core.Objects.Models;
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

        public OverridableUInt3x4(bool overriding, uint3x4 value) : base(overriding, value)
        {
        }

        public OverridableUInt3x4(Overridable<uint3x4, OverridableUInt3x4> value) : base(value)
        {
        }
    }
}
