#region

using System.Collections.Generic;

#endregion

namespace Appalachia.Core.Collections.Interfaces
{
    public interface IAppaLookupRemoveOnly<TKey, TValue>
    {
        TValue Remove(KeyValuePair<TKey, TValue> pair);
        TValue RemoveAt(int targetIndex);
        TValue RemoveByKey(TKey key);
        void Clear();
    }
}
