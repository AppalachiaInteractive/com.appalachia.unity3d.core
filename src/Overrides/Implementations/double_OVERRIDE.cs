#region

using System;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class double_OVERRIDE : Overridable<double, double_OVERRIDE>
    { public double_OVERRIDE() : base(false, default){}
        public double_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, double value) : base(overrideEnabled, value)
        {
        }

        public double_OVERRIDE(Overridable<double, double_OVERRIDE> value) : base(value)
        {
        }
    }
}
