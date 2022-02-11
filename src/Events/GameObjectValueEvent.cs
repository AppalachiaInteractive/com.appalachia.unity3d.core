using Appalachia.Core.Events.Base;
using Appalachia.Core.Events.Collections;
using Appalachia.Core.Events.Extensions;
using UnityEngine;

namespace Appalachia.Core.Events
{
    public static class GameObjectValueEvent<T>
    {
        public delegate void Handler(Args args);

        #region Nested type: Args

        public sealed class Args : GameObjectValueBaseArgs<Args, T>
        {
            public static implicit operator GameObject(Args o)
            {
                return o.gameObject;
            }

            public static implicit operator T(Args o)
            {
                return o.value;
            }
        }

        #endregion

        #region Nested type: Data

        public struct Data
        {
            public event Handler Event
            {
                add
                {
                    _subscribers ??= new();
                    _subscribers.Add(value);
                }
                remove
                {
                    _subscribers ??= new();
                    _subscribers.Remove(value);
                }
            }

            #region Fields and Autoproperties

            private Subscribers _subscribers;

            #endregion

            public int SubscriberCount => _subscribers.SubscriberCountSafe();

            internal Subscribers Subscribers => _subscribers;

            public void UnsubscribeAll()
            {
                _subscribers.Clear();
            }
        }

        #endregion

        #region Nested type: Subscribers

        public sealed class Subscribers : EventSubscribersCollection<Handler>
        {
        }

        #endregion
    }
}
