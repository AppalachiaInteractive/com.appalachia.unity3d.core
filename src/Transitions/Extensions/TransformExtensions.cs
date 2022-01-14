using Appalachia.Core.Transitions.Methods.Transform;
using UnityEngine;

namespace Appalachia.Core.Transitions.Extensions
{
    public static class TransformExtensions
    {
        public static Transform eulerAnglesTransform(
            this Transform target,
            Vector3 position,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformEulerAngles.Register(target, position, duration, ease);
            return target;
        }

        public static Transform localEulerAnglesTransform(
            this Transform target,
            Vector3 position,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformLocalEulerAngles.Register(target, position, duration, ease);
            return target;
        }

        public static Transform localPositionTransition(
            this Transform target,
            Vector3 value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformLocalPosition.Register(target, value, duration, ease);
            return target;
        }

        public static Transform localPositionTransition_x(
            this Transform target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformLocalPosition_x.Register(target, value, duration, ease);
            return target;
        }

        public static Transform localPositionTransition_xy(
            this Transform target,
            Vector2 value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformLocalPosition_xy.Register(target, value, duration, ease);
            return target;
        }

        public static Transform localPositionTransition_y(
            this Transform target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformLocalPosition_y.Register(target, value, duration, ease);
            return target;
        }

        public static Transform localPositionTransition_z(
            this Transform target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformLocalPosition_z.Register(target, value, duration, ease);
            return target;
        }

        public static Transform localRotationTransition(
            this Transform target,
            Quaternion rotation,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformLocalRotation.Register(target, rotation, duration, ease);
            return target;
        }

        public static Transform localScaleTransition(
            this Transform target,
            Vector3 value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformLocalScale.Register(target, value, duration, ease);
            return target;
        }

        public static Transform localScaleTransition_x(
            this Transform target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformLocalScale_x.Register(target, value, duration, ease);
            return target;
        }

        public static Transform localScaleTransition_xy(
            this Transform target,
            Vector2 value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformLocalScale_xy.Register(target, value, duration, ease);
            return target;
        }

        public static Transform localScaleTransition_y(
            this Transform target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformLocalScale_y.Register(target, value, duration, ease);
            return target;
        }

        public static Transform localScaleTransition_z(
            this Transform target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformLocalScale_z.Register(target, value, duration, ease);
            return target;
        }

        public static Transform positionTransition(
            this Transform target,
            Vector3 value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformPosition.Register(target, value, duration, ease);
            return target;
        }

        public static Transform positionTransition_x(
            this Transform target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformPosition_X.Register(target, value, duration, ease);
            return target;
        }

        public static Transform positionTransition_xy(
            this Transform target,
            Vector2 value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformPosition_xy.Register(target, value, duration, ease);
            return target;
        }

        public static Transform positionTransition_y(
            this Transform target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformPosition_Y.Register(target, value, duration, ease);
            return target;
        }

        public static Transform positionTransition_z(
            this Transform target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformPosition_Z.Register(target, value, duration, ease);
            return target;
        }

        public static Transform RotateTransition(
            this Transform target,
            float x,
            float y,
            float z,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformRotate.Register(target, new Vector3(x, y, z), Space.Self, duration, ease);
            return target;
        }

        public static Transform RotateTransition(
            this Transform target,
            Vector3 eulerAngles,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformRotate.Register(target, eulerAngles, Space.Self, duration, ease);
            return target;
        }

        public static Transform RotateTransition(
            this Transform target,
            float x,
            float y,
            float z,
            Space space,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformRotate.Register(target, new Vector3(x, y, z), space, duration, ease);
            return target;
        }

        public static Transform RotateTransition(
            this Transform target,
            Vector3 eulerAngles,
            Space space,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformRotate.Register(target, eulerAngles, space, duration, ease);
            return target;
        }

        public static Transform rotationTransition(
            this Transform target,
            Quaternion rotation,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformRotation.Register(target, rotation, duration, ease);
            return target;
        }

        public static Transform TranslateTransition(
            this Transform target,
            float x,
            float y,
            float z,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformTranslate.Register(target, new Vector3(x, y, z), Space.Self, duration, ease);
            return target;
        }

        public static Transform TranslateTransition(
            this Transform target,
            Vector3 translation,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformTranslate.Register(target, translation, Space.Self, duration, ease);
            return target;
        }

        public static Transform TranslateTransition(
            this Transform target,
            float x,
            float y,
            float z,
            Space space,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformTranslate.Register(target, new Vector3(x, y, z), space, duration, ease);
            return target;
        }

        public static Transform TranslateTransition(
            this Transform target,
            Vector3 translation,
            Space space,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformTranslate.Register(target, translation, space, duration, ease);
            return target;
        }

        public static Transform TranslateTransition(
            this Transform target,
            float x,
            float y,
            float z,
            Transform relativeTo,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformTranslate.Register(target, new Vector3(x, y, z), relativeTo, duration, ease);
            return target;
        }

        public static Transform TranslateTransition(
            this Transform target,
            Vector3 translation,
            Transform relativeTo,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTransformTranslate.Register(target, translation, relativeTo, duration, ease);
            return target;
        }
    }
}
