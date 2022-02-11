using System;
using Appalachia.Core.Objects.Models;
using UnityEngine;

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public class OverridableTexture2D : Overridable<Texture2D, OverridableTexture2D>
    {
        public OverridableTexture2D(bool overriding, Texture2D value) : base(overriding, value)
        {
        }

        public OverridableTexture2D(Overridable<Texture2D, OverridableTexture2D> value) : base(value)
        {
        }

        public OverridableTexture2D() : base(false, default)
        {
        }
    }
}
