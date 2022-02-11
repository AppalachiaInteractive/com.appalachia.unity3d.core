using System;
using System.Collections.Generic;
using System.Linq;
using Appalachia.Utility.Logging;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Events.Collections
{
    public abstract class EventSubscribersCollection<T> : ISerializationCallbackReceiver
        where T : MulticastDelegate
    {
        #region Fields and Autoproperties

        private bool _currentlyInvoking;
        private bool _lockedToModifications;
        private bool _pendingClear;

        private Dictionary<int, T> _pendingAdds;
        private Dictionary<int, T> _pendingRemoves;
        private Dictionary<int, T> _subscribers;

        private object _lock;

        private T[] _subscriberArray;

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

        private IReadOnlyCollection<T> Subscribers
        {
            get
            {
                Initialize();
                return _subscribers.Values;
            }
        }

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
                        _pendingAdds.Add(hashCode, subscriber);
                    }
                }
                else if (!alreadySubscribed)
                {
                    _subscribers.Add(hashCode, subscriber);
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

        internal void InvokeSafe(Action<T> invocation, params IDisposable[] disposeAfter)
        {
            using (_PRF_InvokeSafe.Auto())
            {
                InvokeSafe(invocation);

                for (var index = 0; index < disposeAfter.Length; index++)
                {
                    var disposable = disposeAfter[index];
                    disposable?.Dispose();
                }
            }
        }

        internal void InvokeSafe(Action<T> invocation)
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
                            _subscribers.Add(pendingAdd.Key, pendingAdd.Value);
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
                    Log.Warn("Recursive invocation of events!");
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
                        if (duration.TotalMilliseconds > 20f)
                        {
                            Log.Warn(
                                ZString.Format("Invoking this event took {0}ms.", duration.TotalMilliseconds)
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
                var method = subscriber.Method;
                hashCode.Add(target);
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
                Initialize();
                _subscriberArray = _subscribers.Values.ToArray();
            }
        }

        public void OnAfterDeserialize()
        {
            using (_PRF_OnAfterDeserialize.Auto())
            {
                Initialize();

                if (_subscriberArray != null)
                {
                    for (var subscriberIndex = 0;
                         subscriberIndex < _subscriberArray.Length;
                         subscriberIndex++)
                    {
                        var subscriber = _subscriberArray[subscriberIndex];
                        var hashCode = GetSubscriberHashCode(subscriber);

                        _subscribers.Add(hashCode, subscriber);
                    }
                }
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
