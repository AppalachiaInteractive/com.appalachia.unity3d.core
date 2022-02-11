using System;

namespace Appalachia.Core.Objects.Availability
{
    public class InstanceAvailableWrapper : ITypedInstanceRegistrationWrapper,
                                            ISortedInstanceRegistrationWrapper
    {
        #region Fields and Autoproperties

        public int? sortOrder;
        public Type interestedType;

        private IAvailabilitySet _when;

        #endregion

        public ITypedInstanceRegistrationWrapper For<T>()
        {
            interestedType = typeof(T);
            return this;
        }

        public ITypedInstanceRegistrationWrapper For(Type t)
        {
            interestedType = t;
            return this;
        }

        public ISortedInstanceRegistrationWrapper WithExplicitSort(int sort)
        {
            sortOrder = sort;
            return this;
        }

        public ISortedInstanceRegistrationWrapper WithoutSorting()
        {
            sortOrder = int.MaxValue;
            return this;
        }

        #region ITypedInstanceRegistrationWrapper Members

        public IAvailabilitySet When
        {
            get
            {
                _when ??= new AvailabilitySet(interestedType, sortOrder);
                return _when;
            }
        }

        #endregion
    }
}
