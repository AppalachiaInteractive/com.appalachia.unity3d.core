#region

using System;
using Appalachia.Core.Objects.Models;
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

        public OverridableUInt3x2(bool overriding, uint3x2 value) : base(overriding, value)
        {
        }

        public OverridableUInt3x2(Overridable<uint3x2, OverridableUInt3x2> value) : base(value)
        {
        }
    }
}
