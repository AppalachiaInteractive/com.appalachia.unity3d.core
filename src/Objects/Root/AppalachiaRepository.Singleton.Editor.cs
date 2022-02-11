#if UNITY_EDITOR

#region

using System;
using System.Collections.Generic;
using System.Linq;
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Objects.Collections;
using Appalachia.Core.Objects.Models;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Reflection.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Core.Objects.Root
{
    public sealed partial class AppalachiaRepository
    {
        #region Constants and Static Readonly

        private const int SINGLETON_BOTTOM = REPO_BOTTOM + SINGLETON_OFFSET;
        private const int SINGLETON_BOTTOM_CLEAR = SINGLETON_BOTTOM + 20;
        private const int SINGLETON_BOTTOM_SORT = SINGLETON_BOTTOM + 10;
        private const int SINGLETON_OFFSET = 50;
        private const int SINGLETON_TOP = REPO_TOP + SINGLETON_OFFSET;
        private const int SINGLETON_TOP_CLEAR = SINGLETON_TOP + 20;
        private const int SINGLETON_TOP_SORT = SINGLETON_TOP + 10;

        #endregion

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
            const string assetExtension = "asset";

            using (_PRF_UpdateSingletons.Auto())
            {
                InitializeSingletons();

                for (var index = _singletons.Count - 1; index >= 0; index--)
                {
                    var singleton = _singletons[index];
                    if (singleton.assetReference.AssetGUID.IsNullOrWhiteSpace())
                    {
                        _singletons.RemoveAt(index);
                    }
                }

                for (var index = _editorSingletons.Count - 1; index >= 0; index--)
                {
                    var singleton = _editorSingletons[index];

                    if (singleton.editorAsset == null)
                    {
                        _editorSingletons.RemoveAt(index);
                    }
                }

                if (AppalachiaApplication.IsPlaying)
                {
                    return;
                }

                var baseSingletonObjectType = typeof(SingletonAppalachiaObject<>);

                var allAssetPaths = AssetDatabaseManager.GetAllAssetPaths();

                var runtimeTypes = AssetDatabaseManager.GetAllRuntimeMonoScripts()
                                                       .Select(ms => ms.GetClass())
                                                       .ToHashSet();

                var addedAny = false;

                foreach (var path in allAssetPaths)
                {
                    if (!path.HasExtension(assetExtension))
                    {
                        continue;
                    }

                    if (path.FileDoesNotExist)
                    {
                        continue;
                    }

                    var objectAtPath =
                        AssetDatabaseManager.LoadMainAssetAtPath(path.relativePath) as ScriptableObject;

                    if (objectAtPath == null)
                    {
                        continue;
                    }

                    var typeAtPath = objectAtPath.GetType();

                    if (!typeAtPath.InheritsFrom(baseSingletonObjectType))
                    {
                        continue;
                    }

                    if (_singletonLookup.ContainsKey(typeAtPath))
                    {
                        var existingInstance = _singletonLookup[typeAtPath];

                        if ((existingInstance.assetReference.editorAsset == null) &&
                            (existingInstance.TypeName == objectAtPath.name))
                        {
                            continue; // editor only reference
                        }

                        if (existingInstance.assetReference.editorAsset == objectAtPath)
                        {
                            continue;
                        }

                        Context.Log.Error("Duplicate types! (Instance 1)", objectAtPath);
                        Context.Log.Error("Duplicate types! (Instance 2)", existingInstance.assetReference);

                        continue;
                    }

                    if (objectAtPath.name != typeAtPath.Name)
                    {
                        objectAtPath.name = typeAtPath.Name;

                        AssetDatabaseManager.UpdateAssetName(path.relativePath, typeAtPath.Name);
                        /*
                         var directory = AppaPath.GetDirectoryName(path.relativePath);
                        var extension = path.extension;

                        var newName = AppaPath.Combine(
                            directory,
                            ZString.Format("{0}{1}", typeAtPath.Name, extension)
                        );

                        AppaFile.Move(path.relativePath, newName);*/
                    }

                    addedAny = true;
                    StoreNewSingletonReference(objectAtPath, typeAtPath, runtimeTypes);
                }

                var concreteSingletonObjectTypes = baseSingletonObjectType.GetAllConcreteInheritors();

                for (var index = 0; index < concreteSingletonObjectTypes.Count; index++)
                {
                    var concreteSingletonObjectType = concreteSingletonObjectTypes[index];

                    if (_singletonLookup.ContainsKey(concreteSingletonObjectType))
                    {
                        continue;
                    }

                    if (concreteSingletonObjectType.IsAbstract)
                    {
                        continue;
                    }

                    if (concreteSingletonObjectType == typeof(AppalachiaRepository))
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

                    addedAny = true;
                    StoreNewSingletonReference(assetInstance, concreteSingletonObjectType, runtimeTypes);
                }

                if (addedAny)
                {
                    Sort();
                    MarkAsModified();

                    AssetDatabaseManager.SaveAssets();
                    AssetDatabaseManager.Refresh();
                }
            }
        }

        [ButtonGroup(nameof(SINGLETON_TOP),    Order = SINGLETON_TOP_CLEAR)]
        [ButtonGroup(nameof(SINGLETON_BOTTOM), Order = SINGLETON_BOTTOM_CLEAR)]
        private void ClearSingletons()
        {
            using (_PRF_ClearSingletons.Auto())
            {
                _singletons.Clear();
                _editorSingletons.Clear();

                MarkAsModified();
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
                _editorSingletons.Sort(
                    (s1, s2) => string.Compare(s1.TypeName, s2.TypeName, StringComparison.Ordinal)
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

                if (_singletonLookup.ContainsKey(typeAtPath))
                {
                    _singletonLookup[typeAtPath] = repositorySingletonAssetReference;
                }
                else
                {
                    _singletonLookup.Add(typeAtPath, repositorySingletonAssetReference);
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_ClearSingletons =
            new ProfilerMarker(_PRF_PFX + nameof(ClearSingletons));

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
