#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class double2x4_OVERRIDE : Overridable<double2x4, double2x4_OVERRIDE>
    {
        public double2x4_OVERRIDE() : base(false, default)
        {
        }

        public double2x4_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, double2x4 value) :
            base(overrideEnabled, value)
        {
        }

        public double2x4_OVERRIDE(Overridable<double2x4, double2x4_OVERRIDE> value) : base(value)
        {
        }
    }
}
