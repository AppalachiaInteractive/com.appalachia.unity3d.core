#region

using System;
using System.Collections.Generic;
using Appalachia.Core.Objects.Collections;
using Appalachia.Core.Objects.Models;
using Appalachia.Utility.Async;
using Appalachia.Utility.Async.External.Addressables;
using Appalachia.Utility.Constants;
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
        #region Fields and Autoproperties

        [SerializeField, InlineProperty, HideLabel, Title("Prefabs"), PropertyOrder(15)]
        [Searchable(FuzzySearch = true, Recursive = true)]
        [ListDrawerSettings(
            HideAddButton = true,
            HideRemoveButton = true,
            DraggableItems = false,
            NumberOfItemsPerPage = 10
        )]
        private AppalachiaRepositoryPrefabReferenceList _prefabs;

        [NonSerialized] private Dictionary<string, AppalachiaRepositoryPrefabReference> _prefabLookup;

        #endregion

        public async AppaTask<GameObject> FindPrefab(string prefabAddress)
        {
            using (_PRF_FindPrefab.Auto())
            {
                _prefabs ??= new AppalachiaRepositoryPrefabReferenceList();

                if (!_prefabLookup.ContainsKey(prefabAddress))
                {
                    Context.Log.Error(
                        ZString.Format(
                            "Could not find prefab {0} in the {1}!",
                            prefabAddress,
                            nameof(AppalachiaRepository).FormatForLogging()
                        )
                    );
                    return null;
                }

                var reference = _prefabLookup[prefabAddress];
                var assetReference = reference.assetReference;

                var isLoaded = assetReference.IsValid();

                if (isLoaded)
                {
                    reference.prefab = assetReference.Asset as GameObject;

                    return reference.prefab;
                }

                try
                {
#if UNITY_EDITOR
                    if (!assetReference.RuntimeKeyIsValid())
                    {
                        reference.prefab = assetReference.editorAsset as GameObject;
                    }
                    else
                    {
#endif
                        reference.prefab = await assetReference.LoadAssetAsync<GameObject>();
#if UNITY_EDITOR
                    }
#endif
                }
                catch (Exception ex)
                {
                    Context.Log.Error(
                        ZString.Format(
                            "Failed to load singleton of address {0}: {1}",
                            prefabAddress.FormatConstantForLogging(),
                            ex.Message
                        ),
                        null,
                        ex
                    );

                    return null;
                }

                return reference.prefab;
            }
        }

        public async AppaTask<GameObject> InstantiatePrefab(
            string prefabAddress,
            GameObject parent,
            bool resetTransform)
        {
            using (_PRF_InstantiatePrefab.Auto())
            {
                return await InstantiatePrefab(prefabAddress, parent.transform, resetTransform);
            }
        }

        public async AppaTask<GameObject> InstantiatePrefab(
            string prefabAddress,
            Transform parent,
            bool resetTransform)
        {
            using (_PRF_InstantiatePrefab.Auto())
            {
                var prefabInstance = await InstantiatePrefab(prefabAddress);

                prefabInstance.transform.SetParent(parent);

                if (resetTransform)
                {
                    prefabInstance.transform.SetToOrigin();
                }

                return prefabInstance;
            }
        }

        public async AppaTask<GameObject> InstantiatePrefab(string prefabAddress)
        {
            using (_PRF_InstantiatePrefab.Auto())
            {
                var prefab = await FindPrefab(prefabAddress);

                return Instantiate(prefab);
            }
        }

        private static void PopulatePrefabLookup(
            Dictionary<string, AppalachiaRepositoryPrefabReference> lookup,
            AppalachiaRepositoryPrefabReferenceList list)
        {
            using (_PRF_PopulateLookup.Auto())
            {
                if (list == null)
                {
                    return;
                }

                for (var index = 0; index < list.Count; index++)
                {
                    var reference = list[index];
                    var prefabAddress = reference.PrefabAddress;

                    if (prefabAddress == null)
                    {
                        continue;
                    }

                    if (lookup.ContainsKey(prefabAddress))
                    {
                        lookup[prefabAddress] = reference;
                    }
                    else
                    {
                        lookup.Add(prefabAddress, reference);
                    }
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_InstantiatePrefab =
            new ProfilerMarker(_PRF_PFX + nameof(InstantiatePrefab));

        private static readonly ProfilerMarker _PRF_FindPrefab =
            new ProfilerMarker(_PRF_PFX + nameof(FindPrefab));

        #endregion
    }
}
