using System;
using UnityEngine.UI;

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableImageFillMethod : Overridable<Image.FillMethod, OverridableImageFillMethod>
    {
        public OverridableImageFillMethod() : base(false, default)
        {
        }

        public OverridableImageFillMethod(bool overrideEnabled, Image.FillMethod value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableImageFillMethod(
            Overridable<Image.FillMethod, OverridableImageFillMethod> value) : base(value)
        {
        }
    }
}
