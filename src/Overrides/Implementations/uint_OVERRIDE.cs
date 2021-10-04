#region

using System;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class uint_OVERRIDE : Overridable<uint, uint_OVERRIDE>
    {
        public uint_OVERRIDE() : base(false, default)
        {
        }

        public uint_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, uint value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public uint_OVERRIDE(Overridable<uint, uint_OVERRIDE> value) : base(value)
        {
        }
    }
}
