using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Types
{
// Q10.6 signed fixed point type

    public struct Fixed16
    {
        public short bits;

        public static Fixed16 FromBits(short v)
        {
            return new() {bits = v};
        }

        [DebuggerStepThrough] public static explicit operator Fixed16(int v)
        {
            return new() {bits = (short) (v << 6)};
        }

        [DebuggerStepThrough] public static explicit operator Fixed16(float v)
        {
            var x = Mathf.Floor(v);
            return new Fixed16 {bits = (short) (((int) x << 6) + Mathf.RoundToInt((v - x) * 64f))};
        }

        [DebuggerStepThrough] public static explicit operator float(Fixed16 v)
        {
            return (v.bits >> 6) + ((v.bits & 0x3f) * 0.015625f);
        }

        [DebuggerStepThrough] public static Fixed16 operator +(Fixed16 x, Fixed16 y)
        {
            return new() {bits = (short) (x.bits + y.bits)};
        }

        [DebuggerStepThrough] public static Fixed16 operator -(Fixed16 x, Fixed16 y)
        {
            return new() {bits = (short) (x.bits - y.bits)};
        }

        [DebuggerStepThrough] public static Fixed16 operator *(Fixed16 x, Fixed16 y)
        {
            return new() {bits = (short) ((x.bits * y.bits) >> 6)};
        }

        [DebuggerStepThrough] public static Fixed16 operator /(Fixed16 x, Fixed16 y)
        {
            return new() {bits = (short) ((x.bits << 6) / y.bits)};
        }

        [DebuggerStepThrough] public static bool operator <(Fixed16 x, Fixed16 y)
        {
            return x.bits < y.bits;
        }

        [DebuggerStepThrough] public static bool operator >(Fixed16 x, Fixed16 y)
        {
            return x.bits > y.bits;
        }

        [DebuggerStepThrough] public static bool operator <=(Fixed16 x, Fixed16 y)
        {
            return x.bits <= y.bits;
        }

        [DebuggerStepThrough] public static bool operator >=(Fixed16 x, Fixed16 y)
        {
            return x.bits >= y.bits;
        }

        public static Fixed16 Abs(Fixed16 v)
        {
            var x = v.bits >> 6;
            return FromBits((short) (((x >= 0 ? x : -x) << 6) + (v.bits & 0x3f)));
        }
    }
} // Hapki
