#region

using System;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableByte : Overridable<byte, OverridableByte>
    {
        public OverridableByte() : base(false, default)
        {
        }

        public OverridableByte(bool overrideEnabled, byte value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableByte(Overridable<byte, OverridableByte> value) : base(value)
        {
        }
    }
}
