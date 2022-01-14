#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableDouble2x3 : Overridable<double2x3, OverridableDouble2x3>
    {
        public OverridableDouble2x3() : base(false, default)
        {
        }

        public OverridableDouble2x3(bool overrideEnabled, double2x3 value) : base(overrideEnabled, value)
        {
        }

        public OverridableDouble2x3(Overridable<double2x3, OverridableDouble2x3> value) : base(value)
        {
        }
    }
}
