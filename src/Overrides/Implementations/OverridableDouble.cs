#region

using System;
using Appalachia.Core.Objects.Models;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableDouble : Overridable<double, OverridableDouble>
    {
        public OverridableDouble() : base(false, default)
        {
        }

        public OverridableDouble(bool overriding, double value) : base(overriding, value)
        {
        }

        public OverridableDouble(Overridable<double, OverridableDouble> value) : base(value)
        {
        }
    }
}
