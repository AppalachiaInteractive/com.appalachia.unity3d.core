#region

using System.Collections.Generic;

#endregion

namespace Appalachia.Core.Collections.Interfaces
{
    public interface IAppaLookupState<TKey, TValue> : IIndexedCollectionState<TValue>
    {
        IReadOnlyDictionary<TKey, int> Indices { get; }
        IReadOnlyDictionary<TKey, TValue> Lookup { get; }
        IReadOnlyList<TKey> Keys { get; }
    }
}
