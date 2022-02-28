using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Events;
using Appalachia.Core.Objects.Routing;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Availability
{
    public class EnabledAvailabilitySet<T1> : IEnabledAvailabilitySet<T1>
        where T1 : class, IEnableNotifier
    {
        #region IEnabledAvailabilitySet<T1> Members

        public void IsEnabledThen(AppaEvent<T1>.Handler handler)
        {
            using (_PRF_IsEnabledThen.Auto())
            {
                ObjectEnableEventRouter.SubscribeTo(handler);
            }
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(EnabledAvailabilitySet<T1>) + ".";

        private static readonly ProfilerMarker _PRF_IsEnabledThen =
            new ProfilerMarker(_PRF_PFX + nameof(IsEnabledThen));

        #endregion
    }
}
