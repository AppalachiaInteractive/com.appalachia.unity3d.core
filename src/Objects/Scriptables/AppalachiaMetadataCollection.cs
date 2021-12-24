using System.Collections.Generic;
using Appalachia.CI.Integration.Assets;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Collections;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Core.Objects.Scriptables
{
    [InspectorIcon(Brand.AppalachiaMetadataCollection.Icon)]
    public abstract class AppalachiaMetadataCollection<T, TValue, TL> : SingletonAppalachiaObject<T>
        where T : AppalachiaMetadataCollection<T, TValue, TL>
        where TValue : AppalachiaObject<TValue> /*, ICategorizable*/
        where TL : AppaList<TValue>, new()
    {
        #region Fields and Autoproperties

        [FormerlySerializedAs("generic")]
        [FormerlySerializedAs("defaultWrapper")]
        [FoldoutGroup("Default")]
        public TValue defaultValue;

        [SerializeField] private TL _all;

        #endregion

        public IReadOnlyList<TValue> all
        {
            get
            {
                if ((_all != null) && (_all.Count > 0))
                {
                    return _all;
                }

                _all = new TL();

#if UNITY_EDITOR
                PopulateAll(_all);
#endif

                return _all;
            }
        }

        protected TL all_internal
        {
            get
            {
                if ((_all != null) && (_all.Count > 0))
                {
                    return _all;
                }

                _all = new TL();

#if UNITY_EDITOR
                if (!AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    PopulateAll(_all);
                }
#endif

                return _all;
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await initializer.Do(
                    this,
                    nameof(defaultValue),
                    defaultValue == null,
                    () => defaultValue = all_internal.FirstOrDefault_NoAlloc()
                );

#if UNITY_EDITOR
                if (!AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    using (new AssetEditingScope())
                    {
                        RegisterNecessaryInstances();
                    }
                }
#endif
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AppalachiaMetadataCollection<T, TValue, TL>) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion

#if UNITY_EDITOR

        protected override string GetTitle()
        {
            return Brand.AppalachiaMetadataCollection.Text;
        }

        protected override string GetFallbackTitle()
        {
            return Brand.AppalachiaMetadataCollection.Fallback;
        }

        protected override string GetTitleColor()
        {
            return Brand.AppalachiaMetadataCollection.Color;
        }

        protected override string GetBackgroundColor()
        {
            return Brand.AppalachiaMetadataCollection.Banner;
        }

        protected virtual void RegisterNecessaryInstances()
        {
        }

        private static readonly ProfilerMarker _PRF_PopulateAll =
            new ProfilerMarker(_PRF_PFX + nameof(PopulateAll));

        protected void PopulateAll(TL values)
        {
            using (_PRF_PopulateAll.Auto())
            {
                values.Clear();

                var assets = AssetDatabaseManager.FindAssets<TValue>();

                for (var i = 0; i < assets.Count; i++)
                {
                    var asset = assets[i];

                    values.Add(asset);
                }
            }
        }

#endif
    }
}
