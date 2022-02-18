using Appalachia.Core.Attributes;
using Appalachia.Utility.Strings;
using UnityEngine;

namespace Appalachia.Core.Collections.NonSerialized
{
    [NonSerializable]
    public sealed class NonSerializedAppaLookup4<TKey1, TKey2, TKey3, TKey4, TValue> : AppaLookup4<TKey1,
        TKey2, TKey3, TKey4, TValue, NonSerializedList<TKey1>, NonSerializedList<TKey2>,
        NonSerializedList<TKey3>, NonSerializedList<TKey4>, NonSerializedList<TValue>,
        NonSerializedAppaLookup<TKey4, TValue>, NonSerializedAppaLookup2<TKey3, TKey4, TValue>,
        NonSerializedAppaLookup3<TKey2, TKey3, TKey4, TValue>,
        NonSerializedList<NonSerializedAppaLookup<TKey4, TValue>>,
        NonSerializedList<NonSerializedAppaLookup2<TKey3, TKey4, TValue>>, NonSerializedList<
            NonSerializedAppaLookup3<TKey2, TKey3, TKey4, TValue>>>

    {
        /// <inheritdoc />
        protected override bool IsSerialized => false;

        /// <inheritdoc />
        protected override Color GetDisplayColor(
            TKey1 key,
            NonSerializedAppaLookup3<TKey2, TKey3, TKey4, TValue> value)
        {
            return Color.white;
        }

        /// <inheritdoc />
        protected override string GetDisplaySubtitle(
            TKey1 key,
            NonSerializedAppaLookup3<TKey2, TKey3, TKey4, TValue> value)
        {
            return ZString.Format("{0} sub-values", value.Count);
        }

        /// <inheritdoc />
        protected override string GetDisplayTitle(
            TKey1 key,
            NonSerializedAppaLookup3<TKey2, TKey3, TKey4, TValue> value)
        {
            return key.ToString();
        }
    }
}
