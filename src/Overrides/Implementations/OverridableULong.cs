#region

using System;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableULong : Overridable<ulong, OverridableULong>
    {
        public OverridableULong() : base(false, default)
        {
        }

        public OverridableULong(bool overrideEnabled, ulong value) : base(overrideEnabled, value)
        {
        }

        public OverridableULong(Overridable<ulong, OverridableULong> value) : base(value)
        {
        }
    }
}
