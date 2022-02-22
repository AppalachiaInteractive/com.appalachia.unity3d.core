using System;
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Models;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Sets.Extensions;
using Appalachia.Utility.Async;
using Appalachia.Utility.Events.Collections;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Sets
{
    [Serializable]
    public abstract class ComponentSetData<TSet, TSetData> : AppalachiaObject<TSetData>,
                                                             IComponentSetData<TSet, TSetData>
        where TSet : ComponentSet<TSet, TSetData>, new()
        where TSetData : ComponentSetData<TSet, TSetData>, IComponentSetData<TSet, TSetData>
    {
        #region Fields and Autoproperties

        private ReusableDelegateCollection<TSet> _delegates;

        #endregion

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="set" />.
        /// </summary>
        /// <remarks>The primary API for applying component set data to component sets.</remarks>
        /// <param name="data">The optional component set data to refresh.</param>
        /// <param name="isElected">Whether the optional should default to elected.</param>
        /// <param name="set">The component set to apply the <paramref name="data" /> to.</param>
        /// <param name="setParent">The <see cref="GameObject" /> that the set should be a child of.</param>
        /// <param name="setName">The name of the set.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void RefreshAndUpdateComponentSet(
            ref Optional data,
            bool isElected,
            ref TSet set,
            GameObject setParent,
            string setName,
            Action<Optional, TSet> before,
            Action<Optional, TSet> after)
        {
            using (_PRF_RefreshAndUpdateComponentSet.Auto())
            {
                CreateOrRefreshSetData(ref data, isElected, setName);
                ComponentSet<TSet, TSetData>.GetOrAddComponents(ref set, data, setParent, setName);
                data.UpdateComponentSet(set, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="set" />.
        /// </summary>
        /// <remarks>The primary API for applying component set data to component sets.</remarks>
        /// <param name="data">The overridable component set data to refresh.</param>
        /// <param name="overriding">Whether the optional should default to overriding.</param>
        /// <param name="set">The component set to apply the <paramref name="data" /> to.</param>
        /// <param name="setParent">The <see cref="GameObject" /> that the set should be a child of.</param>
        /// <param name="setName">The name of the set.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void RefreshAndUpdateComponentSet(
            ref Override data,
            bool overriding,
            ref TSet set,
            GameObject setParent,
            string setName,
            Action<Override, TSet> before,
            Action<Override, TSet> after)
        {
            using (_PRF_RefreshAndUpdateComponentSet.Auto())
            {
                CreateOrRefreshSetData(ref data, overriding, setName);
                ComponentSet<TSet, TSetData>.GetOrAddComponents(ref set, data, setParent, setName);
                data.UpdateComponentSet(set, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="set" />.
        /// </summary>
        /// <remarks>The primary API for applying component set data to component sets.</remarks>
        /// <param name="data">The component set data to refresh.</param>
        /// <param name="set">The component set to apply the <paramref name="data" /> to.</param>
        /// <param name="setParent">The <see cref="GameObject" /> that the set should be a child of.</param>
        /// <param name="setName">The name of the set.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void RefreshAndUpdateComponentSet(
            ref TSetData data,
            ref TSet set,
            GameObject setParent,
            string setName,
            Action<TSetData, TSet> before,
            Action<TSetData, TSet> after)
        {
            using (_PRF_RefreshAndUpdateComponentSet.Auto())
            {
                CreateOrRefreshSetData(ref data, setName);
                ComponentSet<TSet, TSetData>.GetOrAddComponents(ref set, data, setParent, setName);
                data.UpdateComponentSet(set, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="set" />.
        /// </summary>
        /// <remarks>The primary API for applying component set data to component sets.</remarks>
        /// <param name="data">The optional component set data to refresh.</param>
        /// <param name="isElected">Whether the optional should default to elected.</param>
        /// <param name="set">The component set to apply the <paramref name="data" /> to.</param>
        /// <param name="setParent">The <see cref="GameObject" /> that the set should be a child of.</param>
        /// <param name="setName">The name of the set.</param>
        public static void RefreshAndUpdateComponentSet(
            ref Optional data,
            bool isElected,
            ref TSet set,
            GameObject setParent,
            string setName)
        {
            using (_PRF_RefreshAndUpdateComponentSet.Auto())
            {
                CreateOrRefreshSetData(ref data, isElected, setName);
                ComponentSet<TSet, TSetData>.GetOrAddComponents(ref set, data, setParent, setName);
                data.UpdateComponentSet(set);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="set" />.
        /// </summary>
        /// <remarks>The primary API for applying component set data to component sets.</remarks>
        /// <param name="data">The overridable component set data to refresh.</param>
        /// <param name="overriding">Whether the optional should default to overriding.</param>
        /// <param name="set">The component set to apply the <paramref name="data" /> to.</param>
        /// <param name="setParent">The <see cref="GameObject" /> that the set should be a child of.</param>
        /// <param name="setName">The name of the set.</param>
        public static void RefreshAndUpdateComponentSet(
            ref Override data,
            bool overriding,
            ref TSet set,
            GameObject setParent,
            string setName)
        {
            using (_PRF_RefreshAndUpdateComponentSet.Auto())
            {
                CreateOrRefreshSetData(ref data, overriding, setName);
                ComponentSet<TSet, TSetData>.GetOrAddComponents(ref set, data, setParent, setName);
                data.UpdateComponentSet(set);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="set" />.
        /// </summary>
        /// <remarks>The primary API for applying component set data to component sets.</remarks>
        /// <param name="data">The component set data to refresh.</param>
        /// <param name="set">The component set to apply the <paramref name="data" /> to.</param>
        /// <param name="setParent">The <see cref="GameObject" /> that the set should be a child of.</param>
        /// <param name="setName">The name of the set.</param>
        public static void RefreshAndUpdateComponentSet(
            ref TSetData data,
            ref TSet set,
            GameObject setParent,
            string setName)
        {
            using (_PRF_RefreshAndUpdateComponentSet.Auto())
            {
                CreateOrRefreshSetData(ref data, setName);
                ComponentSet<TSet, TSetData>.GetOrAddComponents(ref set, data, setParent, setName);
                data.UpdateComponentSet(set);
            }
        }

        /// <summary>
        ///     Updates the set by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="set" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="before" /> function.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the set is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current configuration data to the set.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="after" /> function.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="set">The set.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        internal void UpdateComponentSet(
            TSet set,
            Action<TSetData, TSet> before,
            Action<TSetData, TSet> after)
        {
            using (_PRF_UpdateComponentSet.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (set == null) return;
                        
                        UpdateComponentSetInternal(set, before, after);
                    }

                    return SubscribableDelegate;
                }

                UpdateComponentSetAndSubscribe(set, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Updates the set by applying the configuration values to the set fields.
        /// </summary>
        /// <param name="set">The set to update.</param>
        /// <param name="subscribe">Should the set be subscribed for subsequent updates?</param>
        /// <exception cref="NotSupportedException">Thrown whenever <paramref name="set" /> is null.</exception>
        internal void UpdateComponentSet(TSet set, bool subscribe = true)
        {
            using (_PRF_UpdateComponentSet.Auto())
            {
                if (!subscribe)
                {
                    UpdateComponentSetInternal(set);
                    return;
                }

                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (set == null) return;
                        
                        UpdateComponentSetInternal(set);
                    }

                    return SubscribableDelegate;
                }

                UpdateComponentSetAndSubscribe(set, CreateSubscribableDelegate);
            }
        }

        [ButtonGroup(GROUP_BUTTONS)]
        protected virtual void CreateOrRefresh()
        {
            using (_PRF_CreateOrRefresh.Auto())
            {
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                CreateOrRefresh();
            }
        }

        private static void CreateOrRefreshSetData(ref TSetData data, string setName)
        {
            using (_PRF_CreateOrRefreshSetData.Auto())
            {
#if UNITY_EDITOR
                if (!AppalachiaApplication.IsPlaying)
                {
                    var targetDataName = GetDataName(setName);

                    if (data == null)
                    {
                        data = LoadOrCreate(targetDataName);
                    }
                    else
                    {
                        AssetDatabaseManager.UpdateAssetName(data, targetDataName);
                    }
                }
#endif

                data.CreateOrRefresh();
            }
        }

        private static void CreateOrRefreshSetData(ref Optional data, bool isElected, string setName)
        {
            using (_PRF_CreateOrRefreshSetData.Auto())
            {
#if UNITY_EDITOR
                var targetDataName = GetDataName(setName);
#endif

                if (data == null)
                {
#if UNITY_EDITOR
                    data = new(isElected, LoadOrCreate(targetDataName));
#else
                    data = new (isElected, default);
#endif
                }
                else if (data.Value == null)
                {
#if UNITY_EDITOR
                    data.Value = LoadOrCreate(targetDataName);
#endif
                }
                else
                {
#if UNITY_EDITOR
                    if (data.Value.name != targetDataName)
                    {
                        AssetDatabaseManager.UpdateAssetName(data.Value, targetDataName);
                    }
#endif
                }

                data.Value.CreateOrRefresh();
            }
        }

        private static void CreateOrRefreshSetData(ref Override data, bool overriding, string setName)
        {
            using (_PRF_CreateOrRefreshSetData.Auto())
            {
#if UNITY_EDITOR
                var targetDataName = GetDataName(setName);
#endif

                if (data == null)
                {
#if UNITY_EDITOR
                    data = new(overriding, LoadOrCreate(targetDataName));
#else
                    data = new (overriding, default);
#endif
                }
                else if (data.Value == null)
                {
#if UNITY_EDITOR
                    data.Value = LoadOrCreate(targetDataName);
#endif
                }
                else
                {
#if UNITY_EDITOR
                    if (data.Value.name != targetDataName)
                    {
                        AssetDatabaseManager.UpdateAssetName(data.Value, targetDataName);
                    }
#endif
                }

                data.Value.CreateOrRefresh();
            }
        }

        private static string GetDataName(string setName)
        {
            using (_PRF_GetDataName.Auto())
            {
                var setType = typeof(TSetData);
                var targetDataName = ZString.Format("{0}{1}", setName, setType.Name);
                return targetDataName;
            }
        }

        private static TSetData LoadOrCreate(string targetDataName)
        {
            using (_PRF_LoadOrCreate.Auto())
            {
                return AppalachiaObject.LoadOrCreateNew<TSetData>(
                    targetDataName,
                    ownerType: AppalachiaRepository.PrimaryOwnerType
                );
            }
        }

        /// <summary>
        ///     Updates the set by applying the configuration values to the set fields.
        /// </summary>
        /// <param name="set">The set to update.</param>
        /// <param name="delegateCreator">A delegate to create the update/subscribe delegate.</param>
        /// <exception cref="NotSupportedException">Thrown whenever <paramref name="set" /> is null.</exception>
        private void UpdateComponentSetAndSubscribe(TSet set, Func<Action> delegateCreator)
        {
            using (_PRF_UpdateComponentSetAndSubscribe.Auto())
            {
                if (set == null)
                {
                    throw new ArgumentNullException(
                        nameof(set),
                        "You must initialize the set prior to configuring it!"
                    );
                }

                _delegates ??= new();
                _delegates.SubscribeAndInvoke(set, ref Changed, delegateCreator);
            }
        }

        /// <summary>
        ///     Updates the set by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="set" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="before" /> function.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the set set is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current configuration data to the set.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="after" /> function.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="set">The set.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        private void UpdateComponentSetInternal(
            TSet set,
            Action<TSetData, TSet> before,
            Action<TSetData, TSet> after)
        {
            using (_PRF_UpdateComponentSetInternal.Auto())
            {
                before?.Invoke(this as TSetData, set);
                UpdateComponentSetInternal(set);
                after?.Invoke(this as TSetData, set);
            }
        }

        /// <summary>
        ///     Updates the set by applying the configuration values to the set fields.
        /// </summary>
        /// <param name="set">The set to update.</param>
        /// <exception cref="NotSupportedException">Thrown whenever <see cref="set" /> is null.</exception>
        private void UpdateComponentSetInternal(TSet set)
        {
            using (_PRF_UpdateComponentSetInternal.Auto())
            {
                ApplyToComponentSet(set);
            }
        }

        #region IComponentSetData<TSet,TSetData> Members

        public abstract void ApplyToComponentSet(TSet componentSet);

        #endregion

        #region Nested type: Optional

        [Serializable]
        public sealed class Optional : Overridable<Optional>
        {
            public Optional() : base(false, default)
            {
            }

            public Optional(bool isElected, TSetData value) : base(isElected, value)
            {
            }

            public Optional(Optional value) : base(value)
            {
            }

            public bool IsElected => Overriding;

            /// <inheritdoc />
            protected override string DisabledColorPrefName => "Optional Disabled Color";

            /// <inheritdoc />
            protected override string EnabledColorPrefName => "Optional Enabled Color";

            /// <inheritdoc />
            protected override string ToggleLabel => "Optional";
        }

        #endregion

        #region Nested type: Overridable

        [Serializable]
        public abstract class Overridable<TO> : Overridable<TSetData, TO>
            where TO : Overridable<TO>, new()
        {
            protected Overridable() : base(false, default)
            {
            }

            protected Overridable(bool overriding, TSetData value) : base(overriding, value)
            {
            }

            protected Overridable(TO value) : base(value)
            {
            }
        }

        #endregion

        #region Nested type: Override

        [Serializable]
        public class Override : Overridable<Override>
        {
            public Override(TSetData value) : base(false, value)
            {
            }

            public Override(bool overrideEnabled, TSetData value) : base(overrideEnabled, value)
            {
            }

            public Override(Override value) : base(value)
            {
            }

            public Override() : base(false, default)
            {
            }

            /// <inheritdoc />
            protected override string DisabledColorPrefName => "Component Set Override Disabled Color";

            /// <inheritdoc />
            protected override string EnabledColorPrefName => "Component Set Override Enabled Color";

            /// <inheritdoc />
            protected override string ToggleLabel => "Override Set";
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_RefreshAndUpdateComponentSet =
            new ProfilerMarker(_PRF_PFX + nameof(RefreshAndUpdateComponentSet));

        private static readonly ProfilerMarker _PRF_UpdateComponentSet =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateComponentSet));

        private static readonly ProfilerMarker _PRF_UpdateComponentSetAndSubscribe =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateComponentSetAndSubscribe));

        private static readonly ProfilerMarker _PRF_UpdateComponentSetInternal =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateComponentSetInternal));

        private static readonly ProfilerMarker _PRF_LoadOrCreate =
            new ProfilerMarker(_PRF_PFX + nameof(LoadOrCreate));

        private static readonly ProfilerMarker _PRF_GetDataName =
            new ProfilerMarker(_PRF_PFX + nameof(GetDataName));

        protected static readonly ProfilerMarker _PRF_ApplyToComponentSet =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyToComponentSet));

        protected static readonly ProfilerMarker _PRF_CreateOrRefresh =
            new ProfilerMarker(_PRF_PFX + nameof(CreateOrRefresh));

        private static readonly ProfilerMarker _PRF_CreateOrRefreshSetData =
            new ProfilerMarker(_PRF_PFX + nameof(CreateOrRefreshSetData));

        #endregion
    }
}
