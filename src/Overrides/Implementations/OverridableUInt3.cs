#region

using System;
using Appalachia.Core.Objects.Models;
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

        public OverridableUInt3(bool overriding, uint3 value) : base(overriding, value)
        {
        }

        public OverridableUInt3(Overridable<uint3, OverridableUInt3> value) : base(value)
        {
        }
    }
}
