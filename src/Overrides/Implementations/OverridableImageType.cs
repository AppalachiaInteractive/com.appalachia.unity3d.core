#region

using System;
using Appalachia.Core.Objects.Models;
using UnityEngine.UI;

#endregion

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public sealed class OverridableImageType : Overridable<Image.Type, OverridableImageType>
    {
        public OverridableImageType() : base(false, default)
        {
        }

        public OverridableImageType(bool overriding, Image.Type value) : base(overriding, value)
        {
        }

        public OverridableImageType(Overridable<Image.Type, OverridableImageType> value) : base(value)
        {
        }
    }
}
