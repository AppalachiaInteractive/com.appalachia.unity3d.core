using System;
using Appalachia.Core.Objects.Models;
using UnityEngine;

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public class OverridableDepthTextureMode : Overridable<DepthTextureMode, OverridableDepthTextureMode>
    {
        public OverridableDepthTextureMode(DepthTextureMode value) : base(false, value)
        {
        }

        public OverridableDepthTextureMode(bool overriding, DepthTextureMode value) : base(overriding, value)
        {
        }

        public OverridableDepthTextureMode(
            Overridable<DepthTextureMode, OverridableDepthTextureMode> value) : base(value)
        {
        }

        public OverridableDepthTextureMode() : base(false, default)
        {
        }
    }
}
