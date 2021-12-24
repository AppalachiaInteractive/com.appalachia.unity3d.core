using Appalachia.Core.Collections.Implementations.Lists;
using Appalachia.Core.Collections.Implementations.Lookups;
using UnityEngine;

namespace Appalachia.Core.Objects.Scriptables.AssetTypes
{
    public class MainFontCollection : SingletonAppalachiaObjectLookupCollection<string, Font, stringList,
        FontList, FontLookup, FontCollection, MainFontCollection>
    {
    }
}
