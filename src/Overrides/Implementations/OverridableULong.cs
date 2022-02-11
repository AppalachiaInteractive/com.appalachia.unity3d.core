#region

using System;
using Appalachia.Core.Objects.Models;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableULong : Overridable<ulong, OverridableULong>
    {
        public OverridableULong() : base(false, default)
        {
        }

        public OverridableULong(bool overriding, ulong value) : base(overriding, value)
        {
        }

        public OverridableULong(Overridable<ulong, OverridableULong> value) : base(value)
        {
        }
    }
}
