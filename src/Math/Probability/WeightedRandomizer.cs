using System.Collections.Generic;
using UnityEngine;

namespace Appalachia.Core.Probability
{
    public struct WeightedRandomizer {
        public List<float> weights;

        public static implicit operator WeightedRandomizer(int _) {
            return new WeightedRandomizer {weights = new List<float>()};
        }

        public static int operator%(WeightedRandomizer rand, int count) {
            float sum;
            int i;
            if (rand.weights.Count != count) {
                rand.weights.Clear();
                sum = count;
                for (i = 0; i < count; ++i)
                    rand.weights.Add(1f);
            } else {
                sum = 0f;
                for (i = 0; i < count; ++i) {
                    const float restore = 0.1f;
                    rand.weights[i] = Mathf.Clamp01(rand.weights[i] + restore);
                    sum += rand.weights[i];
                }
            }

            float val = sum * Randomizer.zeroToOne;
            for (i = 0; i < count - 1 && val >= rand.weights[i]; ++i)
                val -= rand.weights[i];

            const float penalty = 1f;
            rand.weights[i] = Mathf.Clamp01(rand.weights[i] - penalty);

            return i;
        }
    }
}