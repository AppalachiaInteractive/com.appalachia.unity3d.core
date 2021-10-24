#region

using UnityEngine;

#endregion

namespace Appalachia.Core.Extensions
{
    public static class QuaternionsExtensions
    {
        public static bool Approximately(this Quaternion input, Quaternion other)
        {
            return input == other;
        }

        public static Quaternion Anticipate(
            this Quaternion first,
            float elapsed,
            Quaternion second,
            float anticipationDuration)
        {
            return new(second.x + ((anticipationDuration / elapsed) * (second.x - first.x)), second.y +
                ((anticipationDuration / elapsed) * (second.y - first.y)), second.z +
                ((anticipationDuration / elapsed) * (second.z - first.z)), second.w +
                ((anticipationDuration / elapsed) * (second.w - first.w)));
        }

        public static Vector3 Back(this Quaternion quaternion)
        {
            return quaternion * Vector3.back;
        }

        public static Vector3 Down(this Quaternion quaternion)
        {
            return quaternion * Vector3.down;
        }

        public static Vector3 Forward(this Quaternion quaternion)
        {
            return quaternion * Vector3.forward;
        }

        public static Vector3 GetAngularVelocity(this Quaternion older, Quaternion newer, float elapsed)
        {
            var q = newer * Quaternion.Inverse(older);

            if (Mathf.Abs(q.w) > 0.999f)
            {
                return new Vector3(0, 0, 0);
            }

            float gain;

            if (q.w < 0.0f)
            {
                var angle = Mathf.Acos(-q.w);
                gain = (-2.0f * angle) / (Mathf.Sin(angle) * elapsed);
            }
            else
            {
                var angle = Mathf.Acos(q.w);
                gain = (2.0f * angle) / (Mathf.Sin(angle) * elapsed);
            }

            return new Vector3(q.x * gain, q.y * gain, q.z * gain);
        }

        public static Vector3 Left(this Quaternion quaternion)
        {
            return quaternion * Vector3.left;
        }

        public static Vector3 Right(this Quaternion quaternion)
        {
            return quaternion * Vector3.right;
        }

        public static Vector3 Up(this Quaternion quaternion)
        {
            return quaternion * Vector3.up;
        }
    }
}
