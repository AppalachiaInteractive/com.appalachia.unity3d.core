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

#if UNITY_EDITOR
        void SetSerializationOwner(UnityEngine.Object owner);
        void SetSerializationOwner(UnityEngine.Object owner, Action markAsModifiedAction);
#endif
    }
}
