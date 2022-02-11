using System;

namespace Appalachia.Core.Objects.Availability
{
    public static class RegisterInstanceCallbacks
    {
        public static ITypedInstanceRegistrationWrapper For<T>()
        {
            var wrapper = new InstanceAvailableWrapper();
            wrapper.For<T>();
            return wrapper;
        }

        public static ITypedInstanceRegistrationWrapper For(Type t)
        {
            var wrapper = new InstanceAvailableWrapper();
            wrapper.For(t);
            return wrapper;
        }

        public static ISortedInstanceRegistrationWrapper WithExplicitSort(int sortOrder)
        {
            var wrapper = new InstanceAvailableWrapper();
            wrapper.WithExplicitSort(sortOrder);
            return wrapper;
        }

        public static ISortedInstanceRegistrationWrapper WithoutSorting()
        {
            var wrapper = new InstanceAvailableWrapper();
            wrapper.WithoutSorting();
            return wrapper;
        }
    }
}
