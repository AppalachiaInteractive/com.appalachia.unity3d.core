#region

using System;

#endregion

namespace Appalachia.Core.Overridding.Implementations
{
    [Serializable]
    public sealed class decimal_OVERRIDE : Overridable<decimal, decimal_OVERRIDE>
    { public decimal_OVERRIDE() : base(false, default){}
        public decimal_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, decimal value) : base(overrideEnabled, value)
        {
        }

        public decimal_OVERRIDE(Overridable<decimal, decimal_OVERRIDE> value) : base(value)
        {
        }
    }
}
