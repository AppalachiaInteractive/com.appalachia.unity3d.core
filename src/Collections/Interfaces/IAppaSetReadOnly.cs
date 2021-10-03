#region

using System;
using System.Collections.Generic;

#endregion

namespace Appalachia.Core.Collections.Interfaces
{
    public interface IAppaSetReadOnly<TValue>
    {
        IReadOnlyList<TValue> Values { get; }
        TValue this[int index] { get; }
        int Count { get; }
        TValue GetByIndex(int i);
        bool Contains(TValue value);
        int SumCounts(Func<TValue, int> counter);
        void IfPresent(TValue key, Action present, Action notPresent);
    }
}
