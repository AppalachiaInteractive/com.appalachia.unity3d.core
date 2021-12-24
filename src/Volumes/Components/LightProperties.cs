#region

using System;
using Appalachia.Core.Volumes.Parameters;
using UnityEngine;

#endregion

namespace Appalachia.Core.Volumes.Components
{
    [Serializable]
    public sealed class LightProperties : PropertyAppaVolumeComponent<LightProperties>
    {
        public ClampedFloatParameter colorTemperature = new(5500f, 1000f, 20000f);
        public ColorParameter color = new(Color.white);
        public ExposedLightReferenceParameter target = new(default);
        public MinFloatParameter bounceIntensity = new(0f, 0f);
        public MinFloatParameter cookieSize = new(0f, 0f);
        public MinFloatParameter intensity = new(1f, 0f);
        public TextureParameter cookie = new(null);

        public Vector3Parameter rotation = new(Vector3.zero);

        public override void OverrideProperties(PropertyMaster master)
        {
            var light = target.value.Resolve(master);
            if (!light)
            {
                return;
            }

            if (rotation.overrideState)
            {
                light.transform.localRotation = Quaternion.Euler(rotation.value);
            }

            if (color.overrideState)
            {
                light.color = color.value;
            }

            if (colorTemperature.overrideState)
            {
                light.colorTemperature = colorTemperature.value;
            }

            if (intensity.overrideState)
            {
                light.intensity = intensity.value;
            }

            if (bounceIntensity.overrideState)
            {
                light.bounceIntensity = bounceIntensity.value;
            }

            if (cookie.overrideState)
            {
                light.cookie = cookie.value;
            }

            if (cookieSize.overrideState)
            {
                light.cookieSize = cookieSize.value;
            }
        }
    }
}
