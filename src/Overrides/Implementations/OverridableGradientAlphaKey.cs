#region

using System;
using Appalachia.Core.Objects.Models;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class
        OverridableGradientAlphaKey : Overridable<GradientAlphaKey, OverridableGradientAlphaKey>
    {
        public OverridableGradientAlphaKey() : base(false, default)
        {
        }

        public OverridableGradientAlphaKey(
            bool isOverridingAllowed,
            bool overriding,
            GradientAlphaKey value) : base(overriding, value)
        {
        }

        public OverridableGradientAlphaKey(
            Overridable<GradientAlphaKey, OverridableGradientAlphaKey> value) : base(value)
        {
        }
    }
}
