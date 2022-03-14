using System;
using System.Collections.Generic;
using Appalachia.Core.ControlModel.Components.Contracts;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Logging;
using Appalachia.Utility.Standards;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Appalachia.Core.ControlModel
{
    public static class AppaConfigTracker
    {
        #region Static Fields and Autoproperties

        [NonSerialized] private static Dictionary<int, IAppaComponentConfig> _componentIDToComponentConfigLookup;
        [NonSerialized] private static Dictionary<ObjectID, IAppaComponentConfig> _cachedComponentConfigs;
        [NonSerialized] private static Dictionary<ObjectID, Type> _cachedComponentConfigTypes;

        #endregion

        /// <summary>
        ///     Finds a cached component config by key.
        /// </summary>
        /// <param name="id">The component's <see cref="ObjectID" />.</param>
        /// <typeparam name="TConfig">The type of config.</typeparam>
        /// <returns>The cached component config.</returns>
        /// <exception cref="InvalidKeyException">The <see cref="id" /> provided is null.</exception>
        /// <exception cref="NotSupportedException">An incorrect type is requested.</exception>
        /// <exception cref="KeyNotFoundException">The key does not map to a cached config.</exception>
        public static TConfig Get<TConfig>(ObjectID id)
        {
            using (_PRF_Get.Auto())
            {
                string errorMessage;

                ValidateID<TConfig>(id);

                if (_cachedComponentConfigs.TryGetValue(id, out var result))
                {
                    if (result is TConfig t)
                    {
                        return t;
                    }

                    errorMessage = ZString.Format(
                        "A {0} was requested, but a cached {1} was found. ID: {2}",
                        typeof(TConfig).FormatForLogging(),
                        result.GetType().FormatForLogging(),
                        id.FormatForLogging()
                    );

                    AppaLog.Error(errorMessage);
                    throw new NotSupportedException(errorMessage);
                }

                errorMessage = ZString.Format(
                    "No {0} was found with ID: {1}",
                    typeof(TConfig).FormatForLogging(),
                    id.FormatForLogging()
                );

                AppaLog.Error(errorMessage);

                throw new KeyNotFoundException(errorMessage);
            }
        }

        public static void Store<TConfig>(Component c, TConfig d)
            where TConfig : IAppaComponentConfig
        {
            using (_PRF_Store.Auto())
            {
                ValidateSingleConfigOwnerForComponent(c, d);
                CacheComponentConfig(c, d);
            }
        }

        private static void CacheComponentConfig<TConfig>(Component c, TConfig d)
            where TConfig : IAppaComponentConfig
        {
            using (_PRF_CacheComponentConfig.Auto())
            {
                InitializeCollections();
                var configType = typeof(TConfig);
                var id = d.ObjectID;

                if (CheckIfPreviousCacheWasDifferentType(c, id, configType))
                {
                    return;
                }

                CacheConfigInstance(d, id, configType);
            }
        }

        private static void CacheConfigInstance<TConfig>(TConfig d, ObjectID id, Type configType)
            where TConfig : IAppaComponentConfig
        {
            using (_PRF_CacheConfigInstance.Auto())
            {
                _cachedComponentConfigTypes.Add(id, configType);

                if (!_cachedComponentConfigs.ContainsKey(id))
                {
                    _cachedComponentConfigs.Add(id, d);
                }
            }
        }

        private static bool CheckIfPreviousCacheWasDifferentType(Component c, ObjectID id, Type configType)
        {
            using (_PRF_CheckIfPreviousCacheWasDifferentType.Auto())
            {
                var componentType = c.GetType();

                if (_cachedComponentConfigTypes.TryGetValue(id, out var cachedConfigType))
                {
                    if (configType != cachedConfigType)
                    {
                        AppaLog.Error(
                            ZString.Format(
                                "The {0} on {1} has a {2} controlling it which was previously cached as a {3}.  ID: {4}",
                                componentType.FormatForLogging(),
                                c.name.FormatNameForLogging(),
                                configType.FormatForLogging(),
                                cachedConfigType.FormatForLogging(),
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
                _cachedComponentConfigs ??= new();
                _cachedComponentConfigTypes ??= new();
                _componentIDToComponentConfigLookup ??= new();
            }
        }

        private static void ValidateID<TConfig>(ObjectID id)
        {
            using (_PRF_ValidateID.Auto())
            {
                if (id == null)
                {
                    var errorMessage = ZString.Format(
                        "The key is null. It should be initialized before getting a {0}.",
                        typeof(TConfig).FormatForLogging()
                    );

                    AppaLog.Error(errorMessage);

                    throw new InvalidKeyException(errorMessage);
                }

                if (id == ObjectID.Empty)
                {
                    var errorMessage = ZString.Format(
                        "The key is empty. It should be re-initialized before getting a {0}.",
                        typeof(TConfig).FormatForLogging()
                    );

                    AppaLog.Error(errorMessage);

                    throw new InvalidKeyException(errorMessage);
                }
            }
        }

        private static void ValidateSingleConfigOwnerForComponent<TConfig>(Component c, TConfig d)
            where TConfig : IAppaComponentConfig
        {
            using (_PRF_ValidateSingleConfigOwnerForComponent.Auto())
            {
                InitializeCollections();
                var instanceID = c.GetInstanceID();

                if (!_componentIDToComponentConfigLookup.TryGetValue(instanceID, out var existing))
                {
                    _componentIDToComponentConfigLookup.Add(instanceID, d);
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

        #region Profiling

        private const string _PRF_PFX = nameof(AppaConfigTracker) + ".";

        private static readonly ProfilerMarker _PRF_ValidateSingleConfigOwnerForComponent =
            new ProfilerMarker(_PRF_PFX + nameof(ValidateSingleConfigOwnerForComponent));

        private static readonly ProfilerMarker _PRF_ValidateID = new ProfilerMarker(_PRF_PFX + nameof(ValidateID));

        private static readonly ProfilerMarker _PRF_Get = new ProfilerMarker(_PRF_PFX + nameof(Get));

        private static readonly ProfilerMarker _PRF_CacheConfigInstance =
            new ProfilerMarker(_PRF_PFX + nameof(CacheConfigInstance));

        private static readonly ProfilerMarker _PRF_CheckIfPreviousCacheWasDifferentType =
            new ProfilerMarker(_PRF_PFX + nameof(CheckIfPreviousCacheWasDifferentType));

        private static readonly ProfilerMarker _PRF_InitializeCollections =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeCollections));

        private static readonly ProfilerMarker _PRF_CacheComponentConfig =
            new ProfilerMarker(_PRF_PFX + nameof(CacheComponentConfig));

        private static readonly ProfilerMarker _PRF_Store = new ProfilerMarker(_PRF_PFX + nameof(Store));

        #endregion
    }
}
