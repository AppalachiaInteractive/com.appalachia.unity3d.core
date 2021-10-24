#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Collections.NonSerialized
{
    public sealed class NonSerializedAppaLookup<TKey, TValue> : AppaLookup<TKey, TValue,
        NonSerializedList<TKey>, NonSerializedList<TValue>>
    {
        protected override Color GetDisplayColor(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        protected override string GetDisplaySubtitle(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        protected override string GetDisplayTitle(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }
    }
}
