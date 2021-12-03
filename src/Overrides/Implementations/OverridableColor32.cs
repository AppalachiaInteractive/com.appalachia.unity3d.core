#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableColor32 : Overridable<Color32, OverridableColor32>
    {
        public OverridableColor32() : base(false, default)
        {
        }

        public OverridableColor32(bool overrideEnabled, Color32 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableColor32(Overridable<Color32, OverridableColor32> value) : base(value)
        {
        }
    }
}
