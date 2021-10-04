#region

using System;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class short_OVERRIDE : Overridable<short, short_OVERRIDE>
    {
        public short_OVERRIDE() : base(false, default)
        {
        }

        public short_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, short value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public short_OVERRIDE(Overridable<short, short_OVERRIDE> value) : base(value)
        {
        }
    }
}
