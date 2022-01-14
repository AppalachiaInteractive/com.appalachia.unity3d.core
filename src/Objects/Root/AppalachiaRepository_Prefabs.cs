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
        #region Constants and Static Readonly

        private const string COULD_NOT_INSTANTIATE_PREFAB_WITH_ADDRESS =
            "Could not instantiate prefab with address [{0}]";

        #endregion

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

        public async AppaTask<GameObject> InstantiatePrefab(
            string prefabAddress,
            GameObject parent,
            bool resetTransform,
            string prefabName = null)
        {
            return await InstantiatePrefab(prefabAddress, parent.transform, resetTransform, prefabName);
        }

        public async AppaTask<GameObject> InstantiatePrefab(
            string prefabAddress,
            Transform parent,
            bool resetTransform,
            string prefabName = null)
        {
            var prefabInstance = await InstantiatePrefab(prefabAddress, prefabName, parent);

            if (resetTransform)
            {
                prefabInstance.transform.SetToOrigin();
            }

            return prefabInstance;
        }

        public async AppaTask<GameObject> InstantiatePrefab(
            string prefabAddress,
            string prefabName = null,
            Transform parent = null,
            bool? worldPositionStays = null)
        {
            var prefab = await FindPrefab(prefabAddress);

#if UNITY_EDITOR
            var prefabInstance = UnityEditor.PrefabUtility.InstantiatePrefab(prefab, parent) as GameObject;
#else
                var prefabInstance = Instantiate(prefab, parent);
#endif

            if (prefabInstance == null)
            {
                Context.Log.Error(
                    ZString.Format(
                        COULD_NOT_INSTANTIATE_PREFAB_WITH_ADDRESS,
                        prefabAddress.FormatConstantForLogging()
                    )
                );

                throw new MissingReferenceException();
            }

            if (worldPositionStays.HasValue && !worldPositionStays.Value)
            {
                prefabInstance.transform.SetToOrigin();
            }

            prefabInstance.name = prefabName ?? prefabAddress;
            return prefabInstance;
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
