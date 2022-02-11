#region

using System;
using Appalachia.Core.Objects.Models;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableKeyframe : Overridable<Keyframe, OverridableKeyframe>
    {
        public OverridableKeyframe() : base(false, default)
        {
        }

        public OverridableKeyframe(bool overriding, Keyframe value) : base(overriding, value)
        {
        }

        public OverridableKeyframe(Overridable<Keyframe, OverridableKeyframe> value) : base(value)
        {
        }
    }
}
