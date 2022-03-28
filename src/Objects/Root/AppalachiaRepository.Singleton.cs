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
using Sirenix.Utilities;
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
            var result = await Find(typeof(T));

            if (result is not null)
            {
                return result as T;
            }

            return null;
        }

        internal async AppaTask<ISingleton> Find(Type t)
        {
            if (t.IsAbstract)
            {
                var errorMessage = ZString.Format(
                    "The type {0} is abstract, and cannot be found in the {1}!",
                    t,
                    nameof(AppalachiaRepository).FormatForLogging()
                );

                var context = AssetDatabaseManager.GetMonoScriptFromType(t);
                Context.Log.Error(errorMessage, context);
                
                throw new NotSupportedException(errorMessage);
            }

            _singletons ??= new AppalachiaRepositorySingletonReferenceList();

            if (!_singletonLookup.ContainsKey(t))
            {
                if (t.InheritsFrom(typeof(AppalachiaObject)))
                {
                }

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
            var assetReference = reference.AssetReference;
            ISingleton result;

            var isLoading = assetReference.IsValid() && (assetReference.OperationHandle.PercentComplete > 0f);

            if (isLoading)
            {
                await AppaTask.WaitUntil(() => assetReference.IsDone);
            }

            var isLoaded = assetReference.IsValid() && assetReference.IsDone;

            if (isLoaded)
            {
                result = assetReference.Asset as ISingleton;

                if (result == null)
                {
                    Context.Log.Error(
                        ZString.Format(
                            "Null reference for type {0} in the {1}!",
                            t,
                            nameof(AppalachiaRepository).FormatForLogging()
                        )
                    );
                    return null;
                }

                return result;
            }

            try
            {
#if UNITY_EDITOR
                if (!assetReference.RuntimeKeyIsValid())
                {
                    if (reference.editorAsset != null)
                    {
                        result = reference.editorAsset as ISingleton;
                    }
                    else if (assetReference.editorAsset != null)
                    {
                        result = assetReference.editorAsset as ISingleton;
                    }
                    else
                    {
                        var referenceType = reference.GetReferenceType();
                        result = AssetDatabaseManager.FindFirstAssetMatch(referenceType) as ISingleton;
                    }
                }
                else
                {
                    result = await assetReference.LoadAssetAsync<ISingleton>();
                }
#else
                result = await assetReference.LoadAssetAsync<ISingleton>();
#endif
            }
            catch (Exception ex)
            {
                Context.Log.Error(
                    ZString.Format("Failed to load singleton of type {0}: {1}", t.FormatForLogging(), ex.Message),
                    null,
                    ex
                );

                return null;
            }

            return result;
        }

        internal bool IsTypeInRepository<T>()
        {
            using (_PRF_IsTypeInRepository.Auto())
            {
                return IsTypeInRepository(typeof(T));
            }
        }

        internal bool IsTypeInRepository(Type t)
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
