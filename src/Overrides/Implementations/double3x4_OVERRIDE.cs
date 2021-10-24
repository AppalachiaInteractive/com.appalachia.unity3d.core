#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class double3x4_OVERRIDE : Overridable<double3x4, double3x4_OVERRIDE>
    {
        public double3x4_OVERRIDE() : base(false, default)
        {
        }

        public double3x4_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, double3x4 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public double3x4_OVERRIDE(Overridable<double3x4, double3x4_OVERRIDE> value) : base(value)
        {
        }
    }
}
