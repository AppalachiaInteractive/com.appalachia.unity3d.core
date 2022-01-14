#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableDouble4x2 : Overridable<double4x2, OverridableDouble4x2>
    {
        public OverridableDouble4x2() : base(false, default)
        {
        }

        public OverridableDouble4x2(bool overrideEnabled, double4x2 value) : base(overrideEnabled, value)
        {
        }

        public OverridableDouble4x2(Overridable<double4x2, OverridableDouble4x2> value) : base(value)
        {
        }
    }
}
