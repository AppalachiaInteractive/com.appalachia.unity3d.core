using System;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;

namespace Appalachia.Core.Objects.Availability
{
    public interface IAvailabilitySet<out T1, out T2, out T3, out T4, out T5, out T6, out T7, out T8>
    {
        IAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, TNext> AndBehaviour<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>;

        IAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, TNext> AndBehaviour<TNext>(TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>;

        IAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, TNext> AndObject<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>;

        IAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, TNext> AndObject<TNext>(TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>;

        void AreAvailableThen(Action<T1, T2, T3, T4, T5, T6, T7, T8> action);
    }
}
