using Appalachia.Core.Attributes;
using Appalachia.Utility.Strings;
using UnityEngine;

namespace Appalachia.Core.Collections.NonSerialized
{
    [NonSerializable]
    public sealed class NonSerializedAppaLookup2<TKey1, TKey2, TValue> : AppaLookup2<TKey1, TKey2, TValue,
        NonSerializedList<TKey1>, NonSerializedList<TKey2>, NonSerializedList<TValue>,
        NonSerializedAppaLookup<TKey2, TValue>, NonSerializedList<NonSerializedAppaLookup<TKey2, TValue>>>
    {
        /// <inheritdoc />
        protected override bool IsSerialized => false;

        /// <inheritdoc />
        protected override Color GetDisplayColor(TKey1 key, NonSerializedAppaLookup<TKey2, TValue> value)
        {
            return Color.white;
        }

        /// <inheritdoc />
        protected override string GetDisplaySubtitle(TKey1 key, NonSerializedAppaLookup<TKey2, TValue> value)
        {
            return ZString.Format("{0} sub-values", value.Count);
        }

        /// <inheritdoc />
        protected override string GetDisplayTitle(TKey1 key, NonSerializedAppaLookup<TKey2, TValue> value)
        {
            return key.ToString();
        }
    }
}
