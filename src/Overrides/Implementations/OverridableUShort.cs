#region

using System;
using Appalachia.Core.Objects.Models;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableUShort : Overridable<ushort, OverridableUShort>
    {
        public OverridableUShort() : base(false, default)
        {
        }

        public OverridableUShort(bool overriding, ushort value) : base(overriding, value)
        {
        }

        public OverridableUShort(Overridable<ushort, OverridableUShort> value) : base(value)
        {
        }
    }
}
