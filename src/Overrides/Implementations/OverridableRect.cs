#region

using System;
using Appalachia.Core.Objects.Models;
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

        public OverridableRect(bool overriding, Rect value) : base(overriding, value)
        {
        }

        public OverridableRect(Overridable<Rect, OverridableRect> value) : base(value)
        {
        }
    }
}
