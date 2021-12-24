namespace Appalachia.Core.Collections.Interfaces
{
    public interface IAppaLookup<TKey, TValue, TValueList> : IAppaLookupReadOnly<TKey, TValue, TValueList>,
                                                             IAppaLookupAddOnly<TKey, TValue>,
                                                             IAppaLookupRemoveOnly<TKey, TValue>,
                                                             IIndexedCollectionState<TValue>
        where TValueList : AppaList<TValue>
    {
        new TValueList at { get; set; }
        new TValue this[TKey key] { get; set; }
    }
}
