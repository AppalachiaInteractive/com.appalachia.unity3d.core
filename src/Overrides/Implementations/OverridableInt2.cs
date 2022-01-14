#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableInt2 : Overridable<int2, OverridableInt2>
    {
        public OverridableInt2() : base(false, default)
        {
        }

        public OverridableInt2(bool overrideEnabled, int2 value) : base(overrideEnabled, value)
        {
        }

        public OverridableInt2(Overridable<int2, OverridableInt2> value) : base(value)
        {
        }
    }
}
