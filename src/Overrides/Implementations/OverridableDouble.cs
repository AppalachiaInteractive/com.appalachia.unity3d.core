#region

using System;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableDouble : Overridable<double, OverridableDouble>
    {
        public OverridableDouble() : base(false, default)
        {
        }

        public OverridableDouble(bool overrideEnabled, double value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableDouble(Overridable<double, OverridableDouble> value) : base(value)
        {
        }
    }
}
