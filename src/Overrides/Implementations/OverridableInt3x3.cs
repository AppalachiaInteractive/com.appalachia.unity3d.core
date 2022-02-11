#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableInt3x3 : Overridable<int3x3, OverridableInt3x3>
    {
        public OverridableInt3x3() : base(false, default)
        {
        }

        public OverridableInt3x3(bool overriding, int3x3 value) : base(overriding, value)
        {
        }

        public OverridableInt3x3(Overridable<int3x3, OverridableInt3x3> value) : base(value)
        {
        }
    }
}
