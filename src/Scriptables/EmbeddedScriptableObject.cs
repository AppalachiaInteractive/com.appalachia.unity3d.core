#region

using System;
using Appalachia.CI.Integration.Assets;
using UnityEngine;

#endregion

#if UNITY_EDITOR

#endif

namespace Appalachia.Core.Scriptables
{
    [Serializable]
    public abstract class EmbeddedScriptableObject<T> : AppalachiaScriptableObject<T>
        where T : EmbeddedScriptableObject<T>
    {
#if UNITY_EDITOR
        public static TC CreateAndSaveInExisting<TC>(GameObject mainAsset)
            where TC : AppalachiaScriptableObject<TC>
        {
            var assetName = $"{typeof(TC).Name}_{DateTime.Now:yyyyMMdd-hhmmssfff}.asset";

            return CreateAndSaveInExisting<TC>(mainAsset, assetName);
        }

        public static TC CreateAndSaveInExisting<TC>(GameObject mainAsset, string assetName)
            where TC : AppalachiaScriptableObject<TC>
        {
            var path = AssetDatabaseManager.GetAssetPath(mainAsset);

            if (path == null)
            {
                return null;
            }

            return CreateAndSaveInExisting<TC>(path, assetName);
        }

        public static TC CreateAndSaveInExisting<TC>(string assetPath, string assetName)
            where TC : AppalachiaScriptableObject<TC>
        {
            var instance = (TC) CreateInstance(typeof(TC));
            instance.name = assetName;

            AssetDatabaseManager.AddObjectToAsset(instance, assetPath);

            return instance;
        }
#endif
    }
}
