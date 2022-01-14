#region

using System;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableUInt : Overridable<uint, OverridableUInt>
    {
        public OverridableUInt() : base(false, default)
        {
        }

        public OverridableUInt(bool overrideEnabled, uint value) : base(overrideEnabled, value)
        {
        }

        public OverridableUInt(Overridable<uint, OverridableUInt> value) : base(value)
        {
        }
    }
}
