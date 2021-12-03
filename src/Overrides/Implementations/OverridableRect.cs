#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableRect : Overridable<Rect, OverridableRect>
    {
        public OverridableRect() : base(false, default)
        {
        }

        public OverridableRect(bool overrideEnabled, Rect value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableRect(Overridable<Rect, OverridableRect> value) : base(value)
        {
        }
    }
}
