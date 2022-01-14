using Appalachia.Core.Collections.Implementations.Lists;
using Appalachia.Core.Collections.Implementations.Lookups;
using UnityEngine;

namespace Appalachia.Core.Objects.Scriptables.AssetTypes
{
    public class MainFontCollection : SingletonAppalachiaObjectLookupCollection<string, Font, stringList,
        FontList, FontLookup, FontCollection, MainFontCollection>
    {
        #region Menu Items

#if UNITY_EDITOR
        [UnityEditor.MenuItem(
            PKG.Menu.Assets.Base + nameof(MainFontCollection),
            priority = PKG.Menu.Assets.Priority
        )]
        public static void CreateAsset()
        {
            CreateNew<MainFontCollection>();
        }
#endif

        #endregion
    }
}
