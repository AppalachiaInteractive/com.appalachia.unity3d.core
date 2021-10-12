#region

using Appalachia.Core.Timing;
using UnityEngine;

#endregion

namespace Appalachia.Core.Math
{
    public static class LerpHelper
    {
        public static Vector3 LerpVisual(Vector3 start, Vector3 end, float sharpness)
        {
            return Lerp(start, end, sharpness, CoreClock.VisualDelta);
        }

        public static Vector3 LerpVisual(Vector3 start, Vector3 end, Vector3 sharpness)
        {
            return Lerp(start, end, sharpness, CoreClock.VisualDelta);
        }

        public static Vector3 LerpPhysical(Vector3 start, Vector3 end, float sharpness)
        {
            return Lerp(start, end, sharpness, CoreClock.PhysicalDelta);
        }

        public static Vector3 LerpPhysical(Vector3 start, Vector3 end, Vector3 sharpness)
        {
            return Lerp(start, end, sharpness, CoreClock.PhysicalDelta);
        }

        public static Vector3 Lerp(Vector3 start, Vector3 end, float sharpness, double deltaTime)
        {
            return LerpExact(start, end, GetTimeExact(sharpness, deltaTime));
        }

        public static Vector3 LerpExact(Vector3 start, Vector3 end, float percentage)
        {
            return Vector3.Lerp(start, end, percentage);
        }

        public static Vector3 Lerp(Vector3 start, Vector3 end, Vector3 sharpness, double deltaTime)
        {
            var xLerpTime = GetTimeExact(sharpness.x, deltaTime);
            var yLerpTime = GetTimeExact(sharpness.y, deltaTime);
            var zLerpTime = GetTimeExact(sharpness.z, deltaTime);

            var lerpedX = LerpExact(start.x, end.x, xLerpTime);
            var lerpedY = LerpExact(start.y, end.y, yLerpTime);
            var lerpedZ = LerpExact(start.z, end.z, zLerpTime);

            return new Vector3(lerpedX, lerpedY, lerpedZ);
        }

        public static float LerpVisual(float start, float end, float sharpness)
        {
            return Lerp(start, end, sharpness, CoreClock.VisualDelta);
        }

        public static float LerpPhysical(float start, float end, float sharpness)
        {
            return Lerp(start, end, sharpness, CoreClock.PhysicalDelta);
        }

        public static float Lerp(float start, float end, float sharpness, double deltaTime)
        {
            return LerpExact(start, end, GetTimeExact(sharpness, deltaTime));
        }

        public static float LerpExact(float start, float end, float percentage)
        {
            return Mathf.Lerp(start, end, percentage);
        }

        public static float SmoothStep(float start, float end, float percentage)
        {
            return Mathf.SmoothStep(start, end, percentage);
        }

        public static double LerpVisual(double start, double end, float sharpness)
        {
            return Lerp(start, end, sharpness, CoreClock.VisualDelta);
        }

        public static double LerpPhysical(double start, double end, float sharpness)
        {
            return Lerp(start, end, sharpness, CoreClock.PhysicalDelta);
        }

        public static double Lerp(double start, double end, float sharpness, double deltaTime)
        {
            return LerpExact(start, end, GetTimeExact(sharpness, deltaTime));
        }

        public static double LerpExact(double start, double end, float percentage)
        {
            return Mathd.Lerp(start, end, percentage);
        }

        public static Quaternion LerpVisual(Quaternion start, Quaternion end, float sharpness)
        {
            return Lerp(start, end, sharpness, CoreClock.VisualDelta);
        }

        public static Quaternion LerpPhysical(Quaternion start, Quaternion end, float sharpness)
        {
            return Lerp(start, end, sharpness, CoreClock.PhysicalDelta);
        }

        public static Quaternion Lerp(
            Quaternion start,
            Quaternion end,
            float sharpness,
            double deltaTime)
        {
            return LerpExact(start, end, GetTimeExact(sharpness, deltaTime));
        }

        public static Quaternion LerpExact(Quaternion start, Quaternion end, float percentage)
        {
            return Quaternion.Lerp(start, end, percentage);
        }

        public static Quaternion LookAtVisual(Transform origin, Vector3 target, float sharpness)
        {
            var direction = target - origin.position;
            var toRotation = Quaternion.FromToRotation(origin.forward, direction);
            return LerpVisual(origin.rotation, toRotation, sharpness);
        }

        public static Quaternion LookAtPhysical(Transform origin, Vector3 target, float sharpness)
        {
            var direction = target - origin.position;
            var toRotation = Quaternion.FromToRotation(origin.forward, direction);
            return LerpPhysical(origin.rotation, toRotation, sharpness);
        }

        public static Quaternion SlerpVisual(Quaternion start, Quaternion end, float sharpness)
        {
            return Slerp(start, end, sharpness, CoreClock.VisualDelta);
        }

        public static Quaternion SlerpPhysical(Quaternion start, Quaternion end, float sharpness)
        {
            return Slerp(start, end, sharpness, CoreClock.PhysicalDelta);
        }

        public static Quaternion Slerp(
            Quaternion start,
            Quaternion end,
            float sharpness,
            double deltaTime)
        {
            return SlerpExact(start, end, GetTimeExact(sharpness, deltaTime));
        }

        public static Quaternion SlerpExact(Quaternion start, Quaternion end, float percentage)
        {
            return Quaternion.Slerp(start, end, percentage);
        }

        public static float GetVisualTime(float sharpness)
        {
            return GetTimeExact(sharpness, CoreClock.VisualDelta);
        }

        public static float GetPhysicalTime(float sharpness)
        {
            return GetTimeExact(sharpness, CoreClock.PhysicalDelta);
        }

        private static float GetTimeExact(float sharpness, double deltaTime)
        {
            if (sharpness < 0)
            {
                return 1;
            }

            if (sharpness == 0)
            {
                return 0;
            }

            return (float) (1 - Mathd.Exp(-sharpness * deltaTime));
        }
    }
}
