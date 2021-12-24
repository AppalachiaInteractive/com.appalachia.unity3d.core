#region

using System;
using System.Collections.Generic;
using Appalachia.CI.Constants;
using Random = UnityEngine.Random;

#endregion

namespace Appalachia.Core.Math.Probability
{
    [Serializable]
    public class ProbabilitySet 
    {
        [NonSerialized] private AppaContext _context;

        protected AppaContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new AppaContext(this);
                }

                return _context;
            }
        }
        public double sum;

        public ProbabilityPair[] pairs;

        public int GetNextIndex()
        {
            var random = Random.Range(0.0f, 1.0f);

            for (var i = 0; i < pairs.Length; i++)
            {
                var pair = pairs[i];

                if ((pair.inclusiveStart <= random) && (pair.exclusiveEnd > random))
                {
                    return i;
                }
            }

            return pairs.Length - 1;
        }

        public void ValidateProbabilities(IReadOnlyList<IProbabilityProvider> providers)
        {
            var update = pairs == null;

            if (!update && (pairs.Length != providers.Count))
            {
                update = true;
            }

            if (!update)
            {
                var tempSum = 0.0;

                for (var i = 0; i < providers.Count; i++)
                {
                    var prefab = providers[i];

                    tempSum += prefab.Enabled ? prefab.Probability : 0.0;
                }

                if (System.Math.Abs(tempSum - sum) > float.Epsilon)
                {
                    update = true;
                }
            }

            if (update)
            {
                pairs = new ProbabilityPair[providers.Count];

                sum = 0.0;

                for (var i = 0; i < providers.Count; i++)
                {
                    var prefab = providers[i];

                    sum += prefab.Enabled ? prefab.Probability : 0.0;
                }

                var previousEnd = 0.0;

                for (var i = 0; i < providers.Count; i++)
                {
                    var prefab = providers[i];

                    var start = previousEnd;
                    var denom = sum == 0.0 ? 1.0 : sum;
                    var end = start + ((prefab.Enabled ? prefab.Probability : 0.0) / denom);

                    pairs[i] = new ProbabilityPair {inclusiveStart = start, exclusiveEnd = end};

                    previousEnd = end;
                }

                if (pairs.Length > 0)
                {
                    pairs[pairs.Length - 1].exclusiveEnd = 1.0;
                }
            }
        }
    }
}
