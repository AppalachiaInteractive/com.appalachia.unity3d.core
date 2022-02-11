#region

using System;
using Appalachia.Core.Objects.Models;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableDecimal : Overridable<decimal, OverridableDecimal>
    {
        public OverridableDecimal() : base(false, default)
        {
        }

        public OverridableDecimal(bool overriding, decimal value) : base(overriding, value)
        {
        }

        public OverridableDecimal(Overridable<decimal, OverridableDecimal> value) : base(value)
        {
        }
    }
}
