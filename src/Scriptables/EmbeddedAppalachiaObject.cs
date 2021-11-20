#region

using System;
using Appalachia.CI.Integration.Assets;
using UnityEngine;

#endregion

namespace Appalachia.Core.Scriptables
{
    [Serializable]
    public abstract class EmbeddedAppalachiaObject<T> : AppalachiaObject
        where T : EmbeddedAppalachiaObject<T>
    {
#if UNITY_EDITOR
        public static TC CreateAndSaveInExisting<TC>(GameObject mainAsset)
            where TC : AppalachiaObject
        {
            var assetName = $"{typeof(TC).Name}_{DateTime.Now:yyyyMMdd-hhmmssfff}.asset";

            return CreateAndSaveInExisting<TC>(mainAsset, assetName);
        }

        public static TC CreateAndSaveInExisting<TC>(GameObject mainAsset, string assetName)
            where TC : AppalachiaObject
        {
            var path = AssetDatabaseManager.GetAssetPath(mainAsset);

            if (path == null)
            {
                return null;
            }

            return CreateAndSaveInExisting<TC>(path, assetName);
        }

        public static TC CreateAndSaveInExisting<TC>(string assetPath, string assetName)
            where TC : AppalachiaObject
        {
            var instance = (TC) CreateInstance(typeof(TC));
            instance.name = assetName;

            AssetDatabaseManager.AddObjectToAsset(instance, assetPath);

            return instance;
        }
#endif
    }
}
