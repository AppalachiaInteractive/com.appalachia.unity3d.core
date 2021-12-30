using Appalachia.Core.Collections;
using Appalachia.Core.Collections.Implementations.Lists;

namespace Appalachia.Core.Execution.RateLimiting
{
    public class RateLimit
    {
        public RateLimit(int maxCalls, float periodMilliseconds)
        {
            this.periodMilliseconds = periodMilliseconds;
            timings = new AppaList_float(maxCalls);
        }

        #region Fields and Autoproperties

        public AppaList<float> timings;
        public float periodMilliseconds;

        #endregion
    }
}
