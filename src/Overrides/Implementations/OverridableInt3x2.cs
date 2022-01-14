#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableInt3x2 : Overridable<int3x2, OverridableInt3x2>
    {
        public OverridableInt3x2() : base(false, default)
        {
        }

        public OverridableInt3x2(bool overrideEnabled, int3x2 value) : base(overrideEnabled, value)
        {
        }

        public OverridableInt3x2(Overridable<int3x2, OverridableInt3x2> value) : base(value)
        {
        }
    }
}
