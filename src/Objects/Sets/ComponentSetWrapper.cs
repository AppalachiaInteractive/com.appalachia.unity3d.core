using System;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Sets
{
    [Serializable]
    public abstract class ComponentSetWrapper : AppalachiaSimpleBase
    {
        public static void EnsureReady<TSetWrapper, TSet, TSetMetadata>(
            ref TSetWrapper assignTo,
            GameObject parent,
            string identifier /*,
            string prefixOverride = null*/)
            where TSetWrapper : ComponentSetWrapper<TSet, TSetMetadata>, new()
            where TSet : ComponentSet<TSet, TSetMetadata>, new()
            where TSetMetadata : ComponentSetMetadata<TSet, TSetMetadata>
        {
            using (_PRF_EnsureReady.Auto())
            {
                if (assignTo == null)
                {
                    assignTo = new TSetWrapper();
                }

                assignTo.EnsureReady(parent, identifier /*, prefixOverride*/);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ComponentSetWrapper) + ".";

        private static readonly ProfilerMarker _PRF_EnsureReady =
            new ProfilerMarker(_PRF_PFX + nameof(EnsureReady));

        #endregion
    }

    [Serializable]
    public abstract class ComponentSetWrapper<TSet, TSetMetadata> : ComponentSetWrapper
        where TSet : ComponentSet<TSet, TSetMetadata>, new()
        where TSetMetadata : ComponentSetMetadata<TSet, TSetMetadata>
    {
        #region Fields and Autoproperties

        [SerializeField] private TSet _set;

        [SerializeField] private TSetMetadata _metadata;

        #endregion

        public TSet set
        {
            get => _set;
            set => _set = value;
        }

        public TSetMetadata metadata
        {
            get => _metadata;
            set => _metadata = value;
        }

        public void EnsureReady(GameObject parent, string identifier /*, string prefixOverride = null*/)
        {
            using (_PRF_EnsureReady.Auto())
            {
                if (AppalachiaApplication.IsPlaying)
                {
                    Context.Log.Error(
                        ZString.Format(
                            "{0} should be assigned before playing!",
                            typeof(TSetMetadata).FormatForLogging()
                        ),
                        parent
                    );
                }
                else
                {
                    if (_metadata == null)
                    {
                        _metadata = AppalachiaObject.LoadOrCreateNew<TSetMetadata>(identifier);
                    }
                }

                _set = new TSet();
                _set.CreateComponents(parent, identifier /*, prefixOverride*/);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ComponentSetWrapper<TSet, TSetMetadata>) + ".";

        private static readonly ProfilerMarker _PRF_EnsureReady =
            new ProfilerMarker(_PRF_PFX + nameof(EnsureReady));

        #endregion
    }
}
