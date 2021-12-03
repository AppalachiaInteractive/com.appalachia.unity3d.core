#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableInt3x4 : Overridable<int3x4, OverridableInt3x4>
    {
        public OverridableInt3x4() : base(false, default)
        {
        }

        public OverridableInt3x4(bool overrideEnabled, int3x4 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableInt3x4(Overridable<int3x4, OverridableInt3x4> value) : base(value)
        {
        }
    }
}
