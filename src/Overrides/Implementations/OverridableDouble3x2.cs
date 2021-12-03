#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableDouble3x2 : Overridable<double3x2, OverridableDouble3x2>
    {
        public OverridableDouble3x2() : base(false, default)
        {
        }

        public OverridableDouble3x2(bool overrideEnabled, double3x2 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableDouble3x2(Overridable<double3x2, OverridableDouble3x2> value) : base(value)
        {
        }
    }
}
