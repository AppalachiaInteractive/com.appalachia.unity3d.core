#region

using System;
using Appalachia.Core.Objects.Models;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableLong : Overridable<long, OverridableLong>
    {
        public OverridableLong() : base(false, default)
        {
        }

        public OverridableLong(bool overriding, long value) : base(overriding, value)
        {
        }

        public OverridableLong(Overridable<long, OverridableLong> value) : base(value)
        {
        }
    }
}
