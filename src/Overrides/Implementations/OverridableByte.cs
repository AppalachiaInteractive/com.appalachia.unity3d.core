#region

using System;
using Appalachia.Core.Objects.Models;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableByte : Overridable<byte, OverridableByte>
    {
        public OverridableByte() : base(false, default)
        {
        }

        public OverridableByte(bool overriding, byte value) : base(overriding, value)
        {
        }

        public OverridableByte(Overridable<byte, OverridableByte> value) : base(value)
        {
        }
    }
}
