using System;
using Appalachia.Core.Objects.Models;
using UnityEngine.UI;

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableImageFillMethod : Overridable<Image.FillMethod, OverridableImageFillMethod>
    {
        public OverridableImageFillMethod() : base(false, default)
        {
        }

        public OverridableImageFillMethod(bool overriding, Image.FillMethod value) : base(overriding, value)
        {
        }

        public OverridableImageFillMethod(
            Overridable<Image.FillMethod, OverridableImageFillMethod> value) : base(value)
        {
        }
    }
}
