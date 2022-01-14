#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableInt2x3 : Overridable<int2x3, OverridableInt2x3>
    {
        public OverridableInt2x3() : base(false, default)
        {
        }

        public OverridableInt2x3(bool overrideEnabled, int2x3 value) : base(overrideEnabled, value)
        {
        }

        public OverridableInt2x3(Overridable<int2x3, OverridableInt2x3> value) : base(value)
        {
        }
    }
}
