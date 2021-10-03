#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class double4_OVERRIDE : Overridable<double4, double4_OVERRIDE>
    { public double4_OVERRIDE() : base(false, default){}
        public double4_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, double4 value) : base(overrideEnabled, value)
        {
        }

        public double4_OVERRIDE(Overridable<double4, double4_OVERRIDE> value) : base(value)
        {
        }
    }
}
