using Appalachia.Core.Transitions.Methods.CanvasGroup;

namespace Appalachia.Core.Transitions.Extensions
{
    public static class CanvasGroupExtensions
    {
        public static UnityEngine.CanvasGroup alphaTransition(
            this UnityEngine.CanvasGroup target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaCanvasGroupAlpha.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.CanvasGroup blocksRaycastsTransition(
            this UnityEngine.CanvasGroup target,
            bool value,
            float duration)
        {
            AppaCanvasGroupBlocksRaycasts.Register(target, value, duration);
            return target;
        }

        public static UnityEngine.CanvasGroup interactableTransition(
            this UnityEngine.CanvasGroup target,
            bool value,
            float duration)
        {
            AppaCanvasGroupInteractable.Register(target, value, duration);
            return target;
        }
    }
}
