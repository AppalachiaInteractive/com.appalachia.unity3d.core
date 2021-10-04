#region

using System;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class ushort_OVERRIDE : Overridable<ushort, ushort_OVERRIDE>
    {
        public ushort_OVERRIDE() : base(false, default)
        {
        }

        public ushort_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, ushort value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public ushort_OVERRIDE(Overridable<ushort, ushort_OVERRIDE> value) : base(value)
        {
        }
    }
}
