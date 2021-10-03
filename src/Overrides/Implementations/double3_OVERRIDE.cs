#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class double3_OVERRIDE : Overridable<double3, double3_OVERRIDE>
    { public double3_OVERRIDE() : base(false, default){}
        public double3_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, double3 value) : base(overrideEnabled, value)
        {
        }

        public double3_OVERRIDE(Overridable<double3, double3_OVERRIDE> value) : base(value)
        {
        }
    }
}
