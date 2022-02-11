using System;
using Appalachia.Core.Objects.Models;
using UnityEngine.UI;

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public class OverridableGraphicRaycasterBlockingObjects : Overridable<GraphicRaycaster.BlockingObjects,
        OverridableGraphicRaycasterBlockingObjects>
    {
        public OverridableGraphicRaycasterBlockingObjects(GraphicRaycaster.BlockingObjects value) : base(
            false,
            value
        )
        {
        }

        public OverridableGraphicRaycasterBlockingObjects(
            bool overriding,
            GraphicRaycaster.BlockingObjects value) : base(overriding, value)
        {
        }

        public OverridableGraphicRaycasterBlockingObjects(
            Overridable<GraphicRaycaster.BlockingObjects, OverridableGraphicRaycasterBlockingObjects> value) :
            base(value)
        {
        }

        public OverridableGraphicRaycasterBlockingObjects() : base(false, default)
        {
        }
    }
}
