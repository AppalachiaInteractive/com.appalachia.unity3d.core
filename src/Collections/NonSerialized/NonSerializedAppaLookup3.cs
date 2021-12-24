using Appalachia.Utility.Strings;
using UnityEngine;

namespace Appalachia.Core.Collections.NonSerialized
{
    public sealed class NonSerializedAppaLookup3<TKey1, TKey2, TKey3, TValue> : AppaLookup3<TKey1, TKey2,
        TKey3, TValue, NonSerializedList<TKey1>, NonSerializedList<TKey2>, NonSerializedList<TKey3>,
        NonSerializedList<TValue>, NonSerializedAppaLookup<TKey3, TValue>,
        NonSerializedAppaLookup2<TKey2, TKey3, TValue>,
        NonSerializedList<NonSerializedAppaLookup<TKey3, TValue>>,
        NonSerializedList<NonSerializedAppaLookup2<TKey2, TKey3, TValue>>>
    {
        protected override bool IsSerialized => false;

        protected override Color GetDisplayColor(
            TKey1 key,
            NonSerializedAppaLookup2<TKey2, TKey3, TValue> value)
        {
            return Color.white;
        }

        protected override string GetDisplaySubtitle(
            TKey1 key,
            NonSerializedAppaLookup2<TKey2, TKey3, TValue> value)
        {
            return ZString.Format("{0} sub-values", value.Count);
        }

        protected override string GetDisplayTitle(
            TKey1 key,
            NonSerializedAppaLookup2<TKey2, TKey3, TValue> value)
        {
            return key.ToString();
        }
    }
}
