#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableInt2x4 : Overridable<int2x4, OverridableInt2x4>
    {
        public OverridableInt2x4() : base(false, default)
        {
        }

        public OverridableInt2x4(bool overrideEnabled, int2x4 value) : base(overrideEnabled, value)
        {
        }

        public OverridableInt2x4(Overridable<int2x4, OverridableInt2x4> value) : base(value)
        {
        }
    }
}
