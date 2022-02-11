using Appalachia.Core.Events;
using UnityEngine;

namespace Appalachia.Core.Objects.Availability
{
    public interface IEnabledAvailabilitySet<T1>
        where T1 : Component
    {
        void IsEnabledThen(ComponentEvent<T1>.Handler handler);
    }
}
