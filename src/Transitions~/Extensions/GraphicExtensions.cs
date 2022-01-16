using Appalachia.Core.Transitions.Methods.Graphic;

namespace Appalachia.Core.Transitions.Extensions
{
    public static class GraphicExtensions
    {
        public static UnityEngine.UI.Graphic colorTransition(
            this UnityEngine.UI.Graphic target,
            UnityEngine.Color value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaGraphicColor.Register(target, value, duration, ease);
            return target;
        }
    }
}
