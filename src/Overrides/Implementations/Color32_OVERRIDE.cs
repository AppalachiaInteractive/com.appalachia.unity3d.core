#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class Color32_OVERRIDE : Overridable<Color32, Color32_OVERRIDE>
    {
        public Color32_OVERRIDE() : base(false, default)
        {
        }

        public Color32_OVERRIDE(bool isOverridingAllowed, bool overrideEnabled, Color32 value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public Color32_OVERRIDE(Overridable<Color32, Color32_OVERRIDE> value) : base(value)
        {
        }
    }
}
