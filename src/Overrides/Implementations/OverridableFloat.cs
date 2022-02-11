#region

using System;
using Appalachia.Core.Objects.Models;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableFloat : Overridable<float, OverridableFloat>
    {
        public OverridableFloat() : base(false, default)
        {
        }

        public OverridableFloat(bool overriding, float value) : base(overriding, value)
        {
        }

        public OverridableFloat(Overridable<float, OverridableFloat> value) : base(value)
        {
        }
    }
}
