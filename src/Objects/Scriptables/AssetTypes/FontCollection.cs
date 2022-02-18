using Appalachia.Core.Collections.Implementations.Lists;
using Appalachia.Core.Collections.Implementations.Lookups;
using UnityEngine;

namespace Appalachia.Core.Objects.Scriptables.AssetTypes
{
    public sealed class FontCollection : AppalachiaObjectLookupCollection<string, Font, stringList, FontList,
        FontLookup, FontCollection>
    {
        /// <inheritdoc />
        public override bool HasDefault => false;

        /// <inheritdoc />
        protected override string GetUniqueKeyFromValue(Font value)
        {
            return value.name;
        }
    }
}
