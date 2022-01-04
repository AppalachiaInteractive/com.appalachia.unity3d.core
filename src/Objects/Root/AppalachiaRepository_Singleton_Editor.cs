#if UNITY_EDITOR

#region

using System;
using System.Collections.Generic;
using System.Linq;
using Appalachia.CI.Integration.Assets;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Core.Objects.Collections;
using Appalachia.Core.Objects.Models;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Reflection.Extensions;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Core.Objects.Root
{
    public sealed partial class AppalachiaRepository
    {
        private const int SINGLETON_OFFSET = 50;
        private const int SINGLETON_BOTTOM = REPO_BOTTOM + SINGLETON_OFFSET;
        private const int SINGLETON_TOP = REPO_TOP + SINGLETON_OFFSET;
        private const int SINGLETON_BOTTOM_SORT = SINGLETON_BOTTOM + 10;
        private const int SINGLETON_TOP_SORT = SINGLETON_TOP + 10;

        #region Fields and Autoproperties

        [SerializeField, Title("Editor Singletons"), PropertyOrder(10)]
        [ListDrawerSettings(
            HideAddButton = true,
            HideRemoveButton = true,
            DraggableItems = false,
            NumberOfItemsPerPage = 10
        )]
        private AppalachiaRepositorySingletonReferenceList _editorSingletons;

        #endregion

        [ButtonGroup(nameof(SINGLETON_TOP),    Order = SINGLETON_TOP)]
        [ButtonGroup(nameof(SINGLETON_BOTTOM), Order = SINGLETON_BOTTOM)]
        public void UpdateSingletons()
        {
            using (_PRF_UpdateSingletons.Auto())
            {
                _singletons ??= new AppalachiaRepositorySingletonReferenceList();
                _editorSingletons ??= new AppalachiaRepositorySingletonReferenceList();
                _singletonLookup ??= new Dictionary<Type, AppalachiaRepositorySingletonReference>();

                if (AppalachiaApplication.IsPlaying)
                {
                    return;
                }

                _singletons.Clear();
                _editorSingletons.Clear();
                _singletonLookup.Clear();

                var baseSingletonObjectType = typeof(SingletonAppalachiaObject<>);

                var allAssetPaths = AssetDatabaseManager.GetAllAssetPaths();

                var runtimeTypes = AssetDatabaseManager.GetAllRuntimeMonoScripts()
                                                       .Select(ms => ms.GetClass())
                                                       .ToHashSet();

                foreach (var path in allAssetPaths)
                {
                    if (!AppaFile.Exists(path))
                    {
                        continue;
                    }

                    if (AppaDirectory.Exists(path))
                    {
                        continue;
                    }

                    if (!path.EndsWith(".asset"))
                    {
                        continue;
                    }

                    var allObjectsAtPath = AssetDatabaseManager.LoadAllAssetsAtPath(path);

                    for (var i = 0; i < allObjectsAtPath.Length; i++)
                    {
                        var objectAtPath = allObjectsAtPath[i] as ScriptableObject;

                        if (objectAtPath == null)
                        {
                            continue;
                        }

                        var typeAtPath = objectAtPath.GetType();

                        if (!typeAtPath.InheritsFrom(baseSingletonObjectType))
                        {
                            continue;
                        }

                        if (objectAtPath.name != typeAtPath.Name)
                        {
                            objectAtPath.name = typeAtPath.Name;

                            var directory = AppaPath.GetDirectoryName(path);
                            var extension = AppaPath.GetExtension(path);

                            var newName = AppaPath.Combine(
                                directory,
                                ZString.Format("{0}{1}", typeAtPath.Name, extension)
                            );

                            AppaFile.Move(path, newName);
                        }

                        if (_singletonLookup.ContainsKey(typeAtPath))
                        {
                            var existingInstance = _singletonLookup[typeAtPath];

                            Context.Log.Error("Duplicate types! (Instance 1)", objectAtPath);
                            Context.Log.Error(
                                "Duplicate types! (Instance 2)",
                                existingInstance.assetReference
                            );
                            continue;
                        }

                        StoreNewSingletonReference(objectAtPath, typeAtPath, runtimeTypes);
                    }
                }

                var concreteSingletonObjectTypes = baseSingletonObjectType.GetAllConcreteInheritors();

                for (var index = 0; index < concreteSingletonObjectTypes.Count; index++)
                {
                    var concreteSingletonObjectType = concreteSingletonObjectTypes[index];

                    if (concreteSingletonObjectType.IsAbstract)
                    {
                        continue;
                    }

                    if (concreteSingletonObjectType == typeof(AppalachiaRepository))
                    {
                        continue;
                    }

                    if (_singletonLookup.ContainsKey(concreteSingletonObjectType))
                    {
                        continue;
                    }

                    var assetInstance =
                        AssetDatabaseManager.FindFirstAssetMatch(concreteSingletonObjectType) as
                            ScriptableObject;

                    if (assetInstance == null)
                    {
                        var newInstance = CreateInstance(concreteSingletonObjectType);
                        assetInstance = CreateNew(
                            concreteSingletonObjectType,
                            concreteSingletonObjectType.Name,
                            newInstance,
                            DirectoryPath
                        );
                    }

                    StoreNewSingletonReference(assetInstance, concreteSingletonObjectType, runtimeTypes);
                }

                Sort();
                MarkAsModified();

                AssetDatabaseManager.SaveAssets();
                AssetDatabaseManager.Refresh();
            }
        }

        [ButtonGroup(nameof(SINGLETON_TOP),    Order = SINGLETON_TOP_SORT)]
        [ButtonGroup(nameof(SINGLETON_BOTTOM), Order = SINGLETON_BOTTOM_SORT)]
        private void SortSingletons()
        {
            using (_PRF_SortSingletons.Auto())
            {
                _singletons.Sort(
                    (s1, s2) => string.Compare(s1.TypeName, s2.TypeName, StringComparison.Ordinal)
                );
                _prefabs.Sort(
                    (s1, s2) => string.Compare(s1.PrefabAddress, s2.PrefabAddress, StringComparison.Ordinal)
                );
                MarkAsModified();
            }
        }

        private void StoreNewSingletonReference(
            ScriptableObject objectAtPath,
            Type typeAtPath,
            HashSet<Type> runtimeTypeLookup)
        {
            using (_PRF_StoreNewSingletonReference.Auto())
            {
                if ((objectAtPath == null) || objectAtPath is not ISingleton singleton)
                {
                    return;
                }

                AppalachiaRepositorySingletonReference repositorySingletonAssetReference;

                if (runtimeTypeLookup.Contains(typeAtPath) &&
                    objectAtPath.EnsureIsAddressable(out var addressableGuid))
                {
                    repositorySingletonAssetReference = new AppalachiaRepositorySingletonReference(
                        addressableGuid,
                        typeAtPath,
                        singleton
                    );

                    _singletons.Add(repositorySingletonAssetReference);
                }
                else
                {
                    repositorySingletonAssetReference =
                        new AppalachiaRepositorySingletonReference(null, typeAtPath, singleton);

                    _editorSingletons.Add(repositorySingletonAssetReference);
                }

                _singletonLookup.Add(typeAtPath, repositorySingletonAssetReference);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_SortSingletons =
            new ProfilerMarker(_PRF_PFX + nameof(SortSingletons));

        private static readonly ProfilerMarker _PRF_UpdateSingletons =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateSingletons));

        private static readonly ProfilerMarker _PRF_StoreNewSingletonReference =
            new ProfilerMarker(_PRF_PFX + nameof(StoreNewSingletonReference));

        #endregion
    }
}

#endif
