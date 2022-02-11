using System;

namespace Appalachia.Core.Objects.Availability
{
    public interface IAvailabilitySet<out T1, out T2, out T3, out T4, out T5, out T6, out T7, out T8, out T9,
                                      out T10>
    {
        void AreAvailableThen(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action);
    }
}
