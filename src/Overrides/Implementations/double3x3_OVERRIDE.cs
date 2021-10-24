#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class double3x3_OVERRIDE : Overridable<double3x3, double3x3_OVERRIDE>
    {
        public double3x3_OVERRIDE() : base(false, default)
        {
        }

        public double3x3_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, double3x3 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public double3x3_OVERRIDE(Overridable<double3x3, double3x3_OVERRIDE> value) : base(value)
        {
        }
    }
}
