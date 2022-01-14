#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableDouble2 : Overridable<double2, OverridableDouble2>
    {
        public OverridableDouble2() : base(false, default)
        {
        }

        public OverridableDouble2(bool overrideEnabled, double2 value) : base(overrideEnabled, value)
        {
        }

        public OverridableDouble2(Overridable<double2, OverridableDouble2> value) : base(value)
        {
        }
    }
}
