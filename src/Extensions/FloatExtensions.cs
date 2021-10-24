#region

using UnityEngine;

#endregion

namespace Appalachia.Core.Extensions
{
    public static class FloatExtensions
    {
        private const float NegativeEpsilon = -0.00001f;
        private const float PositiveEpsilon = 0.00001f;

        public static bool RoughlyEqual(this float value, float other)
        {
            var diff = value - other;

            return (diff < PositiveEpsilon) && (diff > NegativeEpsilon);
        }

        public static bool RoughlyGreaterThanOrEqualTo(this float value, float comparison)
        {
            return (value >= comparison) || RoughlyEqual(value, comparison);
        }

        public static bool RoughlyLessThanOrEqualTo(this float value, float comparison)
        {
            return (value <= comparison) || RoughlyEqual(value, comparison);
        }

        public static bool RoughlyOne(this float value)
        {
            return RoughlyEqual(value, 1);
        }

        public static bool RoughlyOneOrLess(this float value)
        {
            return RoughlyLessThanOrEqualTo(value, 1);
        }

        public static bool RoughlyOneOrMore(this float value)
        {
            return RoughlyGreaterThanOrEqualTo(value, 1);
        }

        public static bool RoughlyOutsidePositiveNegativeBounds(this float value, float comparison)
        {
            return (value > comparison) ||
                   value.RoughlyEqual(comparison) ||
                   value.RoughlyEqual(-comparison) ||
                   (value < -comparison);
        }

        public static bool RoughlyWithinPositiveNegativeBounds(this float value, float comparison)
        {
            return ((value < comparison) || value.RoughlyEqual(comparison)) &&
                   ((value > -comparison) || value.RoughlyEqual(-comparison));
        }

        public static bool RoughlyZero(this float value)
        {
            return RoughlyEqual(value, 0);
        }

        public static bool RoughlyZeroOrLess(this float value)
        {
            return (value <= 0) || RoughlyEqual(value, 0);
        }

        public static bool RoughlyZeroOrMore(this float value)
        {
            return (value >= 0) || RoughlyEqual(value, 0);
        }

        public static float Clamp(this float value, float min, float max)
        {
            return value < min
                ? min
                : value > max
                    ? max
                    : value;
        }

        public static float Clamp0to1(this float value)
        {
            return Clamp(value, 0, 1);
        }

        public static float ClampAbs(this float value, float min, float max)
        {
            return value < 0 ? Clamp(-1 * value, min, max) : Clamp(value, min, max);
        }

        public static float ClampAbs0to1(this float value)
        {
            return ClampAbs(value, 0, 1);
        }

        public static float ClampAngle(this float angle, float min, float max)
        {
            if ((angle < -360) || (angle > 360))
            {
                angle %= 360.0f;
            }

            return Clamp(angle, min, max);
        }

        public static float HalfSquared(this float value)
        {
            return .5f * value * value;
        }

        public static float Normalized(this float value, float min, float max)
        {
            return (max - value) / (max - min);
        }

        public static float Remap(
            this float value,
            float minOriginal,
            float maxOriginal,
            float minNew,
            float maxNew)
        {
            return value.Normalized(minOriginal, maxOriginal).RemapFrom0to1(minNew, maxNew);
        }

        public static float RemapFrom0to1(this float time, float minNew, float maxNew)
        {
            return Mathf.Lerp(minNew, maxNew, time);
        }

        public static float Squared(this float value)
        {
            return value * value;
        }
    }
}
