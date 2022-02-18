using System;
using System.Collections.Generic;
using Appalachia.Utility.Standards;
using Unity.Profiling;

namespace Appalachia.Core.Events.Collections
{
    public class ReusableDelegateCollection<TKey1, TKey2>
    {
        #region Static Fields and Autoproperties

        private static Dictionary<ObjectId, Dictionary<TKey1, Dictionary<TKey2, Action>>>
            _updateDelegatesByData;

        private static Dictionary<ObjectId, Dictionary<TKey1, Dictionary<TKey2, AppaEvent.Handler>>>
            _updateHandlersByData;

        #endregion

        /// <summary>
        ///     Subscribes the delegate to the provided event if it has not been previously subscribed, using the keys provided to ensure uniqueness.
        /// </summary>
        /// <param name="key1">The first delegate key.</param>
        /// <param name="key2">The second delegate key.</param>
        /// <param name="targetEvent">The event to subscribe to (once).</param>
        /// <param name="delegateCreator">A delegate to create the update/subscribe delegate.</param>
        /// <exception cref="ArgumentNullException">Thrown whenever any key is null.</exception>
        public void Subscribe(
            TKey1 key1,
            TKey2 key2,
            ref AppaEvent.Data targetEvent,
            Func<Action> delegateCreator)
        {
            using (_PRF_Subscribe.Auto())
            {
                if (key1 == null)
                {
                    throw new ArgumentNullException(nameof(key1), "The key may not be null!");
                }

                if (key2 == null)
                {
                    throw new ArgumentNullException(nameof(key2), "The key may not be null!");
                }

                _updateDelegatesByData ??= new();
                _updateHandlersByData ??= new();

                Dictionary<TKey2, Action> updateDelegates;
                Dictionary<TKey2, AppaEvent.Handler> updateHandlers;
                Action updateDelegate;
                AppaEvent.Handler updateHandler;

                // first
                if (!_updateDelegatesByData.ContainsKey(targetEvent.ObjectId))
                {
                    _updateDelegatesByData.Add(targetEvent.ObjectId, new());
                }

                if (!_updateHandlersByData.ContainsKey(targetEvent.ObjectId))
                {
                    _updateHandlersByData.Add(targetEvent.ObjectId, new());
                }

                if (!_updateDelegatesByData[targetEvent.ObjectId].ContainsKey(key1))
                {
                    _updateDelegatesByData[targetEvent.ObjectId].Add(key1, new Dictionary<TKey2, Action>());
                }

                updateDelegates = _updateDelegatesByData[targetEvent.ObjectId][key1];

                if (!_updateHandlersByData[targetEvent.ObjectId].ContainsKey(key1))
                {
                    _updateHandlersByData[targetEvent.ObjectId]
                       .Add(key1, new Dictionary<TKey2, AppaEvent.Handler>());
                }

                updateHandlers = _updateHandlersByData[targetEvent.ObjectId][key1];

                if (!updateDelegates.ContainsKey(key2))
                {
                    updateDelegates.Add(key2, delegateCreator());
                }

                updateDelegate = updateDelegates[key2];

                if (!updateHandlers.ContainsKey(key2))
                {
                    updateHandlers.Add(key2, new AppaEvent.Handler(updateDelegate));
                }

                updateHandler = updateHandlers[key2];

                targetEvent.Event += updateHandler;
            }
        }

        /// <summary>
        ///     Subscribes the delegate to the provided event if it has not been previously subscribed, using the keys provided to ensure uniqueness.
        /// </summary>
        /// <param name="key1">The first delegate key.</param>
        /// <param name="key2">The second delegate key.</param>
        /// <param name="targetEvent">The event to subscribe to (once).</param>
        /// <param name="delegateCreator">A delegate to create the update/subscribe delegate.</param>
        /// <param name="doInvoke">Should we invoke the delegate the first time we are subscribed?</param>
        /// <exception cref="ArgumentNullException">Thrown whenever any key is null.</exception>
        public void SubscribeAndInvoke(
            TKey1 key1,
            TKey2 key2,
            ref AppaEvent.Data targetEvent,
            Func<Action> delegateCreator,
            bool doInvoke = true)
        {
            using (_PRF_SubscribeAndInvoke.Auto())
            {
                Subscribe(key1, key2, ref targetEvent, delegateCreator);

                if (!doInvoke)
                {
                    return;
                }

                var updateDelegate = _updateDelegatesByData[targetEvent.ObjectId][key1][key2];
                updateDelegate();
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ReusableDelegateCollection<TKey1, TKey2>) + ".";

        private static readonly ProfilerMarker _PRF_Subscribe =
            new ProfilerMarker(_PRF_PFX + nameof(Subscribe));

        private static readonly ProfilerMarker _PRF_SubscribeAndInvoke =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeAndInvoke));

        #endregion
    }
}
