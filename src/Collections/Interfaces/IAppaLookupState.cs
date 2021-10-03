#region

using System.Collections.Generic;

#endregion

namespace Appalachia.Core.Collections.Interfaces
{
    public interface IAppaLookupState<TKey, TValue> : IIndexedCollectionState<TValue>
    {
        IReadOnlyList<TKey> Keys { get; }
        IReadOnlyDictionary<TKey, TValue> Lookup { get; }
        IReadOnlyDictionary<TKey, int> Indices { get; }
    }
}
