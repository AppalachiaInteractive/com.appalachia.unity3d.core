#region

using System;
using System.Runtime.CompilerServices;
using Appalachia.Utility.src.Constants;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Extensions
{
    public static class float3Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 xyz1(this float3 val)
        {
            return new(val.x, val.y, val.z, 1f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion fromToRotation(this float3 f, float3 t, bool normalize)
        {
            if (normalize)
            {
                f = math.normalize(f);
                t = math.normalize(t);
            }

            var dotFT = math.dot(f, t);

            if (Math.Abs(dotFT - 1f) < float.Epsilon)
            {
                return quaternion.identity;
            }

            if (Math.Abs(dotFT - (-1f)) < float.Epsilon)
            {
                return quaternion.EulerXYZ(180f * float3c.up_forward);
            }

            var value = float4.zero;
            value.xyz = math.cross(f, t);
            value.w = math.sqrt(math.dot(f, f) * math.dot(t, t)) + dotFT;

            return math.normalize(new quaternion(value));
        }

        public static string ToStringF1(this float3 f)
        {
            return $"({f.x:F1}),({f.y:F1}),({f.z:F1})";
        }

        public static string ToStringF2(this float3 f)
        {
            return $"({f.x:F2}),({f.y:F2}),({f.z:F2})";
        }

        public static string ToStringF3(this float3 f)
        {
            return $"({f.x:F3}),({f.y:F3}),({f.z:F3})";
        }
    }
}
