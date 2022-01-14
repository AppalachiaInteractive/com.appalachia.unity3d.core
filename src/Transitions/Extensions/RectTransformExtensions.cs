using Appalachia.Core.Transitions.Methods.RectTransform;

namespace Appalachia.Core.Transitions.Extensions
{
    public static class RectTransformExtensions
    {
        public static UnityEngine.RectTransform anchoredPositionTransition(
            this UnityEngine.RectTransform target,
            UnityEngine.Vector2 value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaRectTransformAnchoredPosition.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.RectTransform anchoredPositionTransition_x(
            this UnityEngine.RectTransform target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaRectTransformAnchoredPosition_x.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.RectTransform anchoredPositionTransition_y(
            this UnityEngine.RectTransform target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaRectTransformAnchoredPosition_y.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.RectTransform anchorMaxTransition(
            this UnityEngine.RectTransform target,
            UnityEngine.Vector2 value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaRectTransformAnchorMax.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.RectTransform anchorMaxTransition_x(
            this UnityEngine.RectTransform target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaRectTransformAnchorMax_x.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.RectTransform anchorMaxTransition_y(
            this UnityEngine.RectTransform target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaRectTransformAnchorMax_y.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.RectTransform anchorMinTransition(
            this UnityEngine.RectTransform target,
            UnityEngine.Vector2 value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaRectTransformAnchorMin.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.RectTransform anchorMinTransition_x(
            this UnityEngine.RectTransform target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaRectTransformAnchorMin_x.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.RectTransform anchorMinTransition_y(
            this UnityEngine.RectTransform target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaRectTransformAnchorMin_y.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.RectTransform offsetMaxTransition(
            this UnityEngine.RectTransform target,
            UnityEngine.Vector2 value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaRectTransformOffsetMax.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.RectTransform offsetMaxTransition_x(
            this UnityEngine.RectTransform target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaRectTransformOffsetMax_x.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.RectTransform offsetMaxTransition_y(
            this UnityEngine.RectTransform target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaRectTransformOffsetMax_y.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.RectTransform offsetMinTransition(
            this UnityEngine.RectTransform target,
            UnityEngine.Vector2 value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaRectTransformOffsetMin.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.RectTransform offsetMinTransition_x(
            this UnityEngine.RectTransform target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaRectTransformOffsetMin_x.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.RectTransform offsetMinTransition_y(
            this UnityEngine.RectTransform target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaRectTransformOffsetMin_y.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.RectTransform pivotTransition(
            this UnityEngine.RectTransform target,
            UnityEngine.Vector2 value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaRectTransformPivot.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.RectTransform pivotTransition_x(
            this UnityEngine.RectTransform target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaRectTransformPivot_x.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.RectTransform pivotTransition_y(
            this UnityEngine.RectTransform target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaRectTransformPivot_y.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.RectTransform sizeDeltaTransition(
            this UnityEngine.RectTransform target,
            UnityEngine.Vector2 value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaRectTransformSizeDelta.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.RectTransform sizeDeltaTransition_x(
            this UnityEngine.RectTransform target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaRectTransformSizeDelta_x.Register(target, value, duration, ease);
            return target;
        }

        public static UnityEngine.RectTransform sizeDeltaTransition_y(
            this UnityEngine.RectTransform target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaRectTransformSizeDelta_y.Register(target, value, duration, ease);
            return target;
        }
    }
}
