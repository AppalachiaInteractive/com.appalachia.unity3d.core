#region

using System;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableLong : Overridable<long, OverridableLong>
    {
        public OverridableLong() : base(false, default)
        {
        }

        public OverridableLong(bool overrideEnabled, long value) : base(overrideEnabled, value)
        {
        }

        public OverridableLong(Overridable<long, OverridableLong> value) : base(value)
        {
        }
    }
}
