#region

using System;
using System.Collections.Generic;
using System.Reflection;
using Appalachia.Core.Attributes;
using Appalachia.Core.Collections.Implementations.Lists;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Core.Objects.Scriptables;
using Appalachia.Utility.Async;
using Appalachia.Utility.Async.External.Addressables;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Reflection.Extensions;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.AddressableAssets;

#endregion

namespace Appalachia.Core.Objects.Root
{
    [Critical]
    public sealed partial class AppalachiaRepository : AppalachiaObject
    {
        #region Fields and Autoproperties

        [SerializeField] private AssetReferenceList _singletons;
        [SerializeField] private stringList _singletonTypes;
        [NonSerialized] private Dictionary<Assembly, string> _assemblyNames;
        [NonSerialized] private Dictionary<Type, AssetReference> _referenceLookup;
        [NonSerialized] private Dictionary<Type, ISingleton> _lookup;

        #endregion

        public async AppaTask<T> Find<T>()
            where T : SingletonAppalachiaObject<T>, ISingleton<T>
        {
            using (_PRF_Find.Auto())
            {
                var result = await Find(typeof(T));

                if (result is not null)
                {
                    result.SetInstance(result);

                    return result as T;
                }

                return null;
            }
        }

        public async AppaTask<ISingleton> Find(Type t)
        {
            using (_PRF_Find.Auto())
            {
                if (!_referenceLookup.ContainsKey(t))
                {
                    Context.Log.Error(ZString.Format("Could not find type {0} in the lookup!", t));
                    return null;
                }

                var reference = _referenceLookup[t];
                ISingleton result;

                var isLoadedOrLoading = reference.IsValid();
                var isReferenceValid = reference.RuntimeKeyIsValid();

                if (!isLoadedOrLoading || !isReferenceValid)
                {
                    result = reference.Asset as ISingleton;

                    result?.SetInstance(result);

                    return result;
                }

                try
                {
                    await reference.LoadAssetAsync<ISingleton>();
                }
                catch (Exception ex)
                {
                    Context.Log.Error(
                        ZString.Format(
                            "Failed to load singleton of type [{0}: {1}",
                            t.FormatForLogging(),
                            ex.Message
                        ),
                        null,
                        ex
                    );

                    return null;
                }

                result = reference.Asset as ISingleton;

                result?.SetInstance(result);

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
                return _referenceLookup.ContainsKey(t);
            }
        }

        internal static async AppaTask<AppalachiaRepository> AwakeRepository()
        {
            using (_PRF_AwakeRepository.Auto())
            {
                var repositoryHandle = Addressables.LoadAssetAsync<AppalachiaRepository>(
                    nameof(AppalachiaRepository)
                );

                await repositoryHandle;

                SetInstance(repositoryHandle.Result);

                return instance;
            }
        }

        protected override void AfterInitialization()
        {
            using (_PRF_AfterInitialization.Auto())
            {
                _referenceLookup ??= new Dictionary<Type, AssetReference>();

                for (var index = 0; index < _singletons.Count; index++)
                {
                    var singleton = _singletons[index];
                    var singletonType = _singletonTypes[index];

                    var type = ReflectionExtensions.GetByName(singletonType);

                    if (type == null)
                    {
                        continue;
                    }

                    if (_referenceLookup.ContainsKey(type))
                    {
                        _referenceLookup[type] = singleton;
                    }
                    else
                    {
                        _referenceLookup.Add(type, singleton);
                    }
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AppalachiaRepository) + ".";

        private static readonly ProfilerMarker _PRF_AfterInitialization =
            new ProfilerMarker(_PRF_PFX + nameof(AfterInitialization));

        private static readonly ProfilerMarker _PRF_IsTypeInRepository =
            new ProfilerMarker(_PRF_PFX + nameof(IsTypeInRepository));

        private static readonly ProfilerMarker _PRF_AwakeRepository =
            new ProfilerMarker(_PRF_PFX + nameof(AwakeRepository));

        private static readonly ProfilerMarker _PRF_Find = new ProfilerMarker(_PRF_PFX + nameof(Find));

        #endregion
    }
}
