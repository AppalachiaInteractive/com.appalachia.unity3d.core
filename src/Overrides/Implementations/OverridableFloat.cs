#region

using System;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableFloat : Overridable<float, OverridableFloat>
    {
        public OverridableFloat() : base(false, default)
        {
        }

        public OverridableFloat(bool overrideEnabled, float value) : base(overrideEnabled, value)
        {
        }

        public OverridableFloat(Overridable<float, OverridableFloat> value) : base(value)
        {
        }
    }
}
