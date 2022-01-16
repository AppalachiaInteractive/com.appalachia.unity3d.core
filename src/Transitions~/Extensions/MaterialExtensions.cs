using Appalachia.Core.Transitions.Methods.Material;
using UnityEngine;

namespace Appalachia.Core.Transitions.Extensions
{
    public static class MaterialExtensions
    {
        public static Material colorTransition(
            this Material target,
            string property,
            Color color,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaMaterialColor.Register(target, property, color, duration, ease);
            return target;
        }

        public static Material floatTransition(
            this Material target,
            string property,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaMaterialFloat.Register(target, property, value, duration, ease);
            return target;
        }

        public static Material vectorTransition(
            this Material target,
            string property,
            Vector4 value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaMaterialVector.Register(target, property, value, duration, ease);
            return target;
        }
    }
}
