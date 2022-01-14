#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableGradient : Overridable<Gradient, OverridableGradient>
    {
        public OverridableGradient() : base(false, default)
        {
        }

        public OverridableGradient(bool overrideEnabled, Gradient value) : base(overrideEnabled, value)
        {
        }

        public OverridableGradient(Overridable<Gradient, OverridableGradient> value) : base(value)
        {
        }
    }
}
