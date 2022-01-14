#if UNITY_EDITOR

#region

using Appalachia.CI.Integration.Assets;
using Appalachia.Utility.Async;
using Appalachia.Utility.Strings;
using Unity.Profiling;

#endregion

namespace Appalachia.Core.Objects.Root
{
    public abstract partial class SingletonAppalachiaObject<T>
    {
        #region Constants and Static Readonly

        private static readonly string _PRF_PFX2 = typeof(T).Name + ".";

        #endregion

        private static async AppaTask<T> CreateAndSaveSingleton()
        {
            using (_PRF_CreateAndSaveSingleton.Auto())
            {
                return await CreateAndSaveSingleton(ZString.Format("{0}.asset", typeof(T).Name));
            }
        }

        private static async AppaTask<T> CreateAndSaveSingleton(string name)
        {
            using (_PRF_CreateAndSaveSingleton.Auto())
            {
                if (instance != null)
                {
                    return instance;
                }

                var newInstance = await FindInstanceInSingletonLookup();

                if (newInstance != null)
                {
                    SetInstance(newInstance);

                    return newInstance;
                }

                newInstance = CreateInstance(typeof(T)) as T;

                var assetInstance = CreateNew(name, newInstance);

                AssetDatabaseManager.SaveAssets();
                AssetDatabaseManager.Refresh();

                SetInstance(assetInstance);

                return assetInstance;
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_CreateAndSaveSingleton =
            new(_PRF_PFX2 + nameof(CreateAndSaveSingleton));

        #endregion
    }
}

#endif
