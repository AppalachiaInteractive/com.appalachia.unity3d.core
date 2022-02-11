using System;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Reflection.Extensions;
using Unity.Profiling;
using UnityEngine;

// ReSharper disable MemberHidesStaticFromOuterClass
// ReSharper disable StaticMemberInGenericType

namespace Appalachia.Core.Objects.Availability
{
    #region Nested type: AvailabilitySet

    public class AvailabilitySet : IAvailabilitySet
    {
        public AvailabilitySet(Type owner, int? sortOrder = null)
        {
            var order = sortOrder ?? owner.GetBaseClassCount();

            _sortOrder = order;
        }

        #region Fields and Autoproperties

        private readonly int _sortOrder;

        #endregion

        public int SortOrder => _sortOrder;

        #region IAvailabilitySet Members

        public IClosedAvailabilitySet<T1> Any<T1>(T1 unused)
            where T1 : class, IAvailabilityMarker
        {
            using (_PRF_Any.Auto())
            {
                var result = new AvailabilitySet<T1>(_sortOrder)
                {
                    Data1 = AvailabilityData<T1>.ForAny<T1>(_sortOrder)
                };

                result.Data1.AvailabilityChanged.Event += result.OnAvailabilityChanged;

                return result;
            }
        }

        public IEnabledAvailabilitySet<T1> AnyComponent<T1>(T1 unused)
            where T1 : Component
        {
            using (_PRF_AnyComponent.Auto())
            {
                return new EnabledAvailabilitySet<T1>();
            }
        }

        public IClosedAvailabilitySet<T1> Any<T1>()
            where T1 : class, IAvailabilityMarker
        {
            using (_PRF_Any.Auto())
            {
                var result = new AvailabilitySet<T1>(_sortOrder)
                {
                    Data1 = AvailabilityData<T1>.ForAny<T1>(_sortOrder)
                };

                result.Data1.AvailabilityChanged.Event += result.OnAvailabilityChanged;

                return result;
            }
        }

        public IEnabledAvailabilitySet<T1> AnyComponent<T1>()
            where T1 : Component
        {
            using (_PRF_AnyComponent.Auto())
            {
                return new EnabledAvailabilitySet<T1>();
            }
        }

        public IAvailabilitySet<T1> Behaviour<T1>(T1 unused)
            where T1 : SingletonAppalachiaBehaviour<T1>, ISingleton<T1>
        {
            using (_PRF_Behaviour.Auto())
            {
                return Behaviour<T1>();
            }
        }

        public IAvailabilitySet<T1> Behaviour<T1>()
            where T1 : SingletonAppalachiaBehaviour<T1>, ISingleton<T1>
        {
            using (_PRF_Behaviour.Auto())
            {
                var result = new AvailabilitySet<T1>(_sortOrder)
                {
                    Data1 = AvailabilityData<T1>.ForBehaviour<T1>(_sortOrder)
                };

                result.Data1.AvailabilityChanged.Event += result.OnAvailabilityChanged;

                return result;
            }
        }

        public IAvailabilitySet<T1> Object<T1>(T1 unused)
            where T1 : SingletonAppalachiaObject<T1>, ISingleton<T1>
        {
            using (_PRF_Object.Auto())
            {
                var result = new AvailabilitySet<T1>(_sortOrder)
                {
                    Data1 = AvailabilityData<T1>.ForObject<T1>(_sortOrder)
                };

                result.Data1.AvailabilityChanged.Event += result.OnAvailabilityChanged;

                return result;
            }
        }

        public IAvailabilitySet<T1> Object<T1>()
            where T1 : SingletonAppalachiaObject<T1>, ISingleton<T1>
        {
            using (_PRF_Object.Auto())
            {
                var result = new AvailabilitySet<T1>(_sortOrder)
                {
                    Data1 = AvailabilityData<T1>.ForObject<T1>(_sortOrder)
                };

                result.Data1.AvailabilityChanged.Event += result.OnAvailabilityChanged;

                return result;
            }
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(AvailabilitySet) + ".";

        private static readonly ProfilerMarker _PRF_Any = new ProfilerMarker(_PRF_PFX + nameof(Any));

        private static readonly ProfilerMarker _PRF_AnyComponent =
            new ProfilerMarker(_PRF_PFX + nameof(AnyComponent));

        private static readonly ProfilerMarker _PRF_Object = new ProfilerMarker(_PRF_PFX + nameof(Object));

        private static readonly ProfilerMarker _PRF_Behaviour =
            new ProfilerMarker(_PRF_PFX + nameof(Behaviour));

        #endregion
    }

    #endregion
}
