using Appalachia.Core.Collections;
using Appalachia.Core.Collections.Implementations.Lists;
using Appalachia.Core.Objects.Root;

namespace Appalachia.Core.Execution.RateLimiting
{
    public class RateLimit : AppalachiaBase
    {
        public RateLimit(int maxCalls, float periodMilliseconds)
        {
            this.periodMilliseconds = periodMilliseconds;
            timings = new AppaList_float(maxCalls);
        }

        public AppaList<float> timings;
        public float periodMilliseconds;
    }
}
