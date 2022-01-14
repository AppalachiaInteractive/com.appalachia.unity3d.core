#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableDouble2x4 : Overridable<double2x4, OverridableDouble2x4>
    {
        public OverridableDouble2x4() : base(false, default)
        {
        }

        public OverridableDouble2x4(bool overrideEnabled, double2x4 value) : base(overrideEnabled, value)
        {
        }

        public OverridableDouble2x4(Overridable<double2x4, OverridableDouble2x4> value) : base(value)
        {
        }
    }
}
