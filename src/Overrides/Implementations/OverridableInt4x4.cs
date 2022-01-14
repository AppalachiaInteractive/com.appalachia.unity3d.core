#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableInt4x4 : Overridable<int4x4, OverridableInt4x4>
    {
        public OverridableInt4x4() : base(false, default)
        {
        }

        public OverridableInt4x4(bool overrideEnabled, int4x4 value) : base(overrideEnabled, value)
        {
        }

        public OverridableInt4x4(Overridable<int4x4, OverridableInt4x4> value) : base(value)
        {
        }
    }
}
