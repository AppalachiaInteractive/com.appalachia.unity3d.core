#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableGradientAlphaKey : Overridable<GradientAlphaKey, OverridableGradientAlphaKey>
    {
        public OverridableGradientAlphaKey() : base(false, default)
        {
        }

        public OverridableGradientAlphaKey(
            bool isOverridingAllowed,
            bool overrideEnabled,
            GradientAlphaKey value) : base(overrideEnabled, value)
        {
        }

        public OverridableGradientAlphaKey(
            Overridable<GradientAlphaKey, OverridableGradientAlphaKey> value) : base(value)
        {
        }
    }
}
