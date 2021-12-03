#region

using System;
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

        public OverridableInt3x3(bool overrideEnabled, int3x3 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableInt3x3(Overridable<int3x3, OverridableInt3x3> value) : base(value)
        {
        }
    }
}
