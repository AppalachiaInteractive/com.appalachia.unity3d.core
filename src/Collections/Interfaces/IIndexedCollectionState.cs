#region

using System;
using System.Collections.Generic;

#endregion

namespace Appalachia.Core.Collections.Interfaces
{
    public interface IIndexedCollectionState<TValue>
    {
        IReadOnlyList<TValue> Values { get; }
        int LastFrameCheck { get; }
        int InitializerCount { get; set; }

        void SetDirtyAction(Action a);
    }
}
