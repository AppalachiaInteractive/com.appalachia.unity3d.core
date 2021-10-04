#region

using System;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class byte_OVERRIDE : Overridable<byte, byte_OVERRIDE>
    {
        public byte_OVERRIDE() : base(false, default)
        {
        }

        public byte_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, byte value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public byte_OVERRIDE(Overridable<byte, byte_OVERRIDE> value) : base(value)
        {
        }
    }
}
