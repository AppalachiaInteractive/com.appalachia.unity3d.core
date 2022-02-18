using System;
using System.Collections.Generic;
using Appalachia.Core.Events;
using Appalachia.Core.Events.Extensions;
using Appalachia.Utility.Constants;
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

        public static void Notify<T>(Type type, T instance, bool guaranteed)
            where T : Component
        {
            using (_PRF_Notify.Auto())
            {
                Initialize();

                if (!_eventLookup.ContainsKey(type))
                {
                    if (guaranteed)
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
                var targetEventType = typeof(ComponentEvent<T>.Data);

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

                var eventData = (ComponentEvent<T>.Data)storedEventData;

                eventData.RaiseEvent(instance);
            }
        }

        public static void Notify<T>(T instance, bool guaranteed)
            where T : Component
        {
            using (_PRF_Notify.Auto())
            {
                Notify(instance.GetType(), instance, guaranteed);
            }
        }

        public static void SubscribeTo<T>(ComponentEvent<T>.Handler handler)
            where T : Component
        {
            using (_PRF_SubscribeTo.Auto())
            {
                Initialize();

                var type = typeof(T);

                ComponentEvent<T>.Data eventData;
                var add = false;

                if (_eventLookup.ContainsKey(type))
                {
                    eventData = (ComponentEvent<T>.Data)_eventLookup[type];
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
