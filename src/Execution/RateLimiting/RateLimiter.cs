#region

using System;
using Appalachia.Core.Collections.NonSerialized;

#endregion

namespace Appalachia.Core.Execution.RateLimiting
{
    public static class RateLimiter
    {
        private const int _initialSize = 32;

        private static NonSerializedAppaLookup<string, RateLimit> _limits;
        private static bool _initialized;

        private static bool Initialize()
        {
            if (_initialized)
            {
                return false;
            }

            _limits = new NonSerializedAppaLookup<string, RateLimit>();
            _initialized = true;
            return true;
        }

        private static void Setup(string key, int maxCalls, float periodMilliseconds)
        {
            Initialize();

            if (!_limits.ContainsKey(key))
            {
                _limits.Add(key, new RateLimit(maxCalls, periodMilliseconds));
            }
        }

        public static void DoXTimesEvery(
            string key,
            Action a,
            int maxCalls,
            float periodMilliseconds,
            float currentTime)
        {
            Setup(key, maxCalls, periodMilliseconds);

            if (ShouldDo(key, currentTime))
            {
                a();
            }
        }

        private static bool ShouldDo(string key, float currentTime)
        {
            if (Initialize())
            {
                return false;
            }

            var limit = _limits[key];

            for (var i = 0; i < limit.timings.Count; i++)
            {
                var lastTime = limit.timings[i];
                var elapsed = currentTime - lastTime;

                if (elapsed > limit.periodMilliseconds)
                {
                    limit.timings[i] = currentTime;
                    return true;
                }
            }

            return false;
        }

        public static void Dispose()
        {
            if (_limits != null)
            {
                for (var i = 0; i < _limits.Count; i++)
                {
                    _limits.at[i].timings.Dispose();
                    _limits.at[i].timings = null;
                }

                _limits.Clear();
            }
        }
    }
}
