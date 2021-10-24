#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class Keyframe_OVERRIDE : Overridable<Keyframe, Keyframe_OVERRIDE>
    {
        public Keyframe_OVERRIDE() : base(false, default)
        {
        }

        public Keyframe_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, Keyframe value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public Keyframe_OVERRIDE(Overridable<Keyframe, Keyframe_OVERRIDE> value) : base(value)
        {
        }
    }
}
