#region

using System;
using System.Collections.Generic;

#endregion

namespace Appalachia.Core.Collections.Interfaces
{
    public interface IAppaLookupReadOnly<TKey, TValue, TValueList>
        where TValueList : AppaList<TValue>
    {
        int Count { get; }
        IReadOnlyDictionary<TKey, TValue> Lookup { get; }
        TValueList at { get; set; }
        TValue this[TKey key] { get; }
        bool ContainsKey(TKey key);
        bool TryGet(TKey key, out TValue value);
        int SumCounts(Func<TValue, int> counter);
        TKey GetKeyByIndex(int i);
        TValue Get(TKey key);
        TValue GetByIndex(int i);
        void IfPresent(TKey key, Action present, Action notPresent);
    }
}
