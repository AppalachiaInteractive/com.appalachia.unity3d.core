#region

using UnityEngine;

#endregion

namespace Appalachia.Core.Extensions.Helpers
{
    public static class GizmosHelper
    {
        public static void SetColor(Color color)
        {
#if UNITY_EDITOR
            Gizmos.color = color;
#endif
        }

        public static void SetMatrix(Matrix4x4 matrix)
        {
#if UNITY_EDITOR
            Gizmos.matrix = matrix;
#endif
        }

        public static void DrawWireSphere(Vector3 position, float radius, Color color)
        {
#if UNITY_EDITOR
            var temp = Gizmos.color;
            Gizmos.color = color;
            Gizmos.DrawWireSphere(position, radius);
            Gizmos.color = temp;
#endif
        }

        public static void DrawSphere(Vector3 position, float radius, Color color)
        {
#if UNITY_EDITOR
            var temp = Gizmos.color;
            Gizmos.color = color;
            Gizmos.DrawSphere(position, radius);
            Gizmos.color = temp;
#endif
        }

        public static void DrawWireCube(Vector3 position, Vector3 size, Color color)
        {
#if UNITY_EDITOR
            var temp = Gizmos.color;
            Gizmos.color = color;
            Gizmos.DrawWireCube(position, size);
            Gizmos.color = temp;
#endif
        }

        public static void DrawCube(Vector3 position, Vector3 size, Color color)
        {
#if UNITY_EDITOR
            var temp = Gizmos.color;
            Gizmos.color = color;
            Gizmos.DrawCube(position, size);
            Gizmos.color = temp;
#endif
        }

        public static void DrawWireSphere(Vector3 position, float radius)
        {
#if UNITY_EDITOR
            Gizmos.DrawWireSphere(position, radius);
#endif
        }

        public static void DrawSphere(Vector3 position, float radius)
        {
#if UNITY_EDITOR
            Gizmos.DrawSphere(position, radius);
#endif
        }

        public static void DrawWireCube(Vector3 position, Vector3 size)
        {
#if UNITY_EDITOR
            Gizmos.DrawWireCube(position, size);
#endif
        }

        public static void DrawCube(Vector3 position, Vector3 size)
        {
#if UNITY_EDITOR
            Gizmos.DrawCube(position, size);
#endif
        }

        public static void DrawRay(Ray ray)
        {
#if UNITY_EDITOR
            Gizmos.DrawRay(ray);
#endif
        }

        public static void DrawRay(Vector3 from, Vector3 direction)
        {
#if UNITY_EDITOR
            Gizmos.DrawRay(from, direction);
#endif
        }

        public static void DrawRay(Ray ray, Color color)
        {
#if UNITY_EDITOR
            var temp = Gizmos.color;
            Gizmos.color = color;

            Gizmos.DrawRay(ray);
            Gizmos.color = temp;
#endif
        }

        public static void DrawRay(Vector3 from, Vector3 direction, Color color)
        {
#if UNITY_EDITOR
            var temp = Gizmos.color;
            Gizmos.color = color;

            Gizmos.DrawRay(new Ray(from, direction));
            Gizmos.color = temp;
#endif
        }

        public static void DrawLine(Vector3 from, Vector3 to)
        {
#if UNITY_EDITOR
            Gizmos.DrawLine(from, to);
#endif
        }

        public static void DrawLine(Vector3 from, Vector3 to, Color color)
        {
#if UNITY_EDITOR
            var temp = Gizmos.color;
            Gizmos.color = color;

            Gizmos.DrawLine(from, to);
            Gizmos.color = temp;
#endif
        }
    }
}
