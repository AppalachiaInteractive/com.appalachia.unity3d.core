#region

using System;
using Appalachia.Core.Objects.Models;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class
        OverridableGradientColorKey : Overridable<GradientColorKey, OverridableGradientColorKey>
    {
        public OverridableGradientColorKey() : base(false, default)
        {
        }

        public OverridableGradientColorKey(
            bool isOverridingAllowed,
            bool overriding,
            GradientColorKey value) : base(overriding, value)
        {
        }

        public OverridableGradientColorKey(
            Overridable<GradientColorKey, OverridableGradientColorKey> value) : base(value)
        {
        }
    }
}
