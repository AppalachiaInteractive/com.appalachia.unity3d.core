#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableGradientColorKey : Overridable<GradientColorKey, OverridableGradientColorKey>
    {
        public OverridableGradientColorKey() : base(false, default)
        {
        }

        public OverridableGradientColorKey(
            bool isOverridingAllowed,
            bool overrideEnabled,
            GradientColorKey value) : base(overrideEnabled, value)
        {
        }

        public OverridableGradientColorKey(
            Overridable<GradientColorKey, OverridableGradientColorKey> value) : base(value)
        {
        }
    }
}
