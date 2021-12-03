#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableInt4x2 : Overridable<int4x2, OverridableInt4x2>
    {
        public OverridableInt4x2() : base(false, default)
        {
        }

        public OverridableInt4x2(bool overrideEnabled, int4x2 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableInt4x2(Overridable<int4x2, OverridableInt4x2> value) : base(value)
        {
        }
    }
}
