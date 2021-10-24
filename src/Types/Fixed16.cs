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

        public static explicit operator Fixed16(int v)
        {
            return new() {bits = (short) (v << 6)};
        }

        public static explicit operator Fixed16(float v)
        {
            var x = Mathf.Floor(v);
            return new Fixed16 {bits = (short) (((int) x << 6) + Mathf.RoundToInt((v - x) * 64f))};
        }

        public static explicit operator float(Fixed16 v)
        {
            return (v.bits >> 6) + ((v.bits & 0x3f) * 0.015625f);
        }

        public static Fixed16 operator +(Fixed16 x, Fixed16 y)
        {
            return new() {bits = (short) (x.bits + y.bits)};
        }

        public static Fixed16 operator -(Fixed16 x, Fixed16 y)
        {
            return new() {bits = (short) (x.bits - y.bits)};
        }

        public static Fixed16 operator *(Fixed16 x, Fixed16 y)
        {
            return new() {bits = (short) ((x.bits * y.bits) >> 6)};
        }

        public static Fixed16 operator /(Fixed16 x, Fixed16 y)
        {
            return new() {bits = (short) ((x.bits << 6) / y.bits)};
        }

        public static bool operator <(Fixed16 x, Fixed16 y)
        {
            return x.bits < y.bits;
        }

        public static bool operator >(Fixed16 x, Fixed16 y)
        {
            return x.bits > y.bits;
        }

        public static bool operator <=(Fixed16 x, Fixed16 y)
        {
            return x.bits <= y.bits;
        }

        public static bool operator >=(Fixed16 x, Fixed16 y)
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
