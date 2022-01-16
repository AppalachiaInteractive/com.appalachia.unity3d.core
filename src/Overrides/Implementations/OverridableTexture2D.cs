using UnityEngine;

namespace Appalachia.Core.Overrides.Implementations
{
    public class OverridableTexture2D : Overridable<Texture2D, OverridableTexture2D>
    {
        public OverridableTexture2D(bool overrideEnabled, Texture2D value) : base(overrideEnabled, value)
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
