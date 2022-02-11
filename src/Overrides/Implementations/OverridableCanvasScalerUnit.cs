using System;
using Appalachia.Core.Objects.Models;
using UnityEngine.UI;

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public class OverridableCanvasScalerUnit : Overridable<CanvasScaler.Unit, OverridableCanvasScalerUnit>
    {
        public OverridableCanvasScalerUnit(CanvasScaler.Unit value) : base(false, value)
        {
        }

        public OverridableCanvasScalerUnit(bool overrideEnabled, CanvasScaler.Unit value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableCanvasScalerUnit(
            Overridable<CanvasScaler.Unit, OverridableCanvasScalerUnit> value) : base(value)
        {
        }

        public OverridableCanvasScalerUnit() : base(false, default)
        {
        }
    }
}
