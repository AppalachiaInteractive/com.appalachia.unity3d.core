#region

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

#endregion

namespace Appalachia.Core.Extensions
{
    public static class Vector3Extensions
    {
        public static bool GreaterThan(this Vector3 vector3, Vector3 other)
        {
            return (vector3.x > other.x) && (vector3.y > other.y) && (vector3.z > other.z);
        }

        public static bool GreaterThan(this Vector2 vector3, Vector2 other)
        {
            return (vector3.x > other.x) && (vector3.y > other.y);
        }

        public static bool IsUniform(this Vector2 vector)
        {
            return Math.Abs(vector.x - vector.y) < float.Epsilon;
        }

        public static bool IsUniform(this Vector3 vector)
        {
            return (Math.Abs(vector.x - vector.y) < float.Epsilon) &&
                   (Math.Abs(vector.x - vector.z) < float.Epsilon);
        }

        public static bool IsUniform(this Vector4 vector)
        {
            return (Math.Abs(vector.x - vector.y) < float.Epsilon) &&
                   (Math.Abs(vector.x - vector.z) < float.Epsilon) &&
                   (Math.Abs(vector.x - vector.w) < float.Epsilon);
        }

        public static bool LessThan(this Vector3 vector3, Vector3 other)
        {
            return (vector3.x < other.x) && (vector3.y < other.y) && (vector3.z < other.z);
        }

        public static bool LessThan(this Vector2 vector3, Vector2 other)
        {
            return (vector3.x < other.x) && (vector3.y < other.y);
        }

        public static bool NotRoughlyOne(this Vector3 input)
        {
            return !Roughly(input, 1);
        }

        public static bool NotRoughlyZero(this Vector3 input)
        {
            return !Roughly(input, 0);
        }

        public static bool Roughly(this Vector3 input, float scalar)
        {
            return input.x.RoughlyEqual(scalar) &&
                   input.y.RoughlyEqual(scalar) &&
                   input.z.RoughlyEqual(scalar);
        }

        public static bool RoughlyEqual(this Vector3 input, Vector3 other)
        {
            return input.x.RoughlyEqual(other.x) &&
                   input.y.RoughlyEqual(other.y) &&
                   input.z.RoughlyEqual(other.z);
        }

        public static bool RoughlyOne(this Vector3 input)
        {
            return Roughly(input, 1);
        }

        public static bool RoughlyZero(this Vector3 input)
        {
            return Roughly(input, 0);
        }

        public static Vector2 ClampEulerTo180Range(this Vector3 input)
        {
            while (input.x > 180)
            {
                input.x -= 360;
            }

            while (input.y > 180)
            {
                input.y -= 360;
            }

            while (input.z > 180)
            {
                input.z -= 360;
            }

            while (input.x <= -180)
            {
                input.x += 360;
            }

            while (input.y <= -180)
            {
                input.y += 360;
            }

            while (input.z <= -180)
            {
                input.z += 360;
            }

            return input;
        }

        public static Vector2 ClampEulerTo360Range(this Vector3 input)
        {
            while (input.x >= 360)
            {
                input.x -= 360;
            }

            while (input.y >= 360)
            {
                input.y -= 360;
            }

            while (input.z >= 360)
            {
                input.z -= 360;
            }

            while (input.x < 0)
            {
                input.x += 360;
            }

            while (input.y < 0)
            {
                input.y += 360;
            }

            while (input.z < 0)
            {
                input.z += 360;
            }

            return input;
        }

        public static Vector2 EulerAngleToViewRotation(this Vector3 input)
        {
            var lookRotation = new Vector2(input.y, input.x);

            if (lookRotation.x > 180)
            {
                lookRotation.x = -360 + lookRotation.x;
            }

            if (lookRotation.y > 180)
            {
                lookRotation.y = -360 + lookRotation.y;
            }

            lookRotation.y = -lookRotation.y;

            return lookRotation;
        }

        public static Vector2 Round(this Vector2 vector3, int decimalPlaces = 2)
        {
            float multiplier = 1;
            for (var i = 0; i < decimalPlaces; i++)
            {
                multiplier *= 10f;
            }

            return new Vector2(
                Mathf.Round(vector3.x * multiplier) / multiplier,
                Mathf.Round(vector3.y * multiplier) / multiplier
            );
        }

        public static Vector2 xx(this Vector3 vector3)
        {
            return new(vector3.x, vector3.x);
        }

        public static Vector2 xy(this Vector3 vector3)
        {
            return new(vector3.x, vector3.y);
        }

        public static Vector2 xz(this Vector3 vector3)
        {
            return new(vector3.x, vector3.z);
        }

        public static Vector2 yx(this Vector3 vector3)
        {
            return new(vector3.y, vector3.x);
        }

        public static Vector2 yy(this Vector3 vector3)
        {
            return new(vector3.y, vector3.y);
        }

        public static Vector2 yz(this Vector3 vector3)
        {
            return new(vector3.y, vector3.z);
        }

        public static Vector2 zx(this Vector3 vector3)
        {
            return new(vector3.z, vector3.x);
        }

        public static Vector2 zy(this Vector3 vector3)
        {
            return new(vector3.z, vector3.y);
        }

        public static Vector2 zz(this Vector3 vector3)
        {
            return new(vector3.z, vector3.z);
        }

        public static Vector2[] Normalize(
            this IEnumerable<Vector2> args,
            Vector2 min,
            Vector2 max,
            bool clamped)
        {
            return args.Select(
                            v =>
                            {
                                var x = (v.x - min.x) / (max.x - min.x);
                                var y = (v.y - min.y) / (max.y - min.y);
                                return clamped ? new Vector2(x, y).normalized : new Vector2(x, y);
                            }
                        )
                       .ToArray();
        }

        public static Vector2[] Normalize(this IEnumerable<Vector2> args, bool clamped)
        {
            var min = Vector2.positiveInfinity;
            var max = Vector2.negativeInfinity;

            foreach (var arg in args)
            {
                min.x = arg.x < min.x ? arg.x : min.x;
                min.y = arg.y < min.y ? arg.y : min.y;

                max.x = arg.x > max.x ? arg.x : max.x;
                max.y = arg.y > max.y ? arg.y : max.y;
            }

            return Normalize(args, min, max, clamped);
        }

        public static Vector2[] NormalizeFrom0(this IEnumerable<Vector2> args, bool clamped)
        {
            var max = Vector2.negativeInfinity;

            foreach (var arg in args)
            {
                max.x = arg.x > max.x ? arg.x : max.x;
                max.y = arg.y > max.y ? arg.y : max.y;
            }

            return Normalize(args, Vector2.zero, max, clamped);
        }

        public static Vector3 Anticipate(
            this Vector3 first,
            float elapsed,
            Vector3 second,
            float anticipationDuration)
        {
            return new(second.x + ((anticipationDuration / elapsed) * (second.x - first.x)), second.y +
                ((anticipationDuration / elapsed) * (second.y - first.y)), second.z +
                ((anticipationDuration / elapsed) * (second.z - first.z)));
        }

        public static Vector3 Derivative(this Vector3 older, Vector3 newer, float elapsed)
        {
            return (newer - older) / elapsed;
        }

        public static Vector3 GetReciprocal(this Vector3 input)
        {
            return new(1f / input.x, 1f / input.y, 1f / input.z);
        }

        public static Vector3 RandomPointWithinBoundingBox(this Vector3 size)
        {
            return new(Random.Range(-.5f * size.x, .5f * size.x), Random.Range(-.5f * size.y, .5f * size.y),
                Random.Range(-.5f * size.z,        .5f * size.z));
        }

        public static Vector3 RandomPointWithinBoundingBox(this Vector3 size, Vector3 center)
        {
            return center +
                   new Vector3(
                       Random.Range(-.5f * size.x, .5f * size.x),
                       Random.Range(-.5f * size.y, .5f * size.y),
                       Random.Range(-.5f * size.z, .5f * size.z)
                   );
        }

        public static Vector3 RatioCombine(this Vector3 source, Vector3 other1, float ratio1)
        {
            if ((ratio1 < 0) || (ratio1 > 1))
            {
                throw new NotSupportedException($"Invalid ratio: {ratio1}");
            }

            return (source * (1 - ratio1)) + (other1 * ratio1);
        }

        public static Vector3 RatioCombine(
            this Vector3 source,
            Vector3 other1,
            float ratio1,
            Vector3 other2,
            float ratio2)
        {
            if ((ratio1 < 0) || (ratio1 > 1))
            {
                throw new NotSupportedException($"Invalid ratio 1: {ratio1}");
            }

            if ((ratio2 < 0) || (ratio2 > 1))
            {
                throw new NotSupportedException($"Invalid ratio 2: {ratio2}");
            }

            var ratioSum = ratio1 + ratio2;

            if ((ratioSum < 0) || (ratioSum > 1))
            {
                throw new NotSupportedException($"Invalid ratio sum: {ratioSum}");
            }

            return (source * (1 - ratioSum)) + (other1 * ratio1) + (other2 * ratio2);
        }

        public static Vector3 RatioCombine(
            this Vector3 source,
            Vector3 other1,
            float ratio1,
            Vector3 other2,
            float ratio2,
            Vector3 other3,
            float ratio3)
        {
            if ((ratio1 < 0) || (ratio1 > 1))
            {
                throw new NotSupportedException($"Invalid ratio: {ratio1}");
            }

            if ((ratio2 < 0) || (ratio2 > 1))
            {
                throw new NotSupportedException($"Invalid ratio 2: {ratio2}");
            }

            if ((ratio3 < 0) || (ratio3 > 1))
            {
                throw new NotSupportedException($"Invalid ratio 3: {ratio3}");
            }

            var ratioSum = ratio1 + ratio2 + ratio3;

            if ((ratioSum < 0) || (ratioSum > 1))
            {
                throw new NotSupportedException($"Invalid ratio sum: {ratioSum}");
            }

            return (source * (1 - ratioSum)) + (other1 * ratio1) + (other2 * ratio2) + (other3 * ratio3);
        }

        public static Vector3 Round(this Vector3 vector3, int decimalPlaces = 2)
        {
            float multiplier = 1;
            for (var i = 0; i < decimalPlaces; i++)
            {
                multiplier *= 10f;
            }

            return new Vector3(
                Mathf.Round(vector3.x * multiplier) / multiplier,
                Mathf.Round(vector3.y * multiplier) / multiplier,
                Mathf.Round(vector3.z * multiplier) / multiplier
            );
        }

        public static Vector3 ScaleBy(this Vector3 vector, Vector3 scaleBy)
        {
            return new(vector.x * scaleBy.x, vector.y * scaleBy.y, vector.z * scaleBy.z);
        }

        public static Vector3 ViewRotationToEulerAngles(this Vector2 input)
        {
            input.y = -input.y;

            return new Vector3(input.y, input.x, 0);
        }

        public static Vector3 xxx(this Vector3 vector3)
        {
            return new(vector3.x, vector3.x, vector3.x);
        }

        public static Vector3 xxy(this Vector3 vector3)
        {
            return new(vector3.x, vector3.x, vector3.y);
        }

        public static Vector3 xxz(this Vector3 vector3)
        {
            return new(vector3.x, vector3.x, vector3.z);
        }

        public static Vector3 xyx(this Vector3 vector3)
        {
            return new(vector3.x, vector3.y, vector3.x);
        }

        public static Vector3 xyy(this Vector3 vector3)
        {
            return new(vector3.x, vector3.y, vector3.y);
        }

        public static Vector3 xyz(this Vector3 vector3)
        {
            return new(vector3.x, vector3.y, vector3.z);
        }

        public static Vector3 xzx(this Vector3 vector3)
        {
            return new(vector3.x, vector3.z, vector3.x);
        }

        public static Vector3 xzy(this Vector3 vector3)
        {
            return new(vector3.x, vector3.z, vector3.y);
        }

        public static Vector3 xzz(this Vector3 vector3)
        {
            return new(vector3.x, vector3.z, vector3.z);
        }

        public static Vector3 yxx(this Vector3 vector3)
        {
            return new(vector3.y, vector3.x, vector3.x);
        }

        public static Vector3 yxy(this Vector3 vector3)
        {
            return new(vector3.y, vector3.x, vector3.y);
        }

        public static Vector3 yxz(this Vector3 vector3)
        {
            return new(vector3.y, vector3.x, vector3.z);
        }

        public static Vector3 yyx(this Vector3 vector3)
        {
            return new(vector3.y, vector3.y, vector3.x);
        }

        public static Vector3 yyy(this Vector3 vector3)
        {
            return new(vector3.y, vector3.y, vector3.y);
        }

        public static Vector3 yyz(this Vector3 vector3)
        {
            return new(vector3.y, vector3.y, vector3.z);
        }

        public static Vector3 yzx(this Vector3 vector3)
        {
            return new(vector3.y, vector3.z, vector3.x);
        }

        public static Vector3 yzy(this Vector3 vector3)
        {
            return new(vector3.y, vector3.z, vector3.y);
        }

        public static Vector3 yzz(this Vector3 vector3)
        {
            return new(vector3.y, vector3.z, vector3.z);
        }

        public static Vector3 zxx(this Vector3 vector3)
        {
            return new(vector3.z, vector3.x, vector3.x);
        }

        public static Vector3 zxy(this Vector3 vector3)
        {
            return new(vector3.z, vector3.x, vector3.y);
        }

        public static Vector3 zxz(this Vector3 vector3)
        {
            return new(vector3.z, vector3.x, vector3.z);
        }

        public static Vector3 zyx(this Vector3 vector3)
        {
            return new(vector3.z, vector3.y, vector3.x);
        }

        public static Vector3 zyy(this Vector3 vector3)
        {
            return new(vector3.z, vector3.y, vector3.y);
        }

        public static Vector3 zyz(this Vector3 vector3)
        {
            return new(vector3.z, vector3.y, vector3.z);
        }

        public static Vector3 zzx(this Vector3 vector3)
        {
            return new(vector3.z, vector3.z, vector3.x);
        }

        public static Vector3 zzy(this Vector3 vector3)
        {
            return new(vector3.z, vector3.z, vector3.y);
        }

        public static Vector3 zzz(this Vector3 vector3)
        {
            return new(vector3.z, vector3.z, vector3.z);
        }

        public static Vector3[] Normalize(
            this IEnumerable<Vector3> args,
            Vector3 min,
            Vector3 max,
            bool clamped)
        {
            return args.Select(
                            v =>
                            {
                                var x = (v.x - min.x) / (max.x - min.x);
                                var y = (v.y - min.y) / (max.y - min.y);
                                var z = (v.z - min.z) / (max.z - min.z);
                                return clamped ? new Vector3(x, y, z).normalized : new Vector3(x, y, z);
                            }
                        )
                       .ToArray();
        }

        public static Vector3[] Normalize(this IEnumerable<Vector3> args, bool clamped)
        {
            var min = Vector3.positiveInfinity;
            var max = Vector3.negativeInfinity;

            foreach (var arg in args)
            {
                min.x = arg.x < min.x ? arg.x : min.x;
                min.y = arg.y < min.y ? arg.y : min.y;
                min.z = arg.z < min.z ? arg.z : min.z;

                max.x = arg.x > max.x ? arg.x : max.x;
                max.y = arg.y > max.y ? arg.y : max.y;
                max.z = arg.z > max.z ? arg.z : max.z;
            }

            return Normalize(args, min, max, clamped);
        }

        public static Vector3[] NormalizeFrom0(this IEnumerable<Vector3> args, bool clamped)
        {
            var max = Vector3.negativeInfinity;

            foreach (var arg in args)
            {
                max.x = arg.x > max.x ? arg.x : max.x;
                max.y = arg.y > max.y ? arg.y : max.y;
                max.z = arg.z > max.z ? arg.z : max.z;
            }

            return Normalize(args, Vector3.zero, max, clamped);
        }

        public static Vector4 Round(this Vector4 vector3, int decimalPlaces = 2)
        {
            float multiplier = 1;
            for (var i = 0; i < decimalPlaces; i++)
            {
                multiplier *= 10f;
            }

            return new Vector4(
                Mathf.Round(vector3.x * multiplier) / multiplier,
                Mathf.Round(vector3.y * multiplier) / multiplier,
                Mathf.Round(vector3.z * multiplier) / multiplier,
                Mathf.Round(vector3.w * multiplier) / multiplier
            );
        }

        public static Vector4[] Normalize(
            this IEnumerable<Vector4> args,
            Vector4 min,
            Vector4 max,
            bool clamped)
        {
            return args.Select(
                            v =>
                            {
                                var x = (v.x - min.x) / (max.x - min.x);
                                var y = (v.y - min.y) / (max.y - min.y);
                                var z = (v.z - min.z) / (max.z - min.z);
                                var w = (v.w - min.w) / (max.w - min.w);
                                return clamped ? new Vector4(x, y, z, w).normalized : new Vector4(x, y, z, w);
                            }
                        )
                       .ToArray();
        }

        public static Vector4[] Normalize(this IEnumerable<Vector4> args, bool clamped)
        {
            var min = Vector4.positiveInfinity;
            var max = Vector4.negativeInfinity;

            foreach (var arg in args)
            {
                min.x = arg.x < min.x ? arg.x : min.x;
                min.y = arg.y < min.y ? arg.y : min.y;
                min.z = arg.z < min.z ? arg.z : min.z;
                min.w = arg.w < min.w ? arg.w : min.w;

                max.x = arg.x > max.x ? arg.x : max.x;
                max.y = arg.y > max.y ? arg.y : max.y;
                max.z = arg.z > max.z ? arg.z : max.z;
                max.w = arg.w > max.w ? arg.w : max.w;
            }

            return Normalize(args, min, max, clamped);
        }

        public static Vector4[] NormalizeFrom0(this IEnumerable<Vector4> args, bool clamped)
        {
            var max = Vector4.negativeInfinity;

            foreach (var arg in args)
            {
                max.x = arg.x > max.x ? arg.x : max.x;
                max.y = arg.y > max.y ? arg.y : max.y;
                max.z = arg.z > max.z ? arg.z : max.z;
                max.w = arg.w > max.w ? arg.w : max.w;
            }

            return Normalize(args, Vector4.zero, max, clamped);
        }
    }
}
