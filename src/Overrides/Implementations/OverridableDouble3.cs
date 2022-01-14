#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableDouble3 : Overridable<double3, OverridableDouble3>
    {
        public OverridableDouble3() : base(false, default)
        {
        }

        public OverridableDouble3(bool overrideEnabled, double3 value) : base(overrideEnabled, value)
        {
        }

        public OverridableDouble3(Overridable<double3, OverridableDouble3> value) : base(value)
        {
        }
    }
}
