#region

using System;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableInt : Overridable<int, OverridableInt>
    {
        public OverridableInt() : base(false, default)
        {
        }

        public OverridableInt(bool overrideEnabled, int value) : base(overrideEnabled, value)
        {
        }

        public OverridableInt(Overridable<int, OverridableInt> value) : base(value)
        {
        }
    }
}
