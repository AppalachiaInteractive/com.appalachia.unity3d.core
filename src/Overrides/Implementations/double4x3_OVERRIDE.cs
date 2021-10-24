#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class double4x3_OVERRIDE : Overridable<double4x3, double4x3_OVERRIDE>
    {
        public double4x3_OVERRIDE() : base(false, default)
        {
        }

        public double4x3_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, double4x3 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public double4x3_OVERRIDE(Overridable<double4x3, double4x3_OVERRIDE> value) : base(value)
        {
        }
    }
}
