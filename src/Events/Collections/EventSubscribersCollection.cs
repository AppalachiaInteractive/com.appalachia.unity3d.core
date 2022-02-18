using System;
using System.Collections.Generic;
using System.Diagnostics;
using Appalachia.Core.Events.Contracts;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Logging;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Events.Collections
{
    public abstract class EventSubscribersCollection<T> : ISerializationCallbackReceiver
        where T : MulticastDelegate
    {
        #region Constants and Static Readonly

        private const int SUBSCRIBER_FLUSH_LIMIT = 1000;

        private const int SUBSCRIBER_LIMIT = 100;

        #endregion

        #region Fields and Autoproperties

        [NonSerialized] private bool _currentlyInvoking;
        [NonSerialized] private bool _lockedToModifications;
        [NonSerialized] private bool _pendingClear;

        [NonSerialized] private Dictionary<int, T> _pendingAdds;
        [NonSerialized] private Dictionary<int, T> _pendingRemoves;
        [NonSerialized] private Dictionary<int, T> _subscribers;

        [NonSerialized] private object _lock;

        #endregion

        public bool CurrentInvoking => _currentlyInvoking;

        public bool LockedToModifications => _lockedToModifications;

        public int SubscriberCount
        {
            get
            {
                Initialize();
                return _subscribers.Count;
            }
        }

        private AppaLogContext Log => AppaLog.Context.Events;

        public void Add(T subscriber)
        {
            using (_PRF_Add.Auto())
            {
                Initialize();

                GetSubscriberStatus(
                    subscriber,
                    out var hashCode,
                    out var pendingRemove,
                    out var pendingAdd,
                    out var alreadySubscribed
                );

                if (_lockedToModifications)
                {
                    if (pendingRemove)
                    {
                        _pendingRemoves.Remove(hashCode);
                    }
                    else if (!pendingAdd)
                    {
                        if (!_pendingAdds.ContainsKey(hashCode))
                        {
                            _pendingAdds.Add(hashCode, subscriber);
                        }
                    }
                }
                else if (!alreadySubscribed)
                {
                    if (SubscriberCount > SUBSCRIBER_LIMIT)
                    {
                        Log.Warn(
                            ZString.Format(
                                "The event has {0} subscribers, and more are subscribing.",
                                SubscriberCount
                            )
                        );
                    }

                    if (SubscriberCount > SUBSCRIBER_FLUSH_LIMIT)
                    {
                        Log.Warn(
                            ZString.Format(
                                "The event has {0} subscribers, which will now be removed.",
                                SubscriberCount
                            )
                        );

                        _subscribers.Clear();
                    }

                    if (!_subscribers.ContainsKey(hashCode))
                    {
                        _subscribers.Add(hashCode, subscriber);
                    }
                }
            }
        }

        public void Clear()
        {
            using (_PRF_Clear.Auto())
            {
                Initialize();

                if (_lockedToModifications)
                {
                    _pendingClear = true;
                }
                else
                {
                    _subscribers.Clear();
                }
            }
        }

        public void Remove(T subscriber)
        {
            using (_PRF_Remove.Auto())
            {
                Initialize();

                GetSubscriberStatus(
                    subscriber,
                    out var hashCode,
                    out var pendingRemove,
                    out var pendingAdd,
                    out var alreadySubscribed
                );

                if (_lockedToModifications)
                {
                    if (pendingAdd)
                    {
                        _pendingAdds.Remove(hashCode);
                    }
                    else if (!pendingRemove)
                    {
                        _pendingRemoves.Add(hashCode, subscriber);
                    }
                }
                else if (!alreadySubscribed)
                {
                    _subscribers.Remove(hashCode);
                }
            }
        }

        internal void InvokeSafe(
            Action<T> invocation,
            string callerFilePath,
            string callerMemberName,
            int callerLineNumber,
            params IDisposable[] disposeAfter)
        {
            using (_PRF_InvokeSafe.Auto())
            {
                InvokeSafe(invocation, callerFilePath, callerMemberName, callerLineNumber);

                for (var index = 0; index < disposeAfter.Length; index++)
                {
                    var disposable = disposeAfter[index];
                    disposable?.Dispose();
                }
            }
        }

        internal void InvokeSafe(
            Action<T> invocation,
            string callerFilePath,
            string callerMemberName,
            int callerLineNumber)
        {
            using (_PRF_InvokeSafe.Auto())
            {
                void SynchronizeCollections()
                {
                    using (_PRF_SynchronizeCollections.Auto())
                    {
                        if (_pendingClear)
                        {
                            _subscribers.Clear();
                            _pendingRemoves.Clear();
                            _pendingAdds.Clear();
                            return;
                        }

                        foreach (var pendingAdd in _pendingAdds)
                        {
                            if (!_subscribers.ContainsKey(pendingAdd.Key))
                            {
                                _subscribers.Add(pendingAdd.Key, pendingAdd.Value);
                            }
                        }

                        _pendingAdds.Clear();

                        foreach (var pendingRemove in _pendingRemoves)
                        {
                            _subscribers.Remove(pendingRemove.Key);
                        }

                        _pendingRemoves.Clear();
                    }
                }

                if (_currentlyInvoking)
                {
                    Log.Info(
                        ZString.Format(
                            "Recursive invocation of events! Initiated here: {0}",
                            callerFilePath.FormatCallerMembersForLogging(callerMemberName, callerLineNumber)
                        )
                    );
                    return;
                }

                Initialize();

                lock (_lock)
                {
                    var startTime = DateTime.UtcNow;

                    try
                    {
                        _currentlyInvoking = true;
                        _lockedToModifications = true;

                        if (SubscriberCount > SUBSCRIBER_LIMIT)
                        {
                            var message = ZString.Format(
                                "The event invoked at the following location has {0} subscribers: {1}",
                                SubscriberCount,
                                callerFilePath.FormatCallerMembersForLogging(
                                    callerMemberName,
                                    callerLineNumber
                                )
                            );
                            Log.Warn(message);

                            throw new NotSupportedException(message);
                        }

                        foreach (var subscriber in _subscribers)
                        {
                            invocation(subscriber.Value);
                        }
                    }
                    finally
                    {
                        SynchronizeCollections();

                        var endTime = DateTime.UtcNow;
                        var duration = endTime - startTime;

                        if ((duration.TotalMilliseconds > 20f) && !Debugger.IsAttached)
                        {
                            Log.Warn(
                                ZString.Format(
                                    "Invoking the following event took {0}ms: {1}",
                                    duration.TotalMilliseconds.FormatNumberForLogging(),
                                    callerFilePath.FormatCallerMembersForLogging(
                                        callerMemberName,
                                        callerLineNumber
                                    )
                                )
                            );
                        }

                        _lockedToModifications = false;
                        _currentlyInvoking = false;
                    }
                }
            }
        }

        private static int GetSubscriberHashCode(T subscriber)
        {
            using (_PRF_GetSubscriberHashCode.Auto())
            {
                var hashCode = new HashCode();

                var target = subscriber.Target;

                if (target is IUniqueSubscriber i)
                {
                    hashCode.Add(i.ObjectId);
                }
                else if (target is Component c)
                {
                    var path = c.transform.GetFullPath();

                    hashCode.Add(path);
                }
                else if (target is UnityEngine.Object o)
                {
                    var instanceId = o.GetInstanceID();

                    hashCode.Add(instanceId);
                }
                else
                {
                    hashCode.Add(target);
                }

                var method = subscriber.Method;
                hashCode.Add(method);

                return hashCode.ToHashCode();
            }
        }

        private void GetSubscriberStatus(
            T subscriber,
            out int hashCode,
            out bool pendingRemove,
            out bool pendingAdd,
            out bool alreadySubscribed)
        {
            using (_PRF_GetSubscriberStatus.Auto())
            {
                hashCode = GetSubscriberHashCode(subscriber);
                pendingRemove = _pendingRemoves.ContainsKey(hashCode);
                pendingAdd = _pendingAdds.ContainsKey(hashCode);
                alreadySubscribed = _subscribers.ContainsKey(hashCode);
            }
        }

        private void Initialize()
        {
            using (_PRF_InitailizeCollections.Auto())
            {
                _subscribers ??= new();
                _pendingAdds ??= new();
                _pendingRemoves ??= new();
                _lock = new();
            }
        }

        #region ISerializationCallbackReceiver Members

        public void OnBeforeSerialize()
        {
            using (_PRF_OnBeforeSerialize.Auto())
            {
            }
        }

        public void OnAfterDeserialize()
        {
            using (_PRF_OnAfterDeserialize.Auto())
            {
                Initialize();

                _subscribers.Clear();
                _pendingAdds.Clear();
                _pendingRemoves.Clear();
                _currentlyInvoking = false;
                _pendingClear = false;
            }
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(EventSubscribersCollection<T>) + ".";

        private static readonly ProfilerMarker _PRF_GetSubscriberStatus =
            new ProfilerMarker(_PRF_PFX + nameof(GetSubscriberStatus));

        private static readonly ProfilerMarker _PRF_GetSubscriberHashCode =
            new ProfilerMarker(_PRF_PFX + nameof(GetSubscriberHashCode));

        private static readonly ProfilerMarker _PRF_Clear = new ProfilerMarker(_PRF_PFX + nameof(Clear));

        private static readonly ProfilerMarker _PRF_InitailizeCollections =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_SynchronizeCollections =
            new ProfilerMarker(_PRF_PFX + "SynchronizeCollections");

        private static readonly ProfilerMarker _PRF_InvokeSafe =
            new ProfilerMarker(_PRF_PFX + nameof(InvokeSafe));

        private static readonly ProfilerMarker _PRF_Add = new ProfilerMarker(_PRF_PFX + nameof(Add));

        private static readonly ProfilerMarker _PRF_Remove = new ProfilerMarker(_PRF_PFX + nameof(Remove));

        private static readonly ProfilerMarker _PRF_OnBeforeSerialize =
            new ProfilerMarker(_PRF_PFX + nameof(OnBeforeSerialize));

        private static readonly ProfilerMarker _PRF_OnAfterDeserialize =
            new ProfilerMarker(_PRF_PFX + nameof(OnAfterDeserialize));

        #endregion
    }
}
