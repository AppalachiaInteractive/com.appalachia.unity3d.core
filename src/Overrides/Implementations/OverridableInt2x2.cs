#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableInt2x2 : Overridable<int2x2, OverridableInt2x2>
    {
        public OverridableInt2x2() : base(false, default)
        {
        }

        public OverridableInt2x2(bool overrideEnabled, int2x2 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableInt2x2(Overridable<int2x2, OverridableInt2x2> value) : base(value)
        {
        }
    }
}
