#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class double2x3_OVERRIDE : Overridable<double2x3, double2x3_OVERRIDE>
    {
        public double2x3_OVERRIDE() : base(false, default)
        {
        }

        public double2x3_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, double2x3 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public double2x3_OVERRIDE(Overridable<double2x3, double2x3_OVERRIDE> value) : base(value)
        {
        }
    }
}
