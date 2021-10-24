#region

using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

#endregion

namespace Appalachia.Core.Extensions
{
    public static class mathex
    {
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this float a, float b, float threshold = .00001f)
        {
            return math.abs(a - b) < threshold;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this double a, double b, float threshold = .00001f)
        {
            return math.abs(a - b) < threshold;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this half a, half b, float threshold = .00001f)
        {
            return math.abs(a - b) < threshold;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this double2 a, double2 b, float threshold = .00001f)
        {
            var diff = a - b;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this double3 a, double3 b, float threshold = .00001f)
        {
            var diff = a - b;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this double4 a, double4 b, float threshold = .00001f)
        {
            var diff = a - b;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this double2x2 a, double2x2 b, float threshold = .00001f)
        {
            var diff = a - b;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this double3x2 a, double3x2 b, float threshold = .00001f)
        {
            var diff = a - b;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this double4x2 a, double4x2 b, float threshold = .00001f)
        {
            var diff = a - b;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this double2x3 a, double2x3 b, float threshold = .00001f)
        {
            var diff = a - b;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this double3x3 a, double3x3 b, float threshold = .00001f)
        {
            var diff = a - b;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this double4x3 a, double4x3 b, float threshold = .00001f)
        {
            var diff = a - b;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this double2x4 a, double2x4 b, float threshold = .00001f)
        {
            var diff = a - b;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this double3x4 a, double3x4 b, float threshold = .00001f)
        {
            var diff = a - b;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this double4x4 a, double4x4 b, float threshold = .00001f)
        {
            var diff = a - b;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this float2 a, float2 b, float threshold = .00001f)
        {
            var diff = a - b;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this float3 a, float3 b, float threshold = .00001f)
        {
            var diff = a - b;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this float4 a, float4 b, float threshold = .00001f)
        {
            var diff = a - b;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this float2x2 a, float2x2 b, float threshold = .00001f)
        {
            var diff = a - b;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this float3x2 a, float3x2 b, float threshold = .00001f)
        {
            var diff = a - b;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this float4x2 a, float4x2 b, float threshold = .00001f)
        {
            var diff = a - b;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this float2x3 a, float2x3 b, float threshold = .00001f)
        {
            var diff = a - b;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this float3x3 a, float3x3 b, float threshold = .00001f)
        {
            var diff = a - b;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this float4x3 a, float4x3 b, float threshold = .00001f)
        {
            var diff = a - b;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this float2x4 a, float2x4 b, float threshold = .00001f)
        {
            var diff = a - b;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this float3x4 a, float3x4 b, float threshold = .00001f)
        {
            var diff = a - b;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this float4x4 a, float4x4 b, float threshold = .00001f)
        {
            var diff = a - b;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool approx(this quaternion a, quaternion b, float threshold = .00001f)
        {
            var diff = a.value - b.value;
            var checks = (diff < threshold) & (diff > -threshold);
            return checks.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double average(this double2 value)
        {
            return (value.x + value.y) / 2.0;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double average(this double3 value)
        {
            return (value.x + value.y + value.z) / 3.0;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double average(this double4 value)
        {
            return (value.x + value.y + value.z + value.w) / 4.0;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double normalize(this double value, double2 range)
        {
            return (value - range.x) / (range.y - range.x);
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double normalize(this double value, double low, double high)
        {
            return (value - low) / (high - low);
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2 normalize(this double2 value, double2 low, double2 high)
        {
            return new(normalize(value.x, low.x, high.x), normalize(value.y, low.y, high.y));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double3 normalize(this double3 value, double3 low, double3 high)
        {
            return new(normalize(value.x, low.x, high.x), normalize(value.y, low.y, high.y), normalize(
                value.z,
                low.z,
                high.z
            ));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double4 normalize(this double4 value, double4 low, double4 high)
        {
            return new(normalize(value.x, low.x, high.x), normalize(value.y, low.y, high.y), normalize(
                value.z,
                low.z,
                high.z
            ), normalize(value.w, low.w, high.w));
        }

        /// <summary>
        ///     <para>Returns the angle in degrees between from and to.</para>
        /// </summary>
        /// <param name="from">The vector from which the angular difference is measured.</param>
        /// <param name="to">The vector to which the angular difference is measured.</param>
        /// <returns>
        ///     <para>The angle in degrees between the two vectors.</para>
        /// </returns>
        public static float angle(float3 from, float3 to)
        {
            var num = (float) math.sqrt((double) math.lengthsq(from) * math.lengthsq(to));
            return num < 1.00000000362749E-15
                ? 0.0f
                : (float) math.acos((double) math.clamp(math.dot(from, to) / num, -1f, 1f)) * 57.29578f;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float average(this float2 value)
        {
            return (value.x + value.y) / 2.0f;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float average(this float3 value)
        {
            return (value.x + value.y + value.z) / 3.0f;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float average(this float4 value)
        {
            return (value.x + value.y + value.z + value.w) / 4.0f;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float average(this Vector2 value)
        {
            return (value.x + value.y) / 2.0f;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float average(this Vector3 value)
        {
            return (value.x + value.y + value.z) / 3.0f;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float average(this Vector4 value)
        {
            return (value.x + value.y + value.z + value.w) / 4.0f;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float max(float x, float y, float z)
        {
            return math.max(x, math.max(y, z));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float max(float x, float y, float z, float a)
        {
            return math.max(x, math.max(y, math.max(z, a)));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float max(float x, float y, float z, float a, float b)
        {
            return math.max(x, math.max(y, math.max(z, math.max(a, b))));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float max(
            float x,
            float y,
            float z,
            float a,
            float b,
            float c)
        {
            return math.max(x, math.max(y, math.max(z, math.max(a, math.max(b, c)))));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float min(float x, float y, float z)
        {
            return math.min(x, math.min(y, z));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float min(float x, float y, float z, float a)
        {
            return math.min(x, math.min(y, math.min(z, a)));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float min(float x, float y, float z, float a, float b)
        {
            return math.min(x, math.min(y, math.min(z, math.min(a, b))));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float min(
            float x,
            float y,
            float z,
            float a,
            float b,
            float c)
        {
            return math.min(x, math.min(y, math.min(z, math.min(a, math.min(b, c)))));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float normalize(this float value, float2 range)
        {
            return (value - range.x) / (range.y - range.x);
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float normalize(this float value, float low, float high)
        {
            return (value - low) / (high - low);
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 max(float2 x, float2 y, float2 z)
        {
            return math.max(x, math.max(y, z));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 max(float2 x, float2 y, float2 z, float2 a)
        {
            return math.max(x, math.max(y, math.max(z, a)));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 max(float2 x, float2 y, float2 z, float2 a, float2 b)
        {
            return math.max(x, math.max(y, math.max(z, math.max(a, b))));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 max(
            float2 x,
            float2 y,
            float2 z,
            float2 a,
            float2 b,
            float2 c)
        {
            return math.max(x, math.max(y, math.max(z, math.max(a, math.max(b, c)))));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 min(float2 x, float2 y, float2 z)
        {
            return math.min(x, math.min(y, z));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 min(float2 x, float2 y, float2 z, float2 a)
        {
            return math.min(x, math.min(y, math.min(z, a)));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 min(float2 x, float2 y, float2 z, float2 a, float2 b)
        {
            return math.min(x, math.min(y, math.min(z, math.min(a, b))));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 min(
            float2 x,
            float2 y,
            float2 z,
            float2 a,
            float2 b,
            float2 c)
        {
            return math.min(x, math.min(y, math.min(z, math.min(a, math.min(b, c)))));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 normalize(this float2 value, float2 low, float2 high)
        {
            return new(normalize(value.x, low.x, high.x), normalize(value.y, low.y, high.y));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 max(float3 x, float3 y, float3 z)
        {
            return math.max(x, math.max(y, z));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 max(float3 x, float3 y, float3 z, float3 a)
        {
            return math.max(x, math.max(y, math.max(z, a)));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 max(float3 x, float3 y, float3 z, float3 a, float3 b)
        {
            return math.max(x, math.max(y, math.max(z, math.max(a, b))));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 max(
            float3 x,
            float3 y,
            float3 z,
            float3 a,
            float3 b,
            float3 c)
        {
            return math.max(x, math.max(y, math.max(z, math.max(a, math.max(b, c)))));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 min(float3 x, float3 y, float3 z)
        {
            return math.min(x, math.min(y, z));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 min(float3 x, float3 y, float3 z, float3 a)
        {
            return math.min(x, math.min(y, math.min(z, a)));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 min(float3 x, float3 y, float3 z, float3 a, float3 b)
        {
            return math.min(x, math.min(y, math.min(z, math.min(a, b))));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 min(
            float3 x,
            float3 y,
            float3 z,
            float3 a,
            float3 b,
            float3 c)
        {
            return math.min(x, math.min(y, math.min(z, math.min(a, math.min(b, c)))));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 normalize(this float3 value, float3 low, float3 high)
        {
            return new(normalize(value.x, low.x, high.x), normalize(value.y, low.y, high.y), normalize(
                value.z,
                low.z,
                high.z
            ));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 max(float4 x, float4 y, float4 z)
        {
            return math.max(x, math.max(y, z));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 max(float4 x, float4 y, float4 z, float4 a)
        {
            return math.max(x, math.max(y, math.max(z, a)));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 max(float4 x, float4 y, float4 z, float4 a, float4 b)
        {
            return math.max(x, math.max(y, math.max(z, math.max(a, b))));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 max(
            float4 x,
            float4 y,
            float4 z,
            float4 a,
            float4 b,
            float4 c)
        {
            return math.max(x, math.max(y, math.max(z, math.max(a, math.max(b, c)))));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 min(float4 x, float4 y, float4 z)
        {
            return math.min(x, math.min(y, z));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 min(float4 x, float4 y, float4 z, float4 a)
        {
            return math.min(x, math.min(y, math.min(z, a)));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 min(float4 x, float4 y, float4 z, float4 a, float4 b)
        {
            return math.min(x, math.min(y, math.min(z, math.min(a, b))));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 min(
            float4 x,
            float4 y,
            float4 z,
            float4 a,
            float4 b,
            float4 c)
        {
            return math.min(x, math.min(y, math.min(z, math.min(a, math.min(b, c)))));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 normalize(this float4 value, float4 low, float4 high)
        {
            return new(normalize(value.x, low.x, high.x), normalize(value.y, low.y, high.y), normalize(
                value.z,
                low.z,
                high.z
            ), normalize(value.w, low.w, high.w));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int average(this int2 value)
        {
            return (value.x + value.y) / 2;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int average(this int3 value)
        {
            return (value.x + value.y + value.z) / 3;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int average(this int4 value)
        {
            return (value.x + value.y + value.z + value.w) / 4;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int normalize(this int value, int2 range)
        {
            return (value - range.x) / (range.y - range.x);
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int normalize(this int value, int low, int high)
        {
            return (value - low) / (high - low);
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int2 normalize(this int2 value, int2 low, int2 high)
        {
            return new(normalize(value.x, low.x, high.x), normalize(value.y, low.y, high.y));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int3 normalize(this int3 value, int3 low, int3 high)
        {
            return new(normalize(value.x, low.x, high.x), normalize(value.y, low.y, high.y), normalize(
                value.z,
                low.z,
                high.z
            ));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int4 normalize(this int4 value, int4 low, int4 high)
        {
            return new(normalize(value.x, low.x, high.x), normalize(value.y, low.y, high.y), normalize(
                value.z,
                low.z,
                high.z
            ), normalize(value.w, low.w, high.w));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 normalize(this Vector2 value, Vector2 low, Vector2 high)
        {
            return new(normalize(value.x, low.x, high.x), normalize(value.y, low.y, high.y));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 normalize(this Vector3 value, Vector3 low, Vector3 high)
        {
            return new(normalize(value.x, low.x, high.x), normalize(value.y, low.y, high.y), normalize(
                value.z,
                low.z,
                high.z
            ));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 normalize(this Vector3 value, Vector2 rangeX, Vector2 rangeY, Vector2 rangeZ)
        {
            return new(normalize(value.x, rangeX.x, rangeX.y), normalize(value.y, rangeY.x, rangeY.y),
                normalize(value.z,        rangeZ.x, rangeZ.y));
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 normalize(this Vector4 value, Vector4 low, Vector4 high)
        {
            return new(normalize(value.x, low.x, high.x), normalize(value.y, low.y, high.y), normalize(
                value.z,
                low.z,
                high.z
            ), normalize(value.w, low.w, high.w));
        }
    }
}
