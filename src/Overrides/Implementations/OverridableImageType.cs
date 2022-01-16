#region

using System;
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

        public OverridableImageType(bool overrideEnabled, Image.Type value) : base(overrideEnabled, value)
        {
        }

        public OverridableImageType(Overridable<Image.Type, OverridableImageType> value) : base(value)
        {
        }
    }
}
