using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Math.Probability
{
    public struct Randomizer
    {
        #region Constants and Static Readonly

        public const float denominator = 1f / 0x80000000;

        #endregion

        #region Static Fields and Autoproperties

        public static int seed;

        #endregion

        public static float plusMinusOne => next * denominator;
        public static float zeroToOne => (plusMinusOne + 1f) * 0.5f;

        public static int next
        {
            get { return seed = (seed + 35757) * 31313; }
        }

        [DebuggerStepThrough]
        public static int operator %(Randomizer _, int count)
        {
            return Mathf.FloorToInt(zeroToOne * (count - 1));
        }
    }
} // Hapki.Randomization
