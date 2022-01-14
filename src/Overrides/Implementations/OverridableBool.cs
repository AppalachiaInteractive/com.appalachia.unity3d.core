#region

using System;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableBool : Overridable<bool, OverridableBool>
    {
        public OverridableBool() : base(false, default)
        {
        }

        public OverridableBool(bool overrideEnabled, bool value) : base(overrideEnabled, value)
        {
        }

        public OverridableBool(Overridable<bool, OverridableBool> value) : base(value)
        {
        }
    }
}
