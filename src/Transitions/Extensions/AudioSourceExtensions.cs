using Appalachia.Core.Transitions.Methods.AudioSource;

namespace Appalachia.Core.Transitions.Extensions
{
    public static class AudioSourceExtensions
    {
        public static UnityEngine.AudioSource panStereoTransition(
            this UnityEngine.AudioSource target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaAudioSourcePanStereo.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.AudioSource pitchTransition(
            this UnityEngine.AudioSource target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaAudioSourcePitch.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.AudioSource spatialBlendTransition(
            this UnityEngine.AudioSource target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaAudioSourceSpatialBlend.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.AudioSource volumeTransition(
            this UnityEngine.AudioSource target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaAudioSourceVolume.Register(target, value, duration, ease);
            return target;
        }
    }
}
