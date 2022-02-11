using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Sets
{
    internal static class ComponentSetAPI<TSet, TSetMetadata>
        where TSet : ComponentSet<TSet, TSetMetadata>, new()
        where TSetMetadata : ComponentSetData<TSet, TSetMetadata>
    {
        public static void UpdateComponentSet(
            ref TSet set,
            ref ComponentSetData<TSet, TSetMetadata>.Override data,
            GameObject parent,
            string setName)
        {
            UpdateComponentSetInternal(ref data, ref set, parent, setName);
        }

        public static void UpdateComponentSet(
            ref TSet set,
            ref ComponentSetData<TSet, TSetMetadata>.Optional data,
            GameObject parent,
            string setName)
        {
            UpdateComponentSetInternal(ref data, ref set, parent, setName);
        }

        public static void UpdateComponentSet(
            ref TSet set,
            ref TSetMetadata data,
            GameObject parent,
            string setName)
        {
            UpdateComponentSetInternal(ref data, ref set, parent, setName);
        }

        public static void UpdateComponentSet(
            ref ComponentSetData<TSet, TSetMetadata>.Override data,
            ref TSet set,
            GameObject parent,
            string setName)
        {
            UpdateComponentSetInternal(ref data, ref set, parent, setName);
        }

        public static void UpdateComponentSet(
            ref ComponentSetData<TSet, TSetMetadata>.Optional data,
            ref TSet set,
            GameObject parent,
            string setName)
        {
            UpdateComponentSetInternal(ref data, ref set, parent, setName);
        }

        public static void UpdateComponentSet(
            ref TSetMetadata data,
            ref TSet set,
            GameObject parent,
            string setName)
        {
            using (_PRF_UpdateComponentSet.Auto())
            {
                UpdateComponentSetInternal(ref data, ref set, parent, setName);
            }
        }

        private static void ApplyMetadataToComponentSet(TSetMetadata data, TSet target)
        {
            using (_PRF_ApplyMetadataToComponentSet.Auto())
            {
                data.ApplyMetadataToComponentSet(target);
            }
        }

        private static void ConfigureComponentSet(
            TSetMetadata data,
            ref TSet set,
            GameObject parent,
            string setName)
        {
            using (_PRF_ConfigureComponentSet.Auto())
            {
                if (set == null)
                {
                    set = new TSet();
                }

                ComponentSet<TSet, TSetMetadata>.GetOrAddComponents(data, set, parent, setName);
            }
        }

        private static void CreateComponentSetMetadata(ref TSetMetadata data, string setName)
        {
            using (_PRF_CreateComponentSetMetadata.Auto())
            {
#if UNITY_EDITOR
                var targetDataName = GetDataName(setName);

                if (data == null)
                {
                    data = LoadOrCreate(targetDataName);
                }
                else
                {
                    AssetDatabaseManager.UpdateAssetName(data, targetDataName);
                }
#endif
            }
        }

        private static void CreateComponentSetMetadata(
            ref ComponentSetData<TSet, TSetMetadata>.Optional optionalData,
            string setName)
        {
            using (_PRF_CreateComponentSetMetadata.Auto())
            {
#if UNITY_EDITOR
                var targetDataName = GetDataName(setName);
#endif

                if (optionalData == null)
                {
#if UNITY_EDITOR
                    optionalData = new ComponentSetData<TSet, TSetMetadata>.Optional(
                        false,
                        LoadOrCreate(targetDataName)
                    );
#else
                    data = new ComponentSetMetadata<TSet, TSetMetadata>.Override(false, default);
#endif
                }
                else if (optionalData.Value == null)
                {
#if UNITY_EDITOR
                    optionalData.Value = LoadOrCreate(targetDataName);
#endif
                }
                else
                {
#if UNITY_EDITOR
                    if (optionalData.Value.name != targetDataName)
                    {
                        AssetDatabaseManager.UpdateAssetName(optionalData.Value, targetDataName);
                    }
#endif
                }
            }
        }

        private static void CreateComponentSetMetadata(
            ref ComponentSetData<TSet, TSetMetadata>.Override overrideData,
            string setName)
        {
            using (_PRF_CreateComponentSetMetadata.Auto())
            {
#if UNITY_EDITOR
                var targetDataName = GetDataName(setName);
#endif

                if (overrideData == null)
                {
#if UNITY_EDITOR
                    overrideData = new ComponentSetData<TSet, TSetMetadata>.Override(
                        false,
                        LoadOrCreate(targetDataName)
                    );
#else
                    data = new ComponentSetMetadata<TSet, TSetMetadata>.Override(false, default);
#endif
                }
                else if (overrideData.Value == null)
                {
#if UNITY_EDITOR
                    overrideData.Value = LoadOrCreate(targetDataName);
#endif
                }
                else
                {
#if UNITY_EDITOR
                    if (overrideData.Value.name != targetDataName)
                    {
                        AssetDatabaseManager.UpdateAssetName(overrideData.Value, targetDataName);
                    }
#endif
                }
            }
        }

        private static string GetDataName(string setName)
        {
            using (_PRF_GetDataName.Auto())
            {
                var setType = typeof(TSetMetadata);
                var targetDataName = ZString.Format("{0}{1}", setName, setType.Name);
                return targetDataName;
            }
        }

        private static TSetMetadata LoadOrCreate(string targetDataName)
        {
            using (_PRF_LoadOrCreate.Auto())
            {
                return AppalachiaObject.LoadOrCreateNew<TSetMetadata>(
                    targetDataName,
                    ownerType: AppalachiaRepository.PrimaryOwnerType
                );
            }
        }

        private static void UpdateComponentSetInternal(
            ref ComponentSetData<TSet, TSetMetadata>.Optional optionalData,
            ref TSet set,
            GameObject parent,
            string setName)
        {
            using (_PRF_UpdateComponentSetInternal.Auto())
            {
                CreateComponentSetMetadata(ref optionalData, setName);

                ConfigureComponentSet(optionalData, ref set, parent, setName);

                if (optionalData.IsElected)
                {
                    if (!set.GameObject.activeSelf)
                    {
                        set.EnableSet();
                    }

                    ApplyMetadataToComponentSet(optionalData, set);
                }
                else
                {
                    set.DisableSet();
                }
            }
        }

        private static void UpdateComponentSetInternal(
            ref ComponentSetData<TSet, TSetMetadata>.Override overrideData,
            ref TSet set,
            GameObject parent,
            string setName)
        {
            using (_PRF_UpdateComponentSetInternal.Auto())
            {
                CreateComponentSetMetadata(ref overrideData, setName);

                ConfigureComponentSet(overrideData, ref set, parent, setName);

                if (overrideData.Overriding)
                {
                    if (!set.GameObject.activeSelf)
                    {
                        set.EnableSet();
                    }

                    ApplyMetadataToComponentSet(overrideData, set);
                }
            }
        }

        private static void UpdateComponentSetInternal(
            ref TSetMetadata data,
            ref TSet set,
            GameObject parent,
            string setName)
        {
            using (_PRF_UpdateComponentSetInternal.Auto())
            {
                CreateComponentSetMetadata(ref data, setName);

                ConfigureComponentSet(data, ref set, parent, setName);

                ApplyMetadataToComponentSet(data, set);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ComponentSetAPI<TSet, TSetMetadata>) + ".";

        private static readonly ProfilerMarker _PRF_ApplyMetadataToComponentSet =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyMetadataToComponentSet));

        private static readonly ProfilerMarker _PRF_ConfigureComponentSet =
            new ProfilerMarker(_PRF_PFX + nameof(ConfigureComponentSet));

        private static readonly ProfilerMarker _PRF_LoadOrCreate =
            new ProfilerMarker(_PRF_PFX + nameof(LoadOrCreate));

        private static readonly ProfilerMarker _PRF_GetDataName =
            new ProfilerMarker(_PRF_PFX + nameof(GetDataName));

        private static readonly ProfilerMarker _PRF_CreateComponentSetMetadata =
            new ProfilerMarker(_PRF_PFX + nameof(CreateComponentSetMetadata));

        private static readonly ProfilerMarker _PRF_UpdateComponentSetInternal =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateComponentSetInternal));

        private static readonly ProfilerMarker _PRF_UpdateComponentSet =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateComponentSet));

        #endregion
    }
}
