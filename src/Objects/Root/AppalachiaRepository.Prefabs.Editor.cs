#if UNITY_EDITOR

#region

using System;
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Objects.Models;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Core.Objects.Root
{
    public sealed partial class AppalachiaRepository
    {
        #region Constants and Static Readonly

        private const int PREFAB_BOTTOM = REPO_BOTTOM + PREFAB_OFFSET;
        private const int PREFAB_BOTTOM_CLEAR = PREFAB_BOTTOM + 20;
        private const int PREFAB_BOTTOM_SORT = PREFAB_BOTTOM + 10;
        private const int PREFAB_OFFSET = 25;
        private const int PREFAB_TOP = REPO_TOP + PREFAB_OFFSET;
        private const int PREFAB_TOP_CLEAR = PREFAB_TOP + 20;
        private const int PREFAB_TOP_SORT = PREFAB_TOP + 10;

        #endregion

        [ButtonGroup(nameof(PREFAB_TOP),    Order = PREFAB_TOP)]
        [ButtonGroup(nameof(PREFAB_BOTTOM), Order = PREFAB_BOTTOM)]
        public void UpdatePrefabs()
        {
            const string prefabExtension = "prefab";
            using (_PRF_UpdatePrefabs.Auto())
            {
                InitializePrefabs();

                if (AppalachiaApplication.IsPlaying)
                {
                    return;
                }

                var allAssetPaths = AssetDatabaseManager.GetAllAssetPaths();
                var addedAny = false;

                foreach (var path in allAssetPaths)
                {
                    if (!path.HasExtension(prefabExtension))
                    {
                        continue;
                    }

                    if (path.FileDoesNotExist)
                    {
                        continue;
                    }

                    if (path.DirectoryExists)
                    {
                        continue;
                    }

                    var objectAtPath =
                        AssetDatabaseManager.LoadMainAssetAtPath(path.relativePath) as GameObject;

                    if (objectAtPath == null)
                    {
                        continue;
                    }

                    if (_prefabLookup.ContainsKey(objectAtPath.name))
                    {
                        var existingInstance = _prefabLookup[objectAtPath.name];

                        if (existingInstance.assetReference.editorAsset != objectAtPath)
                        {
                            Context.Log.Error("Duplicate prefab names! (Instance 1)", objectAtPath);
                            Context.Log.Error(
                                "Duplicate types! (Instance 2)",
                                existingInstance.assetReference.editorAsset
                            );
                        }

                        continue;
                    }

                    if (!objectAtPath.IsAddressable(out var addressableInfo))
                    {
                        continue;
                    }

                    addedAny = true;
                    StoreNewPrefabReference(objectAtPath, addressableInfo);
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

        [ButtonGroup(nameof(PREFAB_TOP),    Order = PREFAB_TOP_CLEAR)]
        [ButtonGroup(nameof(PREFAB_BOTTOM), Order = PREFAB_BOTTOM_CLEAR)]
        private void ClearPrefabs()
        {
            using (_PRF_ClearPrefabs.Auto())
            {
                _prefabs.Clear();
                MarkAsModified();
            }
        }

        [ButtonGroup(nameof(PREFAB_TOP),    Order = PREFAB_TOP_SORT)]
        [ButtonGroup(nameof(PREFAB_BOTTOM), Order = PREFAB_BOTTOM_SORT)]
        private void SortPrefabs()
        {
            using (_PRF_SortPrefabs.Auto())
            {
                _prefabs.Sort(
                    (s1, s2) => string.Compare(s1.PrefabAddress, s2.PrefabAddress, StringComparison.Ordinal)
                );
                MarkAsModified();
            }
        }

        private void StoreNewPrefabReference(
            GameObject prefabAtPath,
            AddressableExtensions.TargetInfo targetInfo)
        {
            using (_PRF_StoreNewPrefabReference.Auto())
            {
                if (prefabAtPath == null)
                {
                    return;
                }

                AppalachiaRepositoryPrefabReference repositoryPrefabReference;

                if (prefabAtPath.EnsurePrefabIsAddressable(out var addressableGuid))
                {
                    repositoryPrefabReference = new AppalachiaRepositoryPrefabReference(
                        addressableGuid,
                        targetInfo.Address
                    );

                    _prefabs.Add(repositoryPrefabReference);
                    _prefabLookup.Add(targetInfo.Address, repositoryPrefabReference);
                }
                else
                {
                    Context.Log.Error(
                        ZString.Format(
                            "Could not add non-addressable prefab {0} to {1}.",
                            prefabAtPath.name.FormatNameForLogging(),
                            nameof(AppalachiaRepository).FormatForLogging()
                        )
                    );
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_ClearPrefabs =
            new ProfilerMarker(_PRF_PFX + nameof(ClearPrefabs));

        private static readonly ProfilerMarker _PRF_UpdatePrefabs =
            new ProfilerMarker(_PRF_PFX + nameof(UpdatePrefabs));

        private static readonly ProfilerMarker _PRF_SortPrefabs =
            new ProfilerMarker(_PRF_PFX + nameof(SortPrefabs));

        private static readonly ProfilerMarker _PRF_StoreNewPrefabReference =
            new ProfilerMarker(_PRF_PFX + nameof(StoreNewPrefabReference));

        #endregion
    }
}

#endif
