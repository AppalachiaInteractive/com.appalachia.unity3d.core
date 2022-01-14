#region

using System;
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

        public OverridableKeyframe(bool overrideEnabled, Keyframe value) : base(overrideEnabled, value)
        {
        }

        public OverridableKeyframe(Overridable<Keyframe, OverridableKeyframe> value) : base(value)
        {
        }
    }
}
