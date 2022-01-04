#if UNITY_EDITOR

#region

using System;
using System.Collections.Generic;
using Appalachia.CI.Integration.Assets;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Core.Objects.Collections;
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
        private const int PREFAB_BOTTOM_SORT = PREFAB_BOTTOM + 10;
        private const int PREFAB_OFFSET = 25;
        private const int PREFAB_TOP = REPO_TOP + PREFAB_OFFSET;
        private const int PREFAB_TOP_SORT = PREFAB_TOP + 10;

        #endregion

        [ButtonGroup(nameof(PREFAB_TOP),    Order = PREFAB_TOP)]
        [ButtonGroup(nameof(PREFAB_BOTTOM), Order = PREFAB_BOTTOM)]
        public void UpdatePrefabs()
        {
            using (_PRF_UpdatePrefabs.Auto())
            {
                _prefabs ??= new AppalachiaRepositoryPrefabReferenceList();
                _prefabLookup ??= new Dictionary<string, AppalachiaRepositoryPrefabReference>();

                if (AppalachiaApplication.IsPlaying)
                {
                    return;
                }

                _prefabs.Clear();
                _prefabLookup.Clear();

                var allAssetPaths = AssetDatabaseManager.GetAllAssetPaths();

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

                    if (!path.EndsWith(".prefab"))
                    {
                        continue;
                    }

                    var allObjectsAtPath = AssetDatabaseManager.LoadAllAssetsAtPath(path);

                    for (var i = 0; i < allObjectsAtPath.Length; i++)
                    {
                        var objectAtPath = allObjectsAtPath[i] as GameObject;

                        if (objectAtPath == null)
                        {
                            continue;
                        }

                        if (!objectAtPath.IsAddressable(out var addressableInfo))
                        {
                            continue;
                        }

                        if (_prefabLookup.ContainsKey(objectAtPath.name))
                        {
                            var existingInstance = _prefabLookup[objectAtPath.name];

                            Context.Log.Error("Duplicate prefab names! (Instance 1)", objectAtPath);
                            Context.Log.Error(
                                "Duplicate types! (Instance 2)",
                                existingInstance.assetReference.editorAsset
                            );
                            continue;
                        }

                        StoreNewPrefabReference(objectAtPath, addressableInfo);
                    }
                }

                Sort();
                MarkAsModified();

                AssetDatabaseManager.SaveAssets();
                AssetDatabaseManager.Refresh();
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
