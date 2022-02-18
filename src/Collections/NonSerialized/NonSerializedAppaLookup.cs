#region

using System;
using Appalachia.Core.Attributes;
using UnityEngine;

#endregion

namespace Appalachia.Core.Collections.NonSerialized
{
    [NonSerializable]
    public sealed class NonSerializedAppaLookup<TKey, TValue> : AppaLookup<TKey, TValue,
        NonSerializedList<TKey>, NonSerializedList<TValue>>
    {
        /// <inheritdoc />
        protected override bool IsSerialized => false;

        /// <inheritdoc />
        protected override Color GetDisplayColor(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override string GetDisplaySubtitle(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override string GetDisplayTitle(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }
    }
}
