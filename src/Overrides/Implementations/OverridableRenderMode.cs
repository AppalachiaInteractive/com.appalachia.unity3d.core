using System;
using Appalachia.Core.Objects.Models;
using UnityEngine;

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public class OverridableRenderMode : Overridable<RenderMode, OverridableRenderMode>
    {
        public OverridableRenderMode(RenderMode value) : base(false, value)
        {
        }

        public OverridableRenderMode(bool overriding, RenderMode value) : base(overriding, value)
        {
        }

        public OverridableRenderMode(Overridable<RenderMode, OverridableRenderMode> value) : base(value)
        {
        }

        public OverridableRenderMode() : base(false, default)
        {
        }
    }
}
