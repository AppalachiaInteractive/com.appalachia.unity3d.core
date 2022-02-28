using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using UnityEngine;

namespace Appalachia.Core.Objects.Availability
{
    public interface IAvailabilitySet
    {
        public IClosedAvailabilitySet<T1> Any<T1>()
            where T1 : class, IAvailabilityMarker;

        public IClosedAvailabilitySet<T1> Any<T1>(T1 unused)
            where T1 : class, IAvailabilityMarker;

        public IEnabledAvailabilitySet<T1> AnyInstance<T1>()
            where T1 : class, IEnableNotifier;

        public IEnabledAvailabilitySet<T1> AnyInstance<T1>(T1 unused)
            where T1 : class, IEnableNotifier;

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
}
