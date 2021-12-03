#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableDouble4x3 : Overridable<double4x3, OverridableDouble4x3>
    {
        public OverridableDouble4x3() : base(false, default)
        {
        }

        public OverridableDouble4x3(bool overrideEnabled, double4x3 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableDouble4x3(Overridable<double4x3, OverridableDouble4x3> value) : base(value)
        {
        }
    }
}
