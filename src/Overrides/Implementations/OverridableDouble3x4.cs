#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableDouble3x4 : Overridable<double3x4, OverridableDouble3x4>
    {
        public OverridableDouble3x4() : base(false, default)
        {
        }

        public OverridableDouble3x4(bool overrideEnabled, double3x4 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableDouble3x4(Overridable<double3x4, OverridableDouble3x4> value) : base(value)
        {
        }
    }
}
