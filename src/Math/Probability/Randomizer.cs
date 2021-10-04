// Convenience utilities written by Malte Hildingsson, malte@hapki.se.
// No copyright is claimed, and you may use it for any purpose you like.
// No warranty for any purpose is expressed or implied.

using UnityEngine;

namespace Appalachia.Core.Math.Probability
{
    public struct Randomizer
    {
        public const float denominator = 1f / 0x80000000;

        public static int seed;

        public static int next
        {
            get { return seed = (seed + 35757) * 31313; }
        }

        public static float plusMinusOne => next * denominator;
        public static float zeroToOne => (plusMinusOne + 1f) * 0.5f;

        public static int operator %(Randomizer _, int count)
        {
            return Mathf.FloorToInt(zeroToOne * (count - 1));
        }
    }
} // Hapki.Randomization
