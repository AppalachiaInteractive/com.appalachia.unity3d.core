#region

using System;
using Appalachia.Core.Objects.Models;
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

        public OverridableUInt3x3(bool overriding, uint3x3 value) : base(overriding, value)
        {
        }

        public OverridableUInt3x3(Overridable<uint3x3, OverridableUInt3x3> value) : base(value)
        {
        }
    }
}
