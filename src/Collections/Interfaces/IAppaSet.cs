#region

using System.Collections.Generic;

#endregion

namespace Appalachia.Core.Collections.Interfaces
{
    public interface IAppaSet<TValue> : IAppaSetReadOnly<TValue>
    {
        bool Remove(TValue value);
        TValue RemoveAt(int targetIndex);
        void Add(TValue value);
        void AddRange(IList<TValue> values);
        void Clear();
    }
}
