#region

using System;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class long_OVERRIDE : Overridable<long, long_OVERRIDE>
    {
        public long_OVERRIDE() : base(false, default)
        {
        }

        public long_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, long value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public long_OVERRIDE(Overridable<long, long_OVERRIDE> value) : base(value)
        {
        }
    }
}
