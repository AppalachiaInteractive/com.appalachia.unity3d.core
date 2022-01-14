#region

using System;
using System.Collections.Generic;

#endregion

namespace Appalachia.Core.Collections.Interfaces
{
    public interface IAppaSetReadOnly<TValue>
    {
        int Count { get; }
        IReadOnlyList<TValue> Values { get; }
        TValue this[int index] { get; }
        bool Contains(TValue value);
        TValue GetByIndex(int i);
        void IfPresent(TValue key, Action present, Action notPresent);
        int SumCounts(Func<TValue, int> counter);
    }
}
