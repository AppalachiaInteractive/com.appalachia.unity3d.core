using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Events;
using UnityEngine;

namespace Appalachia.Core.Objects.Availability
{
    public interface IEnabledAvailabilitySet<T1>
        where T1 : class, IEnableNotifier
    {
        void IsEnabledThen(AppaEvent<T1>.Handler handler);
    }
}
