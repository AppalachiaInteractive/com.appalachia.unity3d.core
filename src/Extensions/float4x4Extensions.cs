#region

using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

#endregion

namespace Appalachia.Core.Extensions
{
    public static class float4x4Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static bool anyInfinity(this float4x4 matrix)
        {
            var s = matrix[0][0] +
                    matrix[0][1] +
                    matrix[0][2] +
                    matrix[0][3] +
                    matrix[1][0] +
                    matrix[1][1] +
                    matrix[1][2] +
                    matrix[1][3] +
                    matrix[2][0] +
                    matrix[2][1] +
                    matrix[2][2] +
                    matrix[2][3] +
                    matrix[0][0] +
                    matrix[0][1] +
                    matrix[0][2] +
                    matrix[0][3];

            return float.IsInfinity(s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static bool anyNaN(this float4x4 matrix)
        {
            var s = matrix[0][0] +
                    matrix[0][1] +
                    matrix[0][2] +
                    matrix[0][3] +
                    matrix[1][0] +
                    matrix[1][1] +
                    matrix[1][2] +
                    matrix[1][3] +
                    matrix[2][0] +
                    matrix[2][1] +
                    matrix[2][2] +
                    matrix[2][3] +
                    matrix[0][0] +
                    matrix[0][1] +
                    matrix[0][2] +
                    matrix[0][3];

            return float.IsNaN(s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static bool anyNegativeInfinity(this float4x4 matrix)
        {
            var s = matrix[0][0] +
                    matrix[0][1] +
                    matrix[0][2] +
                    matrix[0][3] +
                    matrix[1][0] +
                    matrix[1][1] +
                    matrix[1][2] +
                    matrix[1][3] +
                    matrix[2][0] +
                    matrix[2][1] +
                    matrix[2][2] +
                    matrix[2][3] +
                    matrix[0][0] +
                    matrix[0][1] +
                    matrix[0][2] +
                    matrix[0][3];

            return float.IsNegativeInfinity(s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static bool anyPositiveInfinity(this float4x4 matrix)
        {
            var s = matrix[0][0] +
                    matrix[0][1] +
                    matrix[0][2] +
                    matrix[0][3] +
                    matrix[1][0] +
                    matrix[1][1] +
                    matrix[1][2] +
                    matrix[1][3] +
                    matrix[2][0] +
                    matrix[2][1] +
                    matrix[2][2] +
                    matrix[2][3] +
                    matrix[0][0] +
                    matrix[0][1] +
                    matrix[0][2] +
                    matrix[0][3];

            return float.IsPositiveInfinity(s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static Bounds TranslateAndScaleBounds(this float4x4 matrix, Bounds b)
        {
            var center = b.center;
            var size = b.size;

            var matrixScale = matrix.GetScaleFromMatrix();

            var newCenter = matrix.MultiplyPoint3x4(center);

            float3 newSize;

            newSize.x = matrixScale.x * size.x;
            newSize.y = matrixScale.y * size.y;
            newSize.z = matrixScale.z * size.z;

            b.center = newCenter;
            b.size = newSize;

            return b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static float3 GetPositionFromMatrix(this float4x4 matrix)
        {
            return matrix.c3.xyz;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static float3 GetScaleFromMatrix(this float4x4 matrix)
        {
            return new(math.length(matrix.c0), math.length(matrix.c1), math.length(matrix.c2));
        }

/*
 * 
    public Vector4 GetColumn(int index)
    {
      switch (index)
      {
        case 0:
          return new Vector4(this.m00, this.m10, this.m20, this.m30);
        case 1:
          return new Vector4(this.m01, this.m11, this.m21, this.m31);
        case 2:
          return new Vector4(this.m02, this.m12, this.m22, this.m32);
        case 3:
          return new Vector4(this.m03, this.m13, this.m23, this.m33);
        default:
          throw new IndexOutOfRangeException("Invalid column index!");
      }
    }
 */
        /*public static float m00(this float4x4 matrix) { return matrix.c0.x; }
        public static float m10(this float4x4 matrix) { return matrix.c0.y; }
        public static float m20(this float4x4 matrix) { return matrix.c0.z; }
        public static float m30(this float4x4 matrix) { return matrix.c0.w; }
        public static float m01(this float4x4 matrix) { return matrix.c1.x; }
        public static float m11(this float4x4 matrix) { return matrix.c1.y; }
        public static float m21(this float4x4 matrix) { return matrix.c1.z; }
        public static float m31(this float4x4 matrix) { return matrix.c1.w; }
        public static float m02(this float4x4 matrix) { return matrix.c2.x; }
        public static float m12(this float4x4 matrix) { return matrix.c2.y; }
        public static float m22(this float4x4 matrix) { return matrix.c2.z; }
        public static float m32(this float4x4 matrix) { return matrix.c2.w; }
        public static float m03(this float4x4 matrix) { return matrix.c3.x; }
        public static float m13(this float4x4 matrix) { return matrix.c3.y; }
        public static float m23(this float4x4 matrix) { return matrix.c3.z; }
        public static float m33(this float4x4 matrix) { return matrix.c3.w; }*/

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static float3 MultiplyPoint(this float4x4 matrix, float3 point)
        {
            var vector3 = MultiplyPoint3x4(matrix, point);

            var num = 1f /
                      ((float) ((matrix.c0.w * (double) point.x) +
                                (matrix.c1.w * (double) point.y) +
                                (matrix.c2.w * (double) point.z)) +
                       matrix.c3.w);

            vector3.x *= num;
            vector3.y *= num;
            vector3.z *= num;
            return vector3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static float3 MultiplyPoint3x4(this float4x4 matrix, float3 point)
        {
            var vector3 = MultiplyVector(matrix, point);

            vector3.x += matrix.c3.x;
            vector3.y += matrix.c3.y;
            vector3.z += matrix.c3.z;

            return vector3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static float3 MultiplyVector(this float4x4 matrix, float3 point)
        {
            float3 vector3;

            vector3.x = (float) ((matrix.c0.x * (double) point.x) +
                                 (matrix.c1.x * (double) point.y) +
                                 (matrix.c2.x * (double) point.z));
            vector3.y = (float) ((matrix.c0.y * (double) point.x) +
                                 (matrix.c1.y * (double) point.y) +
                                 (matrix.c2.y * (double) point.z));
            vector3.z = (float) ((matrix.c0.z * (double) point.x) +
                                 (matrix.c1.z * (double) point.y) +
                                 (matrix.c2.z * (double) point.z));

            return vector3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static float4x4 Inverse(this float4x4 matrix)
        {
            return math.inverse(matrix);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static float4x4 RotateOver(this quaternion following, float4x4 baseOperation)
        {
            return baseOperation.Transform(RotationMatrix(following));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static float4x4 RotateUnder(this quaternion baseOperation, float4x4 following)
        {
            return RotationMatrix(baseOperation).Transform(following);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static float4x4 RotationMatrix(this quaternion q)
        {
            var num1 = q.value.x * 2f;
            var num2 = q.value.y * 2f;
            var num3 = q.value.z * 2f;
            var num4 = q.value.x * num1;
            var num5 = q.value.y * num2;
            var num6 = q.value.z * num3;
            var num7 = q.value.x * num2;
            var num8 = q.value.x * num3;
            var num9 = q.value.y * num3;
            var num10 = q.value.w * num1;
            var num11 = q.value.w * num2;
            var num12 = q.value.w * num3;
            float4x4 matrix4x4;
            matrix4x4.c0.x = (float) (1.0 - (num5 + (double) num6));
            matrix4x4.c0.y = num7 + num12;
            matrix4x4.c0.z = num8 - num11;
            matrix4x4.c0.w = 0.0f;
            matrix4x4.c1.x = num7 - num12;
            matrix4x4.c1.y = (float) (1.0 - (num4 + (double) num6));
            matrix4x4.c1.z = num9 + num10;
            matrix4x4.c1.w = 0.0f;
            matrix4x4.c2.x = num8 + num11;
            matrix4x4.c2.y = num9 - num10;
            matrix4x4.c2.z = (float) (1.0 - (num4 + (double) num5));
            matrix4x4.c2.w = 0.0f;
            matrix4x4.c3.x = 0.0f;
            matrix4x4.c3.y = 0.0f;
            matrix4x4.c3.z = 0.0f;
            matrix4x4.c3.w = 1f;
            return matrix4x4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static float4x4 ScaleOver(this float3 following, float4x4 baseOperation)
        {
            return baseOperation.Transform(ScalingMatrix(following));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static float4x4 ScaleUnder(this float3 baseOperation, float4x4 following)
        {
            return ScalingMatrix(baseOperation).Transform(following);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static float4x4 ScalingMatrix(this float3 vector)
        {
            float4x4 matrix4x4;
            matrix4x4.c0.x = vector.x;
            matrix4x4.c0.y = 0.0f;
            matrix4x4.c0.z = 0.0f;
            matrix4x4.c0.w = 0.0f;
            matrix4x4.c1.x = 0.0f;
            matrix4x4.c1.y = vector.y;
            matrix4x4.c1.z = 0.0f;
            matrix4x4.c1.w = 0.0f;
            matrix4x4.c2.x = 0.0f;
            matrix4x4.c2.y = 0.0f;
            matrix4x4.c2.z = vector.z;
            matrix4x4.c2.w = 0.0f;
            matrix4x4.c3.x = 0.0f;
            matrix4x4.c3.y = 0.0f;
            matrix4x4.c3.z = 0.0f;
            matrix4x4.c3.w = 1f;
            return matrix4x4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static float4x4 SetPosition(this float4x4 matrix, float3 position)
        {
            matrix.c3 = position.xyz1();

            return matrix;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static float4x4 SetRotation(this float4x4 matrix, quaternion rotation)
        {
            matrix.c2 = rotation.forward().xyz1();
            matrix.c1 = rotation.up().xyz1();

            return matrix;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static float4x4 SetScale(this float4x4 matrix, float3 scale)
        {
            matrix = float4x4.TRS(matrix.GetPositionFromMatrix(), matrix.GetRotationFromMatrix(), scale);

            return matrix;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static float4x4 Transform(this float4x4 lhs, float4x4 rhs)
        {
            float4x4 matrix4x4;

            matrix4x4.c0.x = (float) ((lhs.c0.x * (double) rhs.c0.x) +
                                      (lhs.c1.x * (double) rhs.c0.y) +
                                      (lhs.c2.x * (double) rhs.c0.z) +
                                      (lhs.c3.x * (double) rhs.c0.w));
            matrix4x4.c1.x = (float) ((lhs.c0.x * (double) rhs.c1.x) +
                                      (lhs.c1.x * (double) rhs.c1.y) +
                                      (lhs.c2.x * (double) rhs.c1.z) +
                                      (lhs.c3.x * (double) rhs.c1.w));
            matrix4x4.c2.x = (float) ((lhs.c0.x * (double) rhs.c2.x) +
                                      (lhs.c1.x * (double) rhs.c2.y) +
                                      (lhs.c2.x * (double) rhs.c2.z) +
                                      (lhs.c3.x * (double) rhs.c2.w));
            matrix4x4.c3.x = (float) ((lhs.c0.x * (double) rhs.c3.x) +
                                      (lhs.c1.x * (double) rhs.c3.y) +
                                      (lhs.c2.x * (double) rhs.c3.z) +
                                      (lhs.c3.x * (double) rhs.c3.w));
            matrix4x4.c0.y = (float) ((lhs.c0.y * (double) rhs.c0.x) +
                                      (lhs.c1.y * (double) rhs.c0.y) +
                                      (lhs.c2.y * (double) rhs.c0.z) +
                                      (lhs.c3.y * (double) rhs.c0.w));
            matrix4x4.c1.y = (float) ((lhs.c0.y * (double) rhs.c1.x) +
                                      (lhs.c1.y * (double) rhs.c1.y) +
                                      (lhs.c2.y * (double) rhs.c1.z) +
                                      (lhs.c3.y * (double) rhs.c1.w));
            matrix4x4.c2.y = (float) ((lhs.c0.y * (double) rhs.c2.x) +
                                      (lhs.c1.y * (double) rhs.c2.y) +
                                      (lhs.c2.y * (double) rhs.c2.z) +
                                      (lhs.c3.y * (double) rhs.c2.w));
            matrix4x4.c3.y = (float) ((lhs.c0.y * (double) rhs.c3.x) +
                                      (lhs.c1.y * (double) rhs.c3.y) +
                                      (lhs.c2.y * (double) rhs.c3.z) +
                                      (lhs.c3.y * (double) rhs.c3.w));
            matrix4x4.c0.z = (float) ((lhs.c0.z * (double) rhs.c0.x) +
                                      (lhs.c1.z * (double) rhs.c0.y) +
                                      (lhs.c2.z * (double) rhs.c0.z) +
                                      (lhs.c3.z * (double) rhs.c0.w));
            matrix4x4.c1.z = (float) ((lhs.c0.z * (double) rhs.c1.x) +
                                      (lhs.c1.z * (double) rhs.c1.y) +
                                      (lhs.c2.z * (double) rhs.c1.z) +
                                      (lhs.c3.z * (double) rhs.c1.w));
            matrix4x4.c2.z = (float) ((lhs.c0.z * (double) rhs.c2.x) +
                                      (lhs.c1.z * (double) rhs.c2.y) +
                                      (lhs.c2.z * (double) rhs.c2.z) +
                                      (lhs.c3.z * (double) rhs.c2.w));
            matrix4x4.c3.z = (float) ((lhs.c0.z * (double) rhs.c3.x) +
                                      (lhs.c1.z * (double) rhs.c3.y) +
                                      (lhs.c2.z * (double) rhs.c3.z) +
                                      (lhs.c3.z * (double) rhs.c3.w));
            matrix4x4.c0.w = (float) ((lhs.c0.w * (double) rhs.c0.x) +
                                      (lhs.c1.w * (double) rhs.c0.y) +
                                      (lhs.c2.w * (double) rhs.c0.z) +
                                      (lhs.c3.w * (double) rhs.c0.w));
            matrix4x4.c1.w = (float) ((lhs.c0.w * (double) rhs.c1.x) +
                                      (lhs.c1.w * (double) rhs.c1.y) +
                                      (lhs.c2.w * (double) rhs.c1.z) +
                                      (lhs.c3.w * (double) rhs.c1.w));
            matrix4x4.c2.w = (float) ((lhs.c0.w * (double) rhs.c2.x) +
                                      (lhs.c1.w * (double) rhs.c2.y) +
                                      (lhs.c2.w * (double) rhs.c2.z) +
                                      (lhs.c3.w * (double) rhs.c2.w));
            matrix4x4.c3.w = (float) ((lhs.c0.w * (double) rhs.c3.x) +
                                      (lhs.c1.w * (double) rhs.c3.y) +
                                      (lhs.c2.w * (double) rhs.c3.z) +
                                      (lhs.c3.w * (double) rhs.c3.w));
            return matrix4x4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static float4x4 TranslateOver(this float3 following, float4x4 baseOperation, bool local)
        {
            if (local)
            {
                var worldToLocalMatrix = baseOperation.Inverse();

                var localOffset = worldToLocalMatrix.MultiplyVector(following);

                return baseOperation.Transform(TranslationMatrix(localOffset));
            }

            return baseOperation.Transform(TranslationMatrix(following));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static float4x4 TranslateUnder(this float3 baseOperation, float4x4 following)
        {
            return TranslationMatrix(baseOperation).Transform(following);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static float4x4 TranslationMatrix(this float3 vector)
        {
            float4x4 matrix4x4;
            matrix4x4.c0.x = 1f;
            matrix4x4.c0.y = 0.0f;
            matrix4x4.c0.z = 0.0f;
            matrix4x4.c0.w = vector.x;
            matrix4x4.c1.x = 0.0f;
            matrix4x4.c1.y = 1f;
            matrix4x4.c1.z = 0.0f;
            matrix4x4.c1.w = vector.y;
            matrix4x4.c2.x = 0.0f;
            matrix4x4.c2.y = 0.0f;
            matrix4x4.c2.z = 1f;
            matrix4x4.c2.w = vector.z;
            matrix4x4.c3.x = 0.0f;
            matrix4x4.c3.y = 0.0f;
            matrix4x4.c3.z = 0.0f;
            matrix4x4.c3.w = 1f;
            return matrix4x4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static quaternion GetRotationFromMatrix(this float4x4 matrix)
        {
            return quaternion.LookRotation(matrix.c2.xyz, matrix.c1.xyz);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static quaternion MultiplyRotation(this float4x4 matrix, quaternion rotation)
        {
            var axis = rotation.value.xyz;

            var rotated = matrix.MultiplyVector(axis);

            rotation.value.xyz = rotated;
            return rotation;
        }

        public static string ToStringTRS(this float4x4 matrix)
        {
            var pos = matrix.GetPositionFromMatrix();
            var rotation = matrix.GetRotationFromMatrix();
            var scale = matrix.GetScaleFromMatrix();

            return $"T [{pos.ToStringF1()}] | " +
                   $"R [{rotation.ToEuler().ToStringF1()}] | " +
                   $"S [{scale.ToStringF1()}]";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static void float4x4ToTransform(this Transform transform, float4x4 matrix)
        {
            transform.position = matrix.c3.xyz;

            transform.localScale = new float3(
                math.length(matrix.c0),
                math.length(matrix.c1),
                math.length(matrix.c2)
            );
            transform.rotation = quaternion.LookRotation(matrix.c2.xyz, matrix.c1.xyz);
        }
    }
}
