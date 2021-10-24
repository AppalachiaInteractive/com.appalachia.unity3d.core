#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class double2_OVERRIDE : Overridable<double2, double2_OVERRIDE>
    {
        public double2_OVERRIDE() : base(false, default)
        {
        }

        public double2_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, double2 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public double2_OVERRIDE(Overridable<double2, double2_OVERRIDE> value) : base(value)
        {
        }
    }
}
