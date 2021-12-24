using System;
using Appalachia.CI.Integration.Core;
using Appalachia.Core.Collections.Implementations.Lists;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Collections.Context
{
    [Serializable]
    public abstract class ContextSpecific<T, TList> : AppaLookup<string, T, stringList, TList>
        where TList : AppaList<T>, new()
        where T : ScriptableObject
    {
        public abstract string GetContextKey();

        public T CreateNew(string context)
        {
            using (_PRF_CreateNew.Auto())
            {
                var contextKey = GetContextKey();

                var assetName = ZString.Format("{0}_{1}_{2}", typeof(T).Name, contextKey, context);

                var result = AppalachiaObjectFactory.LoadExistingOrCreateNewAsset<T>(assetName);

                Add(contextKey, result);

                return result;
            }
        }

        public T GetContext()
        {
            using (_PRF_GetContext.Auto())
            {
                var key = GetContextKey();

                return Get(key);
            }
        }

        public bool HasContext()
        {
            using (_PRF_HasContext.Auto())
            {
                var key = GetContextKey();

                return ContainsKey(key);
            }
        }

        public void SetContext(T context)
        {
            using (_PRF_SetContext.Auto())
            {
                var key = GetContextKey();

                this[key] = context;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ContextSpecific<T, TList>) + ".";

        private static readonly ProfilerMarker _PRF_GetContext =
            new ProfilerMarker(_PRF_PFX + nameof(GetContext));

        private static readonly ProfilerMarker _PRF_HasContext =
            new ProfilerMarker(_PRF_PFX + nameof(HasContext));

        private static readonly ProfilerMarker _PRF_SetContext =
            new ProfilerMarker(_PRF_PFX + nameof(SetContext));

        private static readonly ProfilerMarker _PRF_CreateNew =
            new ProfilerMarker(_PRF_PFX + nameof(CreateNew));

        #endregion
    }
}
