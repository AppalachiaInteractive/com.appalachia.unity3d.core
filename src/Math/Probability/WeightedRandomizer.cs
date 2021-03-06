using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Math.Probability
{
    public struct WeightedRandomizer
    {
        #region Fields and Autoproperties

        public List<float> weights;

        #endregion

        [DebuggerStepThrough]
        public static implicit operator WeightedRandomizer(int _)
        {
            return new() { weights = new List<float>() };
        }

        [DebuggerStepThrough]
        public static int operator %(WeightedRandomizer rand, int count)
        {
            float sum;
            int i;
            if (rand.weights.Count != count)
            {
                rand.weights.Clear();
                sum = count;
                for (i = 0; i < count; ++i)
                {
                    rand.weights.Add(1f);
                }
            }
            else
            {
                sum = 0f;
                for (i = 0; i < count; ++i)
                {
                    const float restore = 0.1f;
                    rand.weights[i] = Mathf.Clamp01(rand.weights[i] + restore);
                    sum += rand.weights[i];
                }
            }

            var val = sum * Randomizer.zeroToOne;
            for (i = 0; (i < (count - 1)) && (val >= rand.weights[i]); ++i)
            {
                val -= rand.weights[i];
            }

            const float penalty = 1f;
            rand.weights[i] = Mathf.Clamp01(rand.weights[i] - penalty);

            return i;
        }
    }
}
