using System;
using Appalachia.Core.Objects.Models;
using UnityEngine.UI;

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public class OverridableCanvasScalerScreenMatchMode : Overridable<CanvasScaler.ScreenMatchMode,
        OverridableCanvasScalerScreenMatchMode>
    {
        public OverridableCanvasScalerScreenMatchMode(CanvasScaler.ScreenMatchMode value) : base(false, value)
        {
        }

        public OverridableCanvasScalerScreenMatchMode(
            bool overrideEnabled,
            CanvasScaler.ScreenMatchMode value) : base(overrideEnabled, value)
        {
        }

        public OverridableCanvasScalerScreenMatchMode(
            Overridable<CanvasScaler.ScreenMatchMode, OverridableCanvasScalerScreenMatchMode> value) : base(
            value
        )
        {
        }

        public OverridableCanvasScalerScreenMatchMode() : base(false, default)
        {
        }
    }
}
