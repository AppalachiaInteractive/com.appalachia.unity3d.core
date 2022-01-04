#region

using System;
using System.Collections.Generic;
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Objects.Collections;
using Appalachia.Core.Objects.Models;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Async;
using Appalachia.Utility.Async.External.Addressables;
using Appalachia.Utility.Constants;
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

        [SerializeField, InlineProperty, HideLabel, Title("Singletons"), PropertyOrder(5)]
        [Searchable(FuzzySearch = true, Recursive = true)]
        [ListDrawerSettings(
            HideAddButton = true,
            HideRemoveButton = true,
            DraggableItems = false,
            NumberOfItemsPerPage = 10
        )]
        private AppalachiaRepositorySingletonReferenceList _singletons;

        [NonSerialized] private Dictionary<Type, AppalachiaRepositorySingletonReference> _singletonLookup;

        #endregion

        public async AppaTask<T> Find<T>()
            where T : SingletonAppalachiaObject<T>, ISingleton<T>
        {
            using (_PRF_Find.Auto())
            {
                var result = await Find(typeof(T));

                if (result is not null)
                {
                    result.SetSingletonInstance(result);

                    return result as T;
                }

                return null;
            }
        }

        public async AppaTask<ISingleton> Find(Type t)
        {
            using (_PRF_Find.Auto())
            {
                _singletons ??= new AppalachiaRepositorySingletonReferenceList();

                if (!_singletonLookup.ContainsKey(t))
                {
                    Context.Log.Error(
                        ZString.Format(
                            "Could not find type {0} in the {1}!",
                            t,
                            nameof(AppalachiaRepository).FormatForLogging()
                        )
                    );
                    return null;
                }

                var reference = _singletonLookup[t];
                var assetReference = reference.assetReference;
                ISingleton result;

                var isLoaded = assetReference.IsValid();

                //var isReferenceValid = referenceSingleton.RuntimeKeyIsValid();

                if (isLoaded)
                {
                    result = assetReference.Asset as ISingleton;

                    result?.SetSingletonInstance(result);

                    return result;
                }

                try
                {
#if UNITY_EDITOR
                    if (!assetReference.RuntimeKeyIsValid())
                    {
                        if (assetReference.editorAsset == null)
                        {
                            var referenceType = reference.GetReferenceType();
                            result = AssetDatabaseManager.FindFirstAssetMatch(referenceType) as ISingleton;
                        }
                        else
                        {
                            result = assetReference.editorAsset as ISingleton;
                        }
                    }
                    else
                    {
#endif
                        result = await assetReference.LoadAssetAsync<ISingleton>();
#if UNITY_EDITOR
                    }
#endif
                }
                catch (Exception ex)
                {
                    Context.Log.Error(
                        ZString.Format(
                            "Failed to load singleton of type {0}: {1}",
                            t.FormatForLogging(),
                            ex.Message
                        ),
                        null,
                        ex
                    );

                    return null;
                }

                result?.SetSingletonInstance(result);

                return result;
            }
        }

        public bool IsTypeInRepository<T>()
        {
            using (_PRF_IsTypeInRepository.Auto())
            {
                return IsTypeInRepository(typeof(T));
            }
        }

        public bool IsTypeInRepository(Type t)
        {
            using (_PRF_IsTypeInRepository.Auto())
            {
                return _singletonLookup.ContainsKey(t);
            }
        }

        private static void PopulateSingletonLookup(
            Dictionary<Type, AppalachiaRepositorySingletonReference> lookup,
            AppalachiaRepositorySingletonReferenceList list)
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
                    var referenceType = reference.GetReferenceType();

                    if (referenceType == null)
                    {
                        continue;
                    }

                    if (lookup.ContainsKey(referenceType))
                    {
                        lookup[referenceType] = reference;
                    }
                    else
                    {
                        lookup.Add(referenceType, reference);
                    }
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_Find = new ProfilerMarker(_PRF_PFX + nameof(Find));

        private static readonly ProfilerMarker _PRF_PopulateLookup =
            new ProfilerMarker(_PRF_PFX + nameof(PopulateSingletonLookup));

        private static readonly ProfilerMarker _PRF_IsTypeInRepository =
            new ProfilerMarker(_PRF_PFX + nameof(IsTypeInRepository));

        #endregion
    }
}
