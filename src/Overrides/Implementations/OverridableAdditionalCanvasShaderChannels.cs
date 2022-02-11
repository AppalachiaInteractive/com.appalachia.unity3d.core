using System;
using Appalachia.Core.Objects.Models;
using UnityEngine;

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public class OverridableAdditionalCanvasShaderChannels : Overridable<AdditionalCanvasShaderChannels,
        OverridableAdditionalCanvasShaderChannels>
    {
        public OverridableAdditionalCanvasShaderChannels(AdditionalCanvasShaderChannels value) : base(
            false,
            value
        )
        {
        }

        public OverridableAdditionalCanvasShaderChannels(
            bool overriding,
            AdditionalCanvasShaderChannels value) : base(overriding, value)
        {
        }

        public OverridableAdditionalCanvasShaderChannels(
            Overridable<AdditionalCanvasShaderChannels, OverridableAdditionalCanvasShaderChannels> value) :
            base(value)
        {
        }

        public OverridableAdditionalCanvasShaderChannels() : base(false, default)
        {
        }
    }
}
