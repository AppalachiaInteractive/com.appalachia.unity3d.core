#region

using System;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class ulong_OVERRIDE : Overridable<ulong, ulong_OVERRIDE>
    {
        public ulong_OVERRIDE() : base(false, default)
        {
        }

        public ulong_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, ulong value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public ulong_OVERRIDE(Overridable<ulong, ulong_OVERRIDE> value) : base(value)
        {
        }
    }
}
