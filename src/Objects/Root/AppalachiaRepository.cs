#region

using System;
using System.Collections.Generic;
using System.Reflection;
using Appalachia.Core.Objects.Collections;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Models;
using Appalachia.Utility.Async;
using Appalachia.Utility.Async.External.Addressables;
using Appalachia.Utility.Execution;
using Unity.Profiling;
using UnityEngine.AddressableAssets;

#endregion

namespace Appalachia.Core.Objects.Root
{
    public sealed partial class AppalachiaRepository : AppalachiaObject
    {
        #region Static Fields and Autoproperties

        private static InitializationStage _initializationStage;
        private static object _repositoryAwakeLock;

        #endregion

        #region Fields and Autoproperties

        [NonSerialized] private Dictionary<Assembly, string> _assemblyNames;

        #endregion

        internal static async AppaTask<AppalachiaRepository> AwakeRepository()
        {
            if (instance != null)
            {
                return instance;
            }

            _repositoryAwakeLock ??= new object();

            bool shouldAwake;
            bool shouldWait;

            lock (_repositoryAwakeLock)
            {
                shouldAwake = _initializationStage == InitializationStage.None;
                shouldWait = _initializationStage == InitializationStage.InProgress;

                if (shouldAwake)
                {
                    _initializationStage = InitializationStage.InProgress;
                }
            }

            if (shouldWait)
            {
                await AppaTask.WaitUntil(() => _initializationStage == InitializationStage.Completed);
                return instance;
            }

            if (!shouldAwake)
            {
                return instance;
            }

            StaticContext.Log.Info("Awakening.  Will attempt to load addressable repository asset.");

            var i = await Addressables.LoadAssetAsync<AppalachiaRepository>(nameof(AppalachiaRepository));

            StaticContext.Log.Info("Initializing repository.");

            await i.ExecuteInitialization();

            i.InitializeLookups();

            SetInstance(i);

            StaticContext.Log.Info("Successfully awakened.");

            _initializationStage = InitializationStage.Completed;

            return i;
        }

        private void InitializeLookups()
        {
            using (_PRF_InitializeLookups.Auto())
            {
                _assemblyNames ??= new Dictionary<Assembly, string>();

                InitializeSingletons();
                InitializePrefabs();
            }
        }

        private void InitializePrefabs()
        {
            using (_PRF_InitializeLookups.Auto())
            {
                _prefabs ??= new AppalachiaRepositoryPrefabReferenceList();
                _prefabLookup ??= new Dictionary<string, AppalachiaRepositoryPrefabReference>();
                _prefabLookup.Clear();

                PopulatePrefabLookup(_prefabLookup, _prefabs);
            }
        }

        private void InitializeSingletons()
        {
            using (_PRF_InitializeLookups.Auto())
            {
                _singletons ??= new AppalachiaRepositorySingletonReferenceList();
#if UNITY_EDITOR
                _editorSingletons ??= new AppalachiaRepositorySingletonReferenceList();
#endif

                _singletonLookup ??= new Dictionary<Type, AppalachiaRepositorySingletonReference>();
                _singletonLookup.Clear();

                PopulateSingletonLookup(_singletonLookup, _singletons);
#if UNITY_EDITOR
                PopulateSingletonLookup(_singletonLookup, _editorSingletons);
#endif
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AppalachiaRepository) + ".";

        private static readonly ProfilerMarker _PRF_InitializeLookups =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeLookups));

        private static readonly ProfilerMarker _PRF_AfterInitialization =
            new ProfilerMarker(_PRF_PFX + nameof(AfterInitialization));

        private static readonly ProfilerMarker _PRF_AwakeRepository =
            new ProfilerMarker(_PRF_PFX + nameof(AwakeRepository));

        #endregion
    }
}
