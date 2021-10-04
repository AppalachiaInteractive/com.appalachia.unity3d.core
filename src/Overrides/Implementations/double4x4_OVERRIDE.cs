#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class double4x4_OVERRIDE : Overridable<double4x4, double4x4_OVERRIDE>
    { public double4x4_OVERRIDE() : base(false, default){}
        public double4x4_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, double4x4 value) : base(overrideEnabled, value)
        {
        }

        public double4x4_OVERRIDE(Overridable<double4x4, double4x4_OVERRIDE> value) : base(value)
        {
        }
    }
}