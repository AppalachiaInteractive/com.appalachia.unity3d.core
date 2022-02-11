using Appalachia.Core.Events.Collections;
using Appalachia.Core.Events.Extensions;

namespace Appalachia.Core.Events
{
    public static class AppaEvent
    {
        public delegate void Handler();

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

            public int SubscriberCount
            {
                get
                {
                    _subscribers ??= new();
                    return _subscribers.SubscriberCountSafe();
                }
            }

            internal Subscribers Subscribers
            {
                get
                {
                    _subscribers ??= new();
                    return _subscribers;
                }
            }

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
