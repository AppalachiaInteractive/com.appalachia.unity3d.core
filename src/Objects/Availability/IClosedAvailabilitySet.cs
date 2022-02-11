using System;

namespace Appalachia.Core.Objects.Availability
{
    public interface IClosedAvailabilitySet<T1>
    {
        void IsAvailableThen(Action<T1> action);
    }
}
