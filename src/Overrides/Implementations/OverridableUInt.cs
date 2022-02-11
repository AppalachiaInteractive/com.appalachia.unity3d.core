#region

using System;
using Appalachia.Core.Objects.Models;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableUInt : Overridable<uint, OverridableUInt>
    {
        public OverridableUInt() : base(false, default)
        {
        }

        public OverridableUInt(bool overriding, uint value) : base(overriding, value)
        {
        }

        public OverridableUInt(Overridable<uint, OverridableUInt> value) : base(value)
        {
        }
    }
}
