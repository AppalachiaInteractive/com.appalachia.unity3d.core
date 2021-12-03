#region

using System;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableDecimal : Overridable<decimal, OverridableDecimal>
    {
        public OverridableDecimal() : base(false, default)
        {
        }

        public OverridableDecimal(bool overrideEnabled, decimal value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableDecimal(Overridable<decimal, OverridableDecimal> value) : base(value)
        {
        }
    }
}
