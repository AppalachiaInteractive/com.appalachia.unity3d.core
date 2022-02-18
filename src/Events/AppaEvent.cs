using System;
using Appalachia.Core.Events.Collections;
using Appalachia.Core.Events.Extensions;
using Appalachia.Utility.Standards;

namespace Appalachia.Core.Events
{
    public static class AppaEvent
    {
        public delegate void Handler();

        #region Nested type: Data

        public struct Data : IEquatable<Data>
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

            private ObjectId _objectId;

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

            public ObjectId ObjectId
            {
                get
                {
                    if ((_objectId == null) || (_objectId == ObjectId.Empty))
                    {
                        _objectId = ObjectId.NewObjectId();
                    }

                    return _objectId;
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

            public static bool operator ==(Data left, Data right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(Data left, Data right)
            {
                return !left.Equals(right);
            }

            public override bool Equals(object obj)
            {
                return obj is Data other && Equals(other);
            }

            public override int GetHashCode()
            {
                return ObjectId.GetHashCode();
            }

            public void UnsubscribeAll()
            {
                _subscribers.Clear();
            }

            #region IEquatable<Data> Members

            public bool Equals(Data other)
            {
                return Equals(_objectId, other._objectId);
            }

            #endregion
        }

        #endregion

        #region Nested type: Subscribers

        public sealed class Subscribers : EventSubscribersCollection<Handler>
        {
        }

        #endregion
    }
}
