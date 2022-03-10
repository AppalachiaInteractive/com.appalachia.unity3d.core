using System;
using System.Collections.Generic;
using Appalachia.Core.Objects.Components.Core;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Logging;
using Appalachia.Utility.Standards;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Appalachia.Core.Objects.Components
{
    public static class ComponentDataTracker
    {
        #region Static Fields and Autoproperties

        [NonSerialized] private static Dictionary<int, IComponentData> _componentIDToComponentDataLookup;
        [NonSerialized] private static Dictionary<ObjectID, IComponentData> _cachedComponentDatas;

        [NonSerialized] private static Dictionary<ObjectID, IComponentData> _componentObjectIDToComponentDataLookup;
        [NonSerialized] private static Dictionary<ObjectID, Type> _cachedComponentDataTypes;

        #endregion

        /// <summary>
        ///     Finds a cached component data by key.
        /// </summary>
        /// <param name="id">The component's <see cref="ObjectID" />.</param>
        /// <typeparam name="TComponentData">The type of data.</typeparam>
        /// <returns>The cached component data.</returns>
        /// <exception cref="InvalidKeyException">The <see cref="id" /> provided is null.</exception>
        /// <exception cref="NotSupportedException">An incorrect type is requested.</exception>
        /// <exception cref="KeyNotFoundException">The key does not map to a cached data.</exception>
        public static TComponentData GetComponentData<TComponentData>(ObjectID id)
        {
            using (_PRF_GetComponentData.Auto())
            {
                string errorMessage;

                ValidateID<TComponentData>(id);

                if (_cachedComponentDatas.TryGetValue(id, out var result))
                {
                    if (result is TComponentData t)
                    {
                        return t;
                    }

                    errorMessage = ZString.Format(
                        "A {0} was requested, but a cached {1} was found. ID: {2}",
                        typeof(TComponentData).FormatForLogging(),
                        result.GetType().FormatForLogging(),
                        id.FormatForLogging()
                    );

                    AppaLog.Error(errorMessage);
                    throw new NotSupportedException(errorMessage);
                }

                errorMessage = ZString.Format(
                    "No {0} was found with ID: {1}",
                    typeof(TComponentData).FormatForLogging(),
                    id.FormatForLogging()
                );

                AppaLog.Error(errorMessage);

                throw new KeyNotFoundException(errorMessage);
            }
        }

        public static void RegisterComponentData<TComponentData>(IUnique u, TComponentData d)
            where TComponentData : IComponentData
        {
            using (_PRF_RegisterComponentData.Auto())
            {
                ValidateSingleDataOwnerForUnique(u, d);
                CacheComponentData(u, d);
            }
        }

        public static void RegisterComponentData<TComponentData>(Component c, TComponentData d)
            where TComponentData : IComponentData
        {
            using (_PRF_RegisterComponentData.Auto())
            {
                ValidateSingleDataOwnerForComponent(c, d);
                CacheComponentData(c, d);
            }
        }

        private static void CacheComponentData<TComponentData>(IUnique u, TComponentData d)
            where TComponentData : IComponentData
        {
            using (_PRF_CacheComponentData.Auto())
            {
                InitializeCollections();
                var dataType = typeof(TComponentData);
                var id = d.ObjectID;

                if (CheckIfPreviousCacheWasDifferentType(u, id, dataType))
                {
                    return;
                }

                CacheDataInstance(d, id, dataType);
            }
        }

        private static void CacheComponentData<TComponentData>(Component c, TComponentData d)
            where TComponentData : IComponentData
        {
            using (_PRF_CacheComponentData.Auto())
            {
                InitializeCollections();
                var dataType = typeof(TComponentData);
                var id = d.ObjectID;

                if (CheckIfPreviousCacheWasDifferentType(c, id, dataType))
                {
                    return;
                }

                CacheDataInstance(d, id, dataType);
            }
        }

        private static void CacheDataInstance<TComponentData>(TComponentData d, ObjectID id, Type dataType)
            where TComponentData : IComponentData
        {
            using (_PRF_CacheDataInstance.Auto())
            {
                _cachedComponentDataTypes.Add(id, dataType);

                if (!_cachedComponentDatas.ContainsKey(id))
                {
                    _cachedComponentDatas.Add(id, d);
                }
            }
        }

        private static bool CheckIfPreviousCacheWasDifferentType(IUnique u, ObjectID id, Type dataType)
        {
            using (_PRF_CheckIfPreviousCacheWasDifferentType.Auto())
            {
                var componentType = u.GetType();

                var name = u is INamed n ? n.NiceName : u.GetType().Name;

                if (_cachedComponentDataTypes.TryGetValue(id, out var cachedDataType))
                {
                    if (dataType != cachedDataType)
                    {
                        AppaLog.Error(
                            ZString.Format(
                                "The {0} on {1} has a {2} controlling it which was previously cached as a {3}.  ID: {4}",
                                componentType.FormatForLogging(),
                                name.FormatNameForLogging(),
                                dataType.FormatForLogging(),
                                cachedDataType.FormatForLogging(),
                                id.FormatForLogging()
                            )
                        );
                    }

                    return true;
                }

                return false;
            }
        }

        private static bool CheckIfPreviousCacheWasDifferentType(Component c, ObjectID id, Type dataType)
        {
            using (_PRF_CheckIfPreviousCacheWasDifferentType.Auto())
            {
                var componentType = c.GetType();

                if (_cachedComponentDataTypes.TryGetValue(id, out var cachedDataType))
                {
                    if (dataType != cachedDataType)
                    {
                        AppaLog.Error(
                            ZString.Format(
                                "The {0} on {1} has a {2} controlling it which was previously cached as a {3}.  ID: {4}",
                                componentType.FormatForLogging(),
                                c.name.FormatNameForLogging(),
                                dataType.FormatForLogging(),
                                cachedDataType.FormatForLogging(),
                                id.FormatForLogging()
                            ),
                            c
                        );
                    }

                    return true;
                }

                return false;
            }
        }

        private static void InitializeCollections()
        {
            using (_PRF_InitializeCollections.Auto())
            {
                _cachedComponentDatas ??= new();
                _cachedComponentDataTypes ??= new();
                _componentIDToComponentDataLookup ??= new();
                _componentObjectIDToComponentDataLookup ??= new();
            }
        }

        private static void ValidateID<TComponentData>(ObjectID id)
        {
            using (_PRF_ValidateID.Auto())
            {
                if (id == null)
                {
                    var errorMessage = ZString.Format(
                        "The key is null. It should be initialized before getting a {0}.",
                        typeof(TComponentData).FormatForLogging()
                    );

                    AppaLog.Error(errorMessage);

                    throw new InvalidKeyException(errorMessage);
                }

                if (id == ObjectID.Empty)
                {
                    var errorMessage = ZString.Format(
                        "The key is empty. It should be re-initialized before getting a {0}.",
                        typeof(TComponentData).FormatForLogging()
                    );

                    AppaLog.Error(errorMessage);

                    throw new InvalidKeyException(errorMessage);
                }
            }
        }

        private static void ValidateSingleDataOwnerForComponent<TComponentData>(Component c, TComponentData d)
            where TComponentData : IComponentData
        {
            using (_PRF_ValidateSingleDataOwnerForComponent.Auto())
            {
                InitializeCollections();
                var instanceID = c.GetInstanceID();

                if (!_componentIDToComponentDataLookup.TryGetValue(instanceID, out var existing))
                {
                    _componentIDToComponentDataLookup.Add(instanceID, d);
                    return;
                }

                if (existing.ObjectID == d.ObjectID)
                {
                    return;
                }

                AppaLog.Error(
                    ZString.Format(
                        "The {0} on {1} has more than one {2} controlling it, owned by {3} and {4}. This can lead to unexpected behaviour.",
                        c.GetType().FormatForLogging(),
                        c.name.FormatNameForLogging(),
                        d.GetType().FormatForLogging(),
                        existing.Owner.name,
                        d.Owner.name
                    ),
                    c
                );

                existing.SharesControlError = true;
                d.SharesControlError = true;

                existing.SharesControlWith = d.Owner.name;
                d.SharesControlWith = existing.Owner.name;
            }
        }

        private static void ValidateSingleDataOwnerForUnique<TComponentData>(IUnique c, TComponentData d)
            where TComponentData : IComponentData
        {
            using (_PRF_ValidateSingleDataOwnerForUnique.Auto())
            {
                InitializeCollections();
                var instanceID = c.ObjectID;

                if (!_componentObjectIDToComponentDataLookup.TryGetValue(instanceID, out var existing))
                {
                    _componentObjectIDToComponentDataLookup.Add(instanceID, d);
                    return;
                }

                if (existing.ObjectID == d.ObjectID)
                {
                    return;
                }

                AppaLog.Error(
                    ZString.Format(
                        "The {0} has more than one {1} controlling it, owned by {2} and {3}. This can lead to unexpected behaviour.",
                        c.GetType().FormatForLogging(),
                        d.GetType().FormatForLogging(),
                        existing.Owner.name,
                        d.Owner.name
                    )
                );

                existing.SharesControlError = true;
                d.SharesControlError = true;

                existing.SharesControlWith = d.Owner.name;
                d.SharesControlWith = existing.Owner.name;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ComponentDataTracker) + ".";

        private static readonly ProfilerMarker _PRF_ValidateID = new ProfilerMarker(_PRF_PFX + nameof(ValidateID));

        private static readonly ProfilerMarker _PRF_GetComponentData =
            new ProfilerMarker(_PRF_PFX + nameof(GetComponentData));

        private static readonly ProfilerMarker _PRF_ValidateSingleDataOwnerForUnique =
            new ProfilerMarker(_PRF_PFX + nameof(ValidateSingleDataOwnerForUnique));

        private static readonly ProfilerMarker _PRF_CacheDataInstance =
            new ProfilerMarker(_PRF_PFX + nameof(CacheDataInstance));

        private static readonly ProfilerMarker _PRF_CheckIfPreviousCacheWasDifferentType =
            new ProfilerMarker(_PRF_PFX + nameof(CheckIfPreviousCacheWasDifferentType));

        private static readonly ProfilerMarker _PRF_InitializeCollections =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeCollections));

        private static readonly ProfilerMarker _PRF_CacheComponentData =
            new ProfilerMarker(_PRF_PFX + nameof(CacheComponentData));

        private static readonly ProfilerMarker _PRF_ValidateSingleDataOwnerForComponent =
            new ProfilerMarker(_PRF_PFX + nameof(ValidateSingleDataOwnerForUnique));

        private static readonly ProfilerMarker _PRF_RegisterComponentData =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterComponentData));

        #endregion
    }
}
