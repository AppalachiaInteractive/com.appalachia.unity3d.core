/*
namespace Appalachia.Core.Objects.Availability
{
    public abstract class BaseAvailabilitySet
    {
        public abstract bool IsFullyAvailable { get; }

        public void OnAvailabilityChanged()
        {
            if (IsFullyAvailable)
            {
                OnFullyAvailable();
            }
        }

        protected abstract void OnFullyAvailable();
    }
}
*/
/*
namespace Appalachia.Core.Objects.Availability
{
    public abstract class BaseAvailabilitySet
    {
        public abstract bool IsFullyAvailable { get; }

        public void OnAvailabilityChanged()
        {
            if (IsFullyAvailable)
            {
                OnFullyAvailable();
            }
        }

        protected abstract void OnFullyAvailable();
    }
}
*/
/*
using System;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using UnityEngine;

namespace Appalachia.Core.Objects.Availability
{
    public interface IAvailabilitySet
    {
        IClosedAvailabilitySet<T1> Any<T1>()
            where T1 : class, IAvailabilityMarker;

        IEnabledAvailabilitySet<T1> AnyComponent<T1>()
            where T1 : Component;

        IAvailabilitySet<T1> Behaviour<T1>()
            where T1 : SingletonAppalachiaBehaviour<T1>, ISingleton<T1>;

        // ReSharper disable once UnusedParameter.Global
        IAvailabilitySet<T1> Behaviour<T1>(T1 unused)
            where T1 : SingletonAppalachiaBehaviour<T1>, ISingleton<T1>;

        // ReSharper disable once UnusedParameter.Global
        IAvailabilitySet<T1> Object<T1>(T1 unused)
            where T1 : SingletonAppalachiaObject<T1>, ISingleton<T1>;

        IAvailabilitySet<T1> Object<T1>()
            where T1 : SingletonAppalachiaObject<T1>, ISingleton<T1>;
    }

    public interface IAvailabilitySet<T1>
    {
        IAvailabilitySet<T1, TNext> AndBehaviour<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>;

        IAvailabilitySet<T1, TNext> AndObject<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>;

        void IsAvailableThen(Action<T1> action);
    }

    public interface IAvailabilitySet<T1, T2>
    {
        IAvailabilitySet<T1, T2, TNext> AndBehaviour<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>;

        IAvailabilitySet<T1, T2, TNext> AndObject<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>;

        void AreAvailableThen(Action<T1, T2> action);
    }

    public interface IAvailabilitySet<T1, T2, T3>
    {
        IAvailabilitySet<T1, T2, T3, TNext> AndBehaviour<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>;

        IAvailabilitySet<T1, T2, T3, TNext> AndObject<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>;

        void AreAvailableThen(Action<T1, T2, T3> action);
    }

    public interface IAvailabilitySet<T1, T2, T3, T4>
    {
        IAvailabilitySet<T1, T2, T3, T4, TNext> AndBehaviour<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>;

        IAvailabilitySet<T1, T2, T3, T4, TNext> AndObject<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>;

        void AreAvailableThen(Action<T1, T2, T3, T4> action);
    }

    public interface IAvailabilitySet<T1, T2, T3, T4, T5>
    {
        IAvailabilitySet<T1, T2, T3, T4, T5, TNext> AndBehaviour<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>;

        IAvailabilitySet<T1, T2, T3, T4, T5, TNext> AndObject<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>;

        void AreAvailableThen(Action<T1, T2, T3, T4, T5> action);
    }

    public interface IAvailabilitySet<T1, T2, T3, T4, T5, T6>
    {
        void AreAvailableThen(Action<T1, T2, T3, T4, T5, T6> action);
    }
}
*/


