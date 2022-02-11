#region

using System;
using Appalachia.Core.Objects.Models;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableBool : Overridable<bool, OverridableBool>
    {
        public OverridableBool() : base(false, default)
        {
        }

        public OverridableBool(bool overriding, bool value) : base(overriding, value)
        {
        }

        public OverridableBool(Overridable<bool, OverridableBool> value) : base(value)
        {
        }
    }
}
