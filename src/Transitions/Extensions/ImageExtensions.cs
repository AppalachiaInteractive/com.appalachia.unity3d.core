using Appalachia.Core.Transitions.Methods.Image;

namespace Appalachia.Core.Transitions.Extensions
{
    public static class ImageExtensions
    {
        public static UnityEngine.UI.Image fillAmountTransition(
            this UnityEngine.UI.Image target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaImageFillAmount.Register(target, value, duration, ease);
            return target;
        }
    }
}
