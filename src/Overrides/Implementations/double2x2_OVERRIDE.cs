#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class double2x2_OVERRIDE : Overridable<double2x2, double2x2_OVERRIDE>
    {
        public double2x2_OVERRIDE() : base(false, default)
        {
        }

        public double2x2_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, double2x2 value) :
            base(overrideEnabled, value)
        {
        }

        public double2x2_OVERRIDE(Overridable<double2x2, double2x2_OVERRIDE> value) : base(value)
        {
        }
    }
}
