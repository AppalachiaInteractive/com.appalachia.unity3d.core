#region

using System;
using Appalachia.Core.Objects.Models;
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

        public OverridableGradient(bool overriding, Gradient value) : base(overriding, value)
        {
        }

        public OverridableGradient(Overridable<Gradient, OverridableGradient> value) : base(value)
        {
        }
    }
}
