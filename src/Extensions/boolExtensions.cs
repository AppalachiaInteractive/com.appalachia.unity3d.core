#region

using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Extensions
{
    public static class boolExtensions
    {
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool all(this bool2 v)
        {
            return v.x && v.y;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool all(this bool3 v)
        {
            return v.x && v.y && v.z;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool all(this bool4 v)
        {
            return v.x && v.y && v.z && v.w;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool all(this bool2x2 v)
        {
            return v.c0.all() && v.c1.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool all(this bool2x3 v)
        {
            return v.c0.all() && v.c1.all() && v.c2.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool all(this bool2x4 v)
        {
            return v.c0.all() && v.c1.all() && v.c2.all() && v.c3.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool all(this bool3x2 v)
        {
            return v.c0.all() && v.c1.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool all(this bool3x3 v)
        {
            return v.c0.all() && v.c1.all() && v.c2.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool all(this bool3x4 v)
        {
            return v.c0.all() && v.c1.all() && v.c2.all() && v.c3.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool all(this bool4x2 v)
        {
            return v.c0.all() && v.c1.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool all(this bool4x3 v)
        {
            return v.c0.all() && v.c1.all() && v.c2.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool all(this bool4x4 v)
        {
            return v.c0.all() && v.c1.all() && v.c2.all() && v.c3.all();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool allButOne(this bool3 v)
        {
            return (!v.x && v.y && v.z) || (v.x && !v.y && v.z) || (v.x && v.y && !v.z);
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool allButOne(this bool4 v)
        {
            return (!v.x && v.y && v.z && v.w) ||
                   (v.x && !v.y && v.z && v.w) ||
                   (v.x && v.y && !v.z && v.w) ||
                   (v.x && v.y && v.z && !v.w);
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool any(this bool2 v)
        {
            return v.x || v.y;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool any(this bool3 v)
        {
            return v.x || v.y || v.z;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool any(this bool4 v)
        {
            return v.x || v.y || v.z || v.w;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool any(this bool2x2 v)
        {
            return v.c0.any() || v.c1.any();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool any(this bool2x3 v)
        {
            return v.c0.any() || v.c1.any() || v.c2.any();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool any(this bool2x4 v)
        {
            return v.c0.any() || v.c1.any() || v.c2.any() || v.c3.any();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool any(this bool3x2 v)
        {
            return v.c0.any() || v.c1.any();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool any(this bool3x3 v)
        {
            return v.c0.any() || v.c1.any() || v.c2.any();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool any(this bool3x4 v)
        {
            return v.c0.any() || v.c1.any() || v.c2.any() || v.c3.any();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool any(this bool4x2 v)
        {
            return v.c0.any() || v.c1.any();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool any(this bool4x3 v)
        {
            return v.c0.any() || v.c1.any() || v.c2.any();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool any(this bool4x4 v)
        {
            return v.c0.any() || v.c1.any() || v.c2.any() || v.c3.any();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool half(this bool4 v)
        {
            return (v.x && v.y && !v.z && !v.w) ||
                   (v.x && !v.y && v.z && !v.w) ||
                   (v.x && !v.y && !v.z && v.w) ||
                   (!v.x && v.y && v.z && !v.w) ||
                   (!v.x && v.y && !v.z && v.w) ||
                   (!v.x && !v.y && v.z && v.w);
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool none(this bool2 v)
        {
            return !v.x && !v.y;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool none(this bool3 v)
        {
            return !v.x && !v.y && !v.z;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool none(this bool4 v)
        {
            return !v.x && !v.y && !v.z && !v.w;
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool none(this bool2x2 v)
        {
            return v.c0.none() && v.c1.none();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool none(this bool2x3 v)
        {
            return v.c0.none() && v.c1.none() && v.c2.none();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool none(this bool2x4 v)
        {
            return v.c0.none() && v.c1.none() && v.c2.none() && v.c3.none();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool none(this bool3x2 v)
        {
            return v.c0.none() && v.c1.none();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool none(this bool3x3 v)
        {
            return v.c0.none() && v.c1.none() && v.c2.none();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool none(this bool3x4 v)
        {
            return v.c0.none() && v.c1.none() && v.c2.none() && v.c3.none();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool none(this bool4x2 v)
        {
            return v.c0.none() && v.c1.none();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool none(this bool4x3 v)
        {
            return v.c0.none() && v.c1.none() && v.c2.none();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool none(this bool4x4 v)
        {
            return v.c0.none() && v.c1.none() && v.c2.none() && v.c3.none();
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool one(this bool2 v)
        {
            return (v.x && !v.y) || (!v.x && v.y);
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool one(this bool3 v)
        {
            return (v.x && !v.y && !v.z) || (!v.x && v.y && !v.z) || (!v.x && !v.y && v.z);
        }

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool one(this bool4 v)
        {
            return (v.x && !v.y && !v.z && !v.w) ||
                   (!v.x && v.y && !v.z && !v.w) ||
                   (!v.x && !v.y && v.z && !v.w) ||
                   (!v.x && !v.y && !v.z && v.w);
        }
    }
}
