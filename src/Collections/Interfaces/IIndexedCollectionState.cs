#region

using System;
using System.Collections.Generic;

#endregion

namespace Appalachia.Core.Collections.Interfaces
{
    public interface IIndexedCollectionState<TValue>
    {
        int LastFrameCheck { get; }
        IReadOnlyList<TValue> Values { get; }
        int InitializerCount { get; set; }

        void SetDirtyAction(Action a);
    }
}
