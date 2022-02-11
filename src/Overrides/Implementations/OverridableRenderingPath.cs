using System;
using Appalachia.Core.Objects.Models;
using UnityEngine;

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public class OverridableRenderingPath : Overridable<RenderingPath, OverridableRenderingPath>
    {
        public OverridableRenderingPath(RenderingPath value) : base(false, value)
        {
        }

        public OverridableRenderingPath(bool overriding, RenderingPath value) : base(overriding, value)
        {
        }

        public OverridableRenderingPath(Overridable<RenderingPath, OverridableRenderingPath> value) : base(
            value
        )
        {
        }

        public OverridableRenderingPath() : base(false, default)
        {
        }
    }
}
