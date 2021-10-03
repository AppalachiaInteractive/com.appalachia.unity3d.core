#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class double3x2_OVERRIDE : Overridable<double3x2, double3x2_OVERRIDE>
    { public double3x2_OVERRIDE() : base(false, default){}
        public double3x2_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, double3x2 value) : base(overrideEnabled, value)
        {
        }

        public double3x2_OVERRIDE(Overridable<double3x2, double3x2_OVERRIDE> value) : base(value)
        {
        }
    }
}
