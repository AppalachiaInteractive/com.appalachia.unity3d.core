#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableDouble4 : Overridable<double4, OverridableDouble4>
    {
        public OverridableDouble4() : base(false, default)
        {
        }

        public OverridableDouble4(bool overrideEnabled, double4 value) : base(overrideEnabled, value)
        {
        }

        public OverridableDouble4(Overridable<double4, OverridableDouble4> value) : base(value)
        {
        }
    }
}
