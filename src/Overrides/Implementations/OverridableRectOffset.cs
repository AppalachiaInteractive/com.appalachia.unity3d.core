#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableRectOffset : Overridable<RectOffset, OverridableRectOffset>
    {
        public OverridableRectOffset() : base(false, default)
        {
        }

        public OverridableRectOffset(bool overrideEnabled, RectOffset value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableRectOffset(Overridable<RectOffset, OverridableRectOffset> value) : base(value)
        {
        }
    }
}
