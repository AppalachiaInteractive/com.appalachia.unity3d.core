#region

using System;
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Strings;
using UnityEngine;

#endregion

namespace Appalachia.Core.Objects.Scriptables
{
    [Serializable]
    public abstract class EmbeddedAppalachiaObject<T> : AppalachiaObject<T>
        where T : EmbeddedAppalachiaObject<T>
    {
#if UNITY_EDITOR
        public static TC CreateAndSaveInExisting<TC>(GameObject mainAsset)
            where TC : AppalachiaObject<TC>
        {
            var assetName = ZString.Format("{0}_{1:yyyyMMdd-hhmmssfff}.asset", typeof(TC).Name, DateTime.Now);

            return CreateAndSaveInExisting<TC>(mainAsset, assetName);
        }

        public static TC CreateAndSaveInExisting<TC>(GameObject mainAsset, string assetName)
            where TC : AppalachiaObject<TC>
        {
            var path = AssetDatabaseManager.GetAssetPath(mainAsset);

            if (path == null)
            {
                return null;
            }

            return CreateAndSaveInExisting<TC>(path, assetName);
        }

        public static TC CreateAndSaveInExisting<TC>(string assetPath, string assetName)
            where TC : AppalachiaObject<TC>
        {
            var instance = (TC)CreateInstance(typeof(TC));
            instance.name = assetName;

            AssetDatabaseManager.AddObjectToAsset(instance, assetPath);

            return instance;
        }
#endif
    }
}
