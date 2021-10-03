using Appalachia.Core.Collections;
using Appalachia.Core.Collections.Implementations.Lists;

namespace Appalachia.Core.RateLimiting
{
    public class RateLimit
    {
        public float periodMilliseconds;
        public AppaList<float> timings;

        public RateLimit(int maxCalls, float periodMilliseconds)
        {
            this.periodMilliseconds = periodMilliseconds;
            this.timings = new AppaList_float(maxCalls);
        }
    }
}
