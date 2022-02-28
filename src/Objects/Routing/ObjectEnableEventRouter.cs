using System;
using System.Collections.Generic;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Events;
using Appalachia.Utility.Events.Extensions;
using Appalachia.Utility.Logging;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Routing
{
    public static class ObjectEnableEventRouter
    {
        #region Static Fields and Autoproperties

        private static Dictionary<Type, List<object>> _pendingInstances;

        private static Dictionary<Type, object> _eventLookup;

        #endregion

        public static void Notify<T>(Type type, T instance)
            where T : class, IEnableNotifier
        {
            using (_PRF_Notify.Auto())
            {
                Initialize();

                if (!_eventLookup.ContainsKey(type))
                {
                    if (instance.GuaranteedEventRouting)
                    {
                        if (_pendingInstances.ContainsKey(type))
                        {
                            _pendingInstances[type].Add(instance);
                        }
                        else
                        {
                            _pendingInstances[type] = new List<object> { instance };
                        }
                    }

                    return;
                }

                var storedEventData = _eventLookup[type];
                var targetEventType = typeof(AppaEvent<T>.Data);

                if (storedEventData.GetType() != targetEventType)
                {
                    AppaLog.Context.Dependencies.Error(
                        ZString.Format(
                            "Could not publish a notification for {0} component enable event data type {1} because the subscriber was stored as a {2}.",
                            type.FormatForLogging(),
                            targetEventType.FormatForLogging(),
                            storedEventData.GetType()
                        )
                    );
                }

                var eventData = (AppaEvent<T>.Data)storedEventData;

                eventData.RaiseEvent(instance);
            }
        }

        public static void Notify<T>(T instance)
            where T : class, IEnableNotifier
        {
            using (_PRF_Notify.Auto())
            {
                Notify(instance.GetType(), instance);
            }
        }

        public static void SubscribeTo<T>(AppaEvent<T>.Handler handler)
            where T : class, IEnableNotifier
        {
            using (_PRF_SubscribeTo.Auto())
            {
                Initialize();

                var type = typeof(T);

                AppaEvent<T>.Data eventData;
                var add = false;

                if (_eventLookup.ContainsKey(type))
                {
                    eventData = (AppaEvent<T>.Data)_eventLookup[type];
                }
                else
                {
                    eventData = new();
                    add = true;
                }

                eventData.Event += handler;

                if (add)
                {
                    _eventLookup.Add(type, eventData);
                }
                else
                {
                    _eventLookup[type] = eventData;
                }

                if (_pendingInstances.ContainsKey(type))
                {
                    var list = _pendingInstances[type];

                    for (var index = list.Count - 1; index >= 0; index--)
                    {
                        var pendingInstance = list[index];

                        eventData.RaiseEvent(pendingInstance as T);

                        list.RemoveAt(index);
                    }
                }
            }
        }

        private static void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                _pendingInstances ??= new();
                _eventLookup ??= new();
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ObjectEnableEventRouter) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_Notify = new ProfilerMarker(_PRF_PFX + nameof(Notify));

        private static readonly ProfilerMarker _PRF_SubscribeTo =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeTo));

        #endregion
    }
}
