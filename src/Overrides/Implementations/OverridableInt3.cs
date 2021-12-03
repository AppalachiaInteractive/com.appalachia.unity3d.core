#region

using System;
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

        public OverridableInt3(bool overrideEnabled, int3 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableInt3(Overridable<int3, OverridableInt3> value) : base(value)
        {
        }
    }
}
