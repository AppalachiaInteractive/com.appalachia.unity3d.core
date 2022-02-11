#region

using System;
using Appalachia.Core.Objects.Models;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableSprite : Overridable<Sprite, OverridableSprite>
    {
        public OverridableSprite() : base(false, default)
        {
        }

        public OverridableSprite(bool overriding, Sprite value) : base(overriding, value)
        {
        }

        public OverridableSprite(Overridable<Sprite, OverridableSprite> value) : base(value)
        {
        }
    }
}
