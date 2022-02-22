using System;
using System.Collections.Generic;
using System.Reflection;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Events;
using Appalachia.Utility.Events.Extensions;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Logging;
using Appalachia.Utility.Reflection.Extensions;
using Appalachia.Utility.Strings;
using Unity.Profiling;

namespace Appalachia.Core.Objects.Availability
{
    public abstract class AvailabilityData : /*don't bother AppalachiaSimpleNonSerializableBase,*/
        IComparable<AvailabilityData>,
        IComparable
    {
        #region Constants and Static Readonly

        public static IComparer<AvailabilityData> SortOrderComparer { get; } =
            new SortOrderRelationalComparer();

        #endregion

        protected AvailabilityData(int sortOrder)
        {
            _sortOrder = sortOrder;
        }

        #region Fields and Autoproperties

        public AppaEvent.Data AvailabilityChanged;

        protected bool _isElementAvailable;

        private readonly int _sortOrder;

        #endregion

        public bool IsAvailable => _isElementAvailable;

        public int SortOrder => _sortOrder;

        public static bool operator >(AvailabilityData left, AvailabilityData right)
        {
            return Comparer<AvailabilityData>.Default.Compare(left, right) > 0;
        }

        public static bool operator >=(AvailabilityData left, AvailabilityData right)
        {
            return Comparer<AvailabilityData>.Default.Compare(left, right) >= 0;
        }

        public static bool operator <(AvailabilityData left, AvailabilityData right)
        {
            return Comparer<AvailabilityData>.Default.Compare(left, right) < 0;
        }

        public static bool operator <=(AvailabilityData left, AvailabilityData right)
        {
            return Comparer<AvailabilityData>.Default.Compare(left, right) <= 0;
        }

        protected void OnAvailabilityChanged()
        {
            using (_PRF_OnAvailabilityChanged.Auto())
            {
                AvailabilityChanged.RaiseEvent();
            }
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return 1;
            }

            if (ReferenceEquals(this, obj))
            {
                return 0;
            }

            return obj is AvailabilityData other
                ? CompareTo(other)
                : throw new ArgumentException($"Object must be of type {nameof(AvailabilityData)}");
        }

        #endregion

        #region IComparable<AvailabilityData> Members

        public int CompareTo(AvailabilityData other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }

            return SortOrder.CompareTo(other.SortOrder);
        }

        #endregion

        #region Nested type: SortOrderRelationalComparer

        private sealed class SortOrderRelationalComparer : IComparer<AvailabilityData>
        {
            #region IComparer<AvailabilityData> Members

            public int Compare(AvailabilityData x, AvailabilityData y)
            {
                if (ReferenceEquals(x, y))
                {
                    return 0;
                }

                if (ReferenceEquals(null, y))
                {
                    return 1;
                }

                if (ReferenceEquals(null, x))
                {
                    return -1;
                }

                return x.SortOrder.CompareTo(y.SortOrder);
            }

            #endregion
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(AvailabilityData) + ".";

        private static readonly ProfilerMarker _PRF_OnAvailabilityChanged =
            new ProfilerMarker(_PRF_PFX + nameof(OnAvailabilityChanged));

        #endregion
    }

    internal class AvailabilityData<T> : AvailabilityData
    {
        #region Constants and Static Readonly

        private const string EVENT_NAME = "InstanceAvailable";

        #endregion

        private AvailabilityData(int sortOrder) : base(sortOrder)
        {
        }

        #region Fields and Autoproperties

        private T _instance;

        #endregion

        public T instance => _instance;

        public static Delegate CreateHandleAvailabilityDelegate<T1>(Type type, AvailabilityData<T1> result)
        {
            using (_PRF_CreateHandleAvailabilityDelegate.Auto())
            {
                var targetType = result.GetType();
                var targetMethod = targetType.GetMethod(nameof(HandleAvailabilityGeneric));
                var genericMethod = targetMethod.MakeGenericMethod(type);

                var instanceAvailableHandler =
                    typeof(SingletonAppalachiaBehaviour<>.InstanceAvailableHandler).MakeGenericType(type);

                return Delegate.CreateDelegate(instanceAvailableHandler, result, genericMethod);
            }
        }

        public static AvailabilityData<T1> ForAny<T1>(int sortOrder)
            where T1 : IAvailabilityMarker
        {
            using (_PRF_ForObject.Auto())
            {
                var result = new AvailabilityData<T1>(sortOrder);

                var implementors = ReflectionExtensions.GetAllConcreteImplementors<T1>();

                if (implementors.Count == 0)
                {
                    AppaLog.Warn(
                        ZString.Format(
                            "There are no concrete implementors of {0}.",
                            typeof(T1).FormatForLogging()
                        )
                    );
                }

                var eventFlags = BindingFlags.DeclaredOnly |
                                 BindingFlags.Public |
                                 BindingFlags.NonPublic |
                                 BindingFlags.Static;

                foreach (var implementor in implementors)
                {
                    var originalImplementorType = implementor;
                    var currentCheckingType = originalImplementorType;

                    var matched = false;

                    while (currentCheckingType != null)
                    {
                        AppalachiaApplication.EnsureStaticConstructorHasBeenCalled(currentCheckingType);
                        
                        var events = currentCheckingType.GetEvents(eventFlags);

                        foreach (var availableEvent in events)
                        {
                            if (availableEvent.Name == EVENT_NAME)
                            {
                                var delegateInstance =
                                    CreateHandleAvailabilityDelegate(originalImplementorType, result);

                                var addMethod = availableEvent.GetAddMethod(true);
                                addMethod.Invoke(null, new object[] { delegateInstance });

                                //availableEvent.AddEventHandler(null, delegateInstance);
                                matched = true;
                                break;
                            }
                        }

                        if (matched)
                        {
                            break;
                        }

                        currentCheckingType = currentCheckingType.BaseType;
                    }
                }

                return result;
            }
        }

        public static AvailabilityData<T1> ForBehaviour<T1>(int sortOrder)
            where T1 : SingletonAppalachiaBehaviour<T1>, ISingleton<T1>
        {
            using (_PRF_ForBehaviour.Auto())
            {
                var result = new AvailabilityData<T1>(sortOrder);
                SingletonAppalachiaBehaviour<T1>.InstanceAvailable += result.HandleAvailability;

                return result;
            }
        }

        public static AvailabilityData<T1> ForObject<T1>(int sortOrder)
            where T1 : SingletonAppalachiaObject<T1>, ISingleton<T1>
        {
            using (_PRF_ForObject.Auto())
            {
                var result = new AvailabilityData<T1>(sortOrder);
                SingletonAppalachiaObject<T1>.InstanceAvailable += result.HandleAvailability;

                return result;
            }
        }

        public void HandleAvailability(T i)
        {
            using (_PRF_HandleAvailability.Auto())
            {
                _isElementAvailable = true;
                _instance = i;
                OnAvailabilityChanged();
            }
        }

        public void HandleAvailabilityGeneric<TOther>(TOther value)
            where TOther : SingletonAppalachiaBehaviour<TOther>
        {
            using (_PRF_HandleAvailabilityGeneric.Auto())
            {
                if (value is T i)
                {
                    HandleAvailability(i);
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AvailabilityData<T>) + ".";

        private static readonly ProfilerMarker _PRF_HandleAvailabilityGeneric =
            new ProfilerMarker(_PRF_PFX + nameof(HandleAvailabilityGeneric));

        private static readonly ProfilerMarker _PRF_CreateHandleAvailabilityDelegate =
            new ProfilerMarker(_PRF_PFX + nameof(CreateHandleAvailabilityDelegate));

        private static readonly ProfilerMarker _PRF_ForBehaviour =
            new ProfilerMarker(_PRF_PFX + nameof(ForBehaviour));

        private static readonly ProfilerMarker _PRF_ForObject =
            new ProfilerMarker(_PRF_PFX + nameof(ForObject));

        private static readonly ProfilerMarker _PRF_HandleAvailability =
            new ProfilerMarker(_PRF_PFX + nameof(HandleAvailability));

        #endregion
    }
}
