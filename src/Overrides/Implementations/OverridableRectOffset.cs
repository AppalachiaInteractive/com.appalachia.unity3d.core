#region

using System;
using Appalachia.Core.Objects.Models;
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

        public OverridableRectOffset(bool overriding, RectOffset value) : base(overriding, value)
        {
        }

        public OverridableRectOffset(Overridable<RectOffset, OverridableRectOffset> value) : base(value)
        {
        }
    }
}
