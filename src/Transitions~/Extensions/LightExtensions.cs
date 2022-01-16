using Appalachia.Core.Transitions.Methods.Light;

namespace Appalachia.Core.Transitions.Extensions
{
    public static class LightExtensions
    {
        public static UnityEngine.Light colorTransition(
            this UnityEngine.Light target,
            UnityEngine.Color value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaLightColor.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.Light intensityTransition(
            this UnityEngine.Light target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaLightIntensity.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.Light rangeTransition(
            this UnityEngine.Light target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaLightRange.Register(target, value, duration, ease);
            return target;
        }
    }
}
