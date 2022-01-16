using Appalachia.Core.Transitions.Methods.SpriteRenderer;

namespace Appalachia.Core.Transitions.Extensions
{
    public static class SpriteRendererExtensions
    {
        public static UnityEngine.SpriteRenderer colorTransition(
            this UnityEngine.SpriteRenderer target,
            UnityEngine.Color value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaSpriteRendererColor.Register(target, value, duration, ease);
            return target;
        }
    }
}
