#region

using System;
using System.Collections.Generic;

#endregion

namespace Appalachia.Core.Collections.Interfaces
{
    public interface IAppaLookupAddOnly<TKey, TValue>
    {
        void Add(TKey key, TValue value);
        void AddIfKeyNotPresent(TKey key, Func<TValue> value);
        void AddIfKeyNotPresent(TKey key, TValue value);
        void AddOrUpdate(KeyValuePair<TKey, TValue> pair);
        void AddOrUpdate(TKey key, Func<TValue> add, Func<TValue> update);
        void AddOrUpdate(TKey key, TValue value);
        void AddOrUpdateIf(TKey key, Func<TValue> valueRetriever, Predicate<TValue> updateIf);
        void AddOrUpdateIf(TKey key, TValue value, Predicate<TValue> updateIf);
        void AddOrUpdateRange(IList<TValue> values, Func<TValue, TKey> selector);
    }
}
