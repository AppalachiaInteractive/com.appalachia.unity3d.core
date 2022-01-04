#region

using System;
using System.Collections.Generic;
using System.Reflection;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Models;
using Appalachia.Utility.Async;
using Appalachia.Utility.Async.External.Addressables;
using Unity.Profiling;
using UnityEngine.AddressableAssets;

#endregion

namespace Appalachia.Core.Objects.Root
{
    [Critical]
    public sealed partial class AppalachiaRepository : AppalachiaObject
    {
        #region Fields and Autoproperties

        [NonSerialized] private Dictionary<Assembly, string> _assemblyNames;

        #endregion

        internal static async AppaTask<AppalachiaRepository> AwakeRepository()
        {
            using (_PRF_AwakeRepository.Auto())
            {
                if (instance != null)
                {
                    return instance;
                }

                StaticContext.Log.Info("Awakening.  Will attempt to load addressable repository asset.");

                var i = await Addressables.LoadAssetAsync<AppalachiaRepository>(nameof(AppalachiaRepository));

                StaticContext.Log.Info("Initializing repository.");

                await i.ExecuteInitialization();

                SetInstance(i);

                StaticContext.Log.Info("Successfully awakened.");

                return i;
            }
        }

        protected override void AfterInitialization()
        {
            using (_PRF_AfterInitialization.Auto())
            {
                _singletonLookup ??= new Dictionary<Type, AppalachiaRepositorySingletonReference>();
                _prefabLookup ??= new Dictionary<string, AppalachiaRepositoryPrefabReference>();

                PopulateSingletonLookup(_singletonLookup, _singletons);
#if UNITY_EDITOR
                PopulateSingletonLookup(_singletonLookup, _editorSingletons);
#endif
                PopulatePrefabLookup(_prefabLookup, _prefabs);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AppalachiaRepository) + ".";

        private static readonly ProfilerMarker _PRF_AfterInitialization =
            new ProfilerMarker(_PRF_PFX + nameof(AfterInitialization));

        private static readonly ProfilerMarker _PRF_AwakeRepository =
            new ProfilerMarker(_PRF_PFX + nameof(AwakeRepository));

        #endregion
    }
}
