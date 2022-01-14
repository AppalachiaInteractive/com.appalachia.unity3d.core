#region

using System.Collections.Generic;

#endregion

namespace Appalachia.Core.Collections.Interfaces
{
    public interface IAppaLookupRemoveOnly<TKey, TValue>
    {
        void Clear();
        TValue Remove(KeyValuePair<TKey, TValue> pair);
        TValue RemoveAt(int targetIndex);
        TValue RemoveByKey(TKey key);
    }
}
