#region

using System;
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

        public OverridableSprite(bool overrideEnabled, Sprite value) : base(overrideEnabled, value)
        {
        }

        public OverridableSprite(Overridable<Sprite, OverridableSprite> value) : base(value)
        {
        }
    }
}
