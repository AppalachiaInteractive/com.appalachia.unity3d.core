using System;
using Appalachia.Core.Objects.Models;
using UnityEngine.UI;

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public class
        OverridableCanvasScalerScaleMode : Overridable<CanvasScaler.ScaleMode,
            OverridableCanvasScalerScaleMode>
    {
        public OverridableCanvasScalerScaleMode(CanvasScaler.ScaleMode value) : base(false, value)
        {
        }

        public OverridableCanvasScalerScaleMode(bool overrideEnabled, CanvasScaler.ScaleMode value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableCanvasScalerScaleMode(
            Overridable<CanvasScaler.ScaleMode, OverridableCanvasScalerScaleMode> value) : base(value)
        {
        }

        public OverridableCanvasScalerScaleMode() : base(false, default)
        {
        }
    }
}
