#region

using System;
using Appalachia.Core.Objects.Models;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableInt : Overridable<int, OverridableInt>
    {
        public OverridableInt() : base(false, default)
        {
        }

        public OverridableInt(bool overriding, int value) : base(overriding, value)
        {
        }

        public OverridableInt(Overridable<int, OverridableInt> value) : base(value)
        {
        }
    }
}
