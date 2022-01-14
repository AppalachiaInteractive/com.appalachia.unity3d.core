#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableInt4x3 : Overridable<int4x3, OverridableInt4x3>
    {
        public OverridableInt4x3() : base(false, default)
        {
        }

        public OverridableInt4x3(bool overrideEnabled, int4x3 value) : base(overrideEnabled, value)
        {
        }

        public OverridableInt4x3(Overridable<int4x3, OverridableInt4x3> value) : base(value)
        {
        }
    }
}
