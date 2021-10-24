#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class double4x2_OVERRIDE : Overridable<double4x2, double4x2_OVERRIDE>
    {
        public double4x2_OVERRIDE() : base(false, default)
        {
        }

        public double4x2_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, double4x2 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public double4x2_OVERRIDE(Overridable<double4x2, double4x2_OVERRIDE> value) : base(value)
        {
        }
    }
}
