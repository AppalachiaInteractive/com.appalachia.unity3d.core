using System;
using Appalachia.Core.Objects.Models;
using UnityEngine;

namespace Appalachia.Core.Overrides.Implementations
{
    [Serializable]
    public class OverridableCameraGateFitMode : Overridable<Camera.GateFitMode, OverridableCameraGateFitMode>
    {
        public OverridableCameraGateFitMode(Camera.GateFitMode value) : base(false, value)
        {
        }

        public OverridableCameraGateFitMode(bool overriding, Camera.GateFitMode value) : base(
            overriding,
            value
        )
        {
        }

        public OverridableCameraGateFitMode(
            Overridable<Camera.GateFitMode, OverridableCameraGateFitMode> value) : base(value)
        {
        }

        public OverridableCameraGateFitMode() : base(false, default)
        {
        }
    }
}
