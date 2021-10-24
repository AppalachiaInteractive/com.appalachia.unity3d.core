#region

using System;
using Unity.Mathematics;
using UnityEngine;

#endregion

namespace Appalachia.Core.Extensions
{
    public static class TransformExtensions
    {
        private static readonly Matrix4x4 _matrix_trs_zero = Matrix4x4.TRS(
            Vector3.zero,
            Quaternion.identity,
            Vector3.one
        );

        private static readonly Matrix4x4 _matrix_zero = Matrix4x4.zero;

        public static float4x4 localToParentMatrix(this Transform t)
        {
            if (t.parent == null)
            {
                return float4x4.identity;
            }

            return float4x4.TRS(t.localPosition, t.localRotation, t.localScale);
        }

        public static void SetMatrix4x4ToTransform(this Transform t, Matrix4x4 matrix)
        {
            if (matrix == _matrix_zero)
            {
                throw new NotSupportedException($"Default matrix for {t.gameObject.name}.");
            }

            t.position = matrix.GetColumn(3);
            t.localScale = new Vector3(
                matrix.GetColumn(0).magnitude,
                matrix.GetColumn(1).magnitude,
                matrix.GetColumn(2).magnitude
            );
            t.rotation = Quaternion.LookRotation(matrix.GetColumn(2), matrix.GetColumn(1));
        }
    }
}
