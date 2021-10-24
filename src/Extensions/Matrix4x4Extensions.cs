#region

using Unity.Mathematics;
using UnityEngine;

#endregion

namespace Appalachia.Core.Extensions
{
    public static class Matrix4x4Extensions
    {
        public static Bounds MatrixToBounds(this Matrix4x4 matrix)
        {
            var bounds = new Bounds
            {
                center = matrix.GetPositionFromMatrix(), size = matrix.GetScaleFromMatrix()
            };

            return bounds;
        }

        public static Bounds TranslateAndScaleBounds(this Matrix4x4 matrix, Bounds b)
        {
            var bounds = matrix.MatrixToBounds();

            var size = bounds.size;

            size.x *= b.size.x;
            size.y *= b.size.y;
            size.z *= b.size.z;

            bounds.size = size;

            return bounds;
        }

        public static float[] Matrix4x4ToFloatArray(this Matrix4x4 matrix4x4)
        {
            var floatArray = new float[16];

            Matrix4x4ToFloatArray(matrix4x4, floatArray);

            return floatArray;
        }

        public static Matrix4x4 Matrix4x4FromString(string matrixStr)
        {
            var matrix4x4 = new Matrix4x4();
            var floatStrArray = matrixStr.Split(';');
            for (var i = 0; i < floatStrArray.Length; i++)
            {
                matrix4x4[i / 4, i % 4] = float.Parse(floatStrArray[i]);
            }

            return matrix4x4;
        }

        public static Matrix4x4 SetPosition(this Matrix4x4 matrix, Vector3 position)
        {
            matrix.SetColumn(3, position);

            return matrix;
        }

        public static Matrix4x4 SetRotation(this Matrix4x4 matrix, Quaternion rotation)
        {
            matrix.SetColumn(2, rotation.Forward());
            matrix.SetColumn(1, rotation.Up());

            return matrix;
        }

        public static Matrix4x4 SetScale(this Matrix4x4 matrix, Vector3 scale)
        {
            matrix.SetTRS(matrix.GetPositionFromMatrix(), matrix.GetRotationFromMatrix(), scale);

            return matrix;
        }

        public static Quaternion GetRotationFromMatrix(this Matrix4x4 matrix)
        {
            var c2 = matrix.GetColumn(2);
            var c1 = matrix.GetColumn(1);

            var len2 = math.length(c2);
            var len1 = math.length(c1);

            if ((c1 == c2) ||
                (c1 == Vector4.zero) ||
                (c2 == Vector4.zero) ||
                (len1 == 0f) ||
                (len2 == 0f) ||
                (c1 == -c2))
            {
                Debug.LogWarning("Bad viewing vector");
                return Quaternion.identity;
            }

            return Quaternion.LookRotation(c2, c1);
        }

        public static string Matrix4x4ToString(Matrix4x4 matrix4x4)
        {
            var matrix4X4String = matrix4x4.ToString().Replace("\n", ";").Replace("\t", ";");
            matrix4X4String = matrix4X4String.Substring(0, matrix4X4String.Length - 1);
            return matrix4X4String;
        }

        public static Vector3 GetPositionFromMatrix(this Matrix4x4 matrix)
        {
            return matrix.GetColumn(3);
        }

        public static Vector3 GetScaleFromMatrix(this Matrix4x4 matrix)
        {
            return new(matrix.GetColumn(0).magnitude, matrix.GetColumn(1).magnitude, matrix.GetColumn(2)
               .magnitude);
        }

        public static void Matrix4x4ToFloatArray(this Matrix4x4 matrix4x4, float[] floatArray)
        {
            floatArray[0] = matrix4x4[0, 0];
            floatArray[1] = matrix4x4[1, 0];
            floatArray[2] = matrix4x4[2, 0];
            floatArray[3] = matrix4x4[3, 0];
            floatArray[4] = matrix4x4[0, 1];
            floatArray[5] = matrix4x4[1, 1];
            floatArray[6] = matrix4x4[2, 1];
            floatArray[7] = matrix4x4[3, 1];
            floatArray[8] = matrix4x4[0, 2];
            floatArray[9] = matrix4x4[1, 2];
            floatArray[10] = matrix4x4[2, 2];
            floatArray[11] = matrix4x4[3, 2];
            floatArray[12] = matrix4x4[0, 3];
            floatArray[13] = matrix4x4[1, 3];
            floatArray[14] = matrix4x4[2, 3];
            floatArray[15] = matrix4x4[3, 3];
        }

        public static void Matrix4x4ToTransform(this Transform transform, Matrix4x4 matrix)
        {
            transform.position = matrix.GetColumn(3);
            transform.localScale = new Vector3(
                matrix.GetColumn(0).magnitude,
                matrix.GetColumn(1).magnitude,
                matrix.GetColumn(2).magnitude
            );
            transform.rotation = Quaternion.LookRotation(matrix.GetColumn(2), matrix.GetColumn(1));
        }
    }
}
