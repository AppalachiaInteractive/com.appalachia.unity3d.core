#region

using System;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableUShort : Overridable<ushort, OverridableUShort>
    {
        public OverridableUShort() : base(false, default)
        {
        }

        public OverridableUShort(bool overrideEnabled, ushort value) : base(overrideEnabled, value)
        {
        }

        public OverridableUShort(Overridable<ushort, OverridableUShort> value) : base(value)
        {
        }
    }
}
