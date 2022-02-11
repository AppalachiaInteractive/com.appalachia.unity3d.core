#region

using System;
using Appalachia.Core.Objects.Models;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableInt3 : Overridable<int3, OverridableInt3>
    {
        public OverridableInt3() : base(false, default)
        {
        }

        public OverridableInt3(bool overriding, int3 value) : base(overriding, value)
        {
        }

        public OverridableInt3(Overridable<int3, OverridableInt3> value) : base(value)
        {
        }
    }
}
