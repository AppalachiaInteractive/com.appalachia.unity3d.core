#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableDouble4x4 : Overridable<double4x4, OverridableDouble4x4>
    {
        public OverridableDouble4x4() : base(false, default)
        {
        }

        public OverridableDouble4x4(bool overrideEnabled, double4x4 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableDouble4x4(Overridable<double4x4, OverridableDouble4x4> value) : base(value)
        {
        }
    }
}
