using UnityEditor;
using UnityEngine;

namespace Appalachia.Core.Assets
{
    public static partial class AssetDatabaseManager
    {
        public static GameObject GetPrefabAsset(GameObject prefabInstance)
        {
            var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefabInstance);

            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            return AssetDatabase.LoadAssetAtPath<GameObject>(path);
        }
    }
}
