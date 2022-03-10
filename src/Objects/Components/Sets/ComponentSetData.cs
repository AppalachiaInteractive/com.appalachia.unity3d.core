using System;
using System.Diagnostics;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Components.Contracts;
using Appalachia.Core.Objects.Components.Core;
using Appalachia.Core.Objects.Components.Exceptions;
using Appalachia.Core.Objects.Components.Extensions;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Events.Collections;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Objects.Components.Sets
{
    [CallStaticConstructorInEditor]
    [Serializable]
    [DebuggerDisplay("{Name} (ComponentSetData)")]
    public abstract partial class ComponentSetData<TComponentSet, TComponentSetData> :
        AppalachiaBase<TComponentSetData>,
        IComponentSetData<TComponentSet, TComponentSetData>,
        IInspectorVisibility,
        IFieldLockable,
        IRemotelyEnabled
        where TComponentSet : ComponentSet<TComponentSet, TComponentSetData>, new()
        where TComponentSetData : ComponentSetData<TComponentSet, TComponentSetData>,
        IComponentSetData<TComponentSet, TComponentSetData>, new()
    {
        #region Constants and Static Readonly

        private const string UPDATE_FAILURE_LOG_FORMAT = "Failed to update the {0}: {1}";

        #endregion

        #region Fields and Autoproperties

        [NonSerialized] private bool _sharesControlError;
        [NonSerialized] private string _sharesControlWith;

        [SerializeField, HideInInspector]
        private bool _showAdvancedOptions;

        [NonSerialized] private ReusableDelegateCollection<TComponentSet> _delegates;

        [NonSerialized] private bool _showAllFields;

        [SerializeField, HideInInspector]
        private bool _hideAllFields;

        [SerializeField, HideInInspector]
        private bool _disableAllFields;

        [SerializeField, HideInInspector]
        private bool _suspendFieldApplication;

        [NonSerialized] private Func<bool> _shouldEnableFunction;

        #endregion

        public static void CreateOrRefresh(ref TComponentSetData data, Object owner)
        {
            using (_PRF_CreateOrRefresh.Auto())
            {
                data ??= new();

                data.CreateOrRefresh(owner);
            }
        }

        public static void CreateOrRefresh(ref Optional data, bool isElected, Object owner)
        {
            using (_PRF_CreateOrRefresh.Auto())
            {
                data ??= new(isElected, new());
                data.Value ??= new();

                data.Value.CreateOrRefresh(owner);
            }
        }

        public static void CreateOrRefresh(ref Override data, bool overriding, Object owner)
        {
            using (_PRF_CreateOrRefresh.Auto())
            {
                data ??= new(overriding, new());
                data.Value ??= new();

                data.Value.CreateOrRefresh(owner);
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
        /// <param name="gameObjectNamePostfix">The name of the set.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        /// <param name="owner"></param>
        public static void RefreshAndApply(
            ref Optional data,
            bool isElected,
            ref TComponentSet set,
            GameObject setParent,
            string gameObjectNamePostfix,
            Action<Optional, TComponentSet> before,
            Action<Optional, TComponentSet> after,
            Object owner)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, isElected, owner);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(
                    ref set,
                    setParent,
                    gameObjectNamePostfix
                );
                data.Apply(set, before, after);
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
        /// <param name="gameObjectNamePostfix">The name of the set.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        /// <param name="owner"></param>
        public static void RefreshAndApply(
            ref Override data,
            bool overriding,
            ref TComponentSet set,
            GameObject setParent,
            string gameObjectNamePostfix,
            Action<Override, TComponentSet> before,
            Action<Override, TComponentSet> after,
            Object owner)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, overriding, owner);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(
                    ref set,
                    setParent,
                    gameObjectNamePostfix
                );
                data.Apply(set, before, after);
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
        /// <param name="gameObjectNamePostfix">The name of the set.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        /// <param name="owner"></param>
        public static void RefreshAndApply(
            ref TComponentSetData data,
            ref TComponentSet set,
            GameObject setParent,
            string gameObjectNamePostfix,
            Action<TComponentSetData, TComponentSet> before,
            Action<TComponentSetData, TComponentSet> after,
            Object owner)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, owner);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(
                    ref set,
                    setParent,
                    gameObjectNamePostfix
                );
                data.Apply(set, before, after);
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
        /// <param name="gameObjectNamePostfix">The name of the set.</param>
        /// <param name="owner"></param>
        public static void RefreshAndApply(
            ref Optional data,
            bool isElected,
            ref TComponentSet set,
            GameObject setParent,
            string gameObjectNamePostfix,
            Object owner)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, isElected, owner);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(
                    ref set,
                    setParent,
                    gameObjectNamePostfix
                );
                data.Apply(set);
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
        /// <param name="gameObjectNamePostfix">The name of the set.</param>
        /// <param name="owner"></param>
        public static void RefreshAndApply(
            ref Override data,
            bool overriding,
            ref TComponentSet set,
            GameObject setParent,
            string gameObjectNamePostfix,
            Object owner)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, overriding, owner);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(
                    ref set,
                    setParent,
                    gameObjectNamePostfix
                );
                data.Apply(set);
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
        /// <param name="gameObjectNamePostfix">The name of the set.</param>
        /// <param name="owner"></param>
        public static void RefreshAndApply(
            ref TComponentSetData data,
            ref TComponentSet set,
            GameObject setParent,
            string gameObjectNamePostfix,
            Object owner)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, owner);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(
                    ref set,
                    setParent,
                    gameObjectNamePostfix
                );
                data.Apply(set);
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
        /// <param name="setRoot">The <see cref="GameObject" /> that the set should be rooted on.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        /// <param name="owner"></param>
        public static void RefreshAndApply(
            ref Optional data,
            bool isElected,
            ref TComponentSet set,
            GameObject setRoot,
            Action<Optional, TComponentSet> before,
            Action<Optional, TComponentSet> after,
            Object owner)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, isElected, owner);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(ref set, setRoot);
                data.Apply(set, before, after);
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
        /// <param name="setRoot">The <see cref="GameObject" /> that the set should be rooted on.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        /// <param name="owner"></param>
        public static void RefreshAndApply(
            ref Override data,
            bool overriding,
            ref TComponentSet set,
            GameObject setRoot,
            Action<Override, TComponentSet> before,
            Action<Override, TComponentSet> after,
            Object owner)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, overriding, owner);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(ref set, setRoot);
                data.Apply(set, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="set" />.
        /// </summary>
        /// <remarks>The primary API for applying component set data to component sets.</remarks>
        /// <param name="data">The component set data to refresh.</param>
        /// <param name="set">The component set to apply the <paramref name="data" /> to.</param>
        /// <param name="setRoot">The <see cref="GameObject" /> that the set should be rooted on.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        /// <param name="owner"></param>
        public static void RefreshAndApply(
            ref TComponentSetData data,
            ref TComponentSet set,
            GameObject setRoot,
            Action<TComponentSetData, TComponentSet> before,
            Action<TComponentSetData, TComponentSet> after,
            Object owner)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, owner);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(ref set, setRoot);
                data.Apply(set, before, after);
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
        /// <param name="setRoot">The <see cref="GameObject" /> that the set should be rooted on.</param>
        /// <param name="owner"></param>
        public static void RefreshAndApply(
            ref Optional data,
            bool isElected,
            ref TComponentSet set,
            GameObject setRoot,
            Object owner)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, isElected, owner);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(ref set, setRoot);
                data.Apply(set);
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
        /// <param name="setRoot">The <see cref="GameObject" /> that the set should be rooted on.</param>
        /// <param name="owner"></param>
        public static void RefreshAndApply(
            ref Override data,
            bool overriding,
            ref TComponentSet set,
            GameObject setRoot,
            Object owner)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, overriding, owner);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(ref set, setRoot);
                data.Apply(set);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="set" />.
        /// </summary>
        /// <remarks>The primary API for applying component set data to component sets.</remarks>
        /// <param name="data">The component set data to refresh.</param>
        /// <param name="set">The component set to apply the <paramref name="data" /> to.</param>
        /// <param name="setRoot">The <see cref="GameObject" /> that the set should be rooted on.</param>
        /// <param name="owner"></param>
        public static void RefreshAndApply(
            ref TComponentSetData data,
            ref TComponentSet set,
            GameObject setRoot,
            Object owner)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, owner);
                ComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(ref set, setRoot);
                data.Apply(set);
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
        internal void Apply(
            TComponentSet set,
            Action<TComponentSetData, TComponentSet> before,
            Action<TComponentSetData, TComponentSet> after)
        {
            using (_PRF_Apply.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (set == null)
                        {
                            return;
                        }

                        OnApplyInternal(set, before, after);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(set, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Updates the set by applying the configuration values to the set fields.
        /// </summary>
        /// <param name="set">The set to update.</param>
        /// <param name="subscribe">Should the set be subscribed for subsequent updates?</param>
        /// <exception cref="NotSupportedException">Thrown whenever <paramref name="set" /> is null.</exception>
        internal void Apply(TComponentSet set, bool subscribe = true)
        {
            using (_PRF_Apply.Auto())
            {
                if (!subscribe)
                {
                    OnApplyInternal(set);
                    return;
                }

                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (set == null)
                        {
                            return;
                        }

                        OnApplyInternal(set);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(set, CreateSubscribableDelegate);
            }
        }

        protected abstract void OnApply(TComponentSet componentSet);

        protected abstract void OnInitializeFields(Initializer initializer, Object owner);

        [ButtonGroup(GROUP_BUTTONS)]
        protected virtual void CreateOrRefresh(Object owner)
        {
            using (_PRF_CreateOrRefresh.Auto())
            {
                if ((_owner == null) || (_owner != owner))
                {
                    _owner = owner;
                    MarkAsModified();
                }
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                InitializeFields(_owner);
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
        private void OnApplyInternal(
            TComponentSet set,
            Action<TComponentSetData, TComponentSet> before,
            Action<TComponentSetData, TComponentSet> after)
        {
            using (_PRF_OnApplyInternal.Auto())
            {
                before?.Invoke(this as TComponentSetData, set);
                OnApplyInternal(set);
                after?.Invoke(this as TComponentSetData, set);
            }
        }

        /// <summary>
        ///     Updates the set by applying the configuration values to the set fields.
        /// </summary>
        /// <param name="set">The set to update.</param>
        /// <exception cref="NotSupportedException">Thrown whenever <see cref="set" /> is null.</exception>
        private void OnApplyInternal(TComponentSet set)
        {
            using (_PRF_OnApplyInternal.Auto())
            {
                try
                {
                    ComponentDataTracker.RegisterComponentData(set, this);
                    OnApply(set);
                }
                catch (ComponentInitializationException setEx)
                {
                    Context.Log.Error(
                        string.Format(
                            UPDATE_FAILURE_LOG_FORMAT,
                            typeof(TComponentSet).FormatForLogging(),
                            setEx.Message
                        )
                    );

                    throw;
                }

                if (ShouldEnable)
                {
                    set.Enable(this as TComponentSetData);
                    set.GameObject.MarkAsShowInHierarchyAndInspector();
                }
                else
                {
                    set.Disable();
                    set.GameObject.MarkAsHideInHierarchyAndInspector();
                }
            }
        }

        /// <summary>
        ///     Updates the set by applying the configuration values to the set fields.
        /// </summary>
        /// <param name="set">The set to update.</param>
        /// <param name="delegateCreator">A delegate to create the update/subscribe delegate.</param>
        /// <exception cref="NotSupportedException">Thrown whenever <paramref name="set" /> is null.</exception>
        private void SubscribeAndApply(TComponentSet set, Func<Action> delegateCreator)
        {
            using (_PRF_SubscribeAndApply.Auto())
            {
                if (set == null)
                {
                    throw new ComponentSetNotInitializedException(
                        typeof(TComponentSet),
                        $"You must initialize the {typeof(TComponentSet).FormatForLogging()} prior to configuring it!"
                    );
                }

                _delegates ??= new();
                _delegates.SubscribeAndInvoke(set, ref Changed, delegateCreator);
            }
        }

        #region IComponentSetData<TComponentSet,TComponentSetData> Members

        public bool SharesControlError
        {
            get => _sharesControlError;
            set => _sharesControlError = value;
        }

        [ShowIf(nameof(SharesControlError))]
        [GUIColor(1f, 0f, 0f)]
        public string SharesControlWith
        {
            get => _sharesControlWith;
            set => _sharesControlWith = value;
        }

        public void InitializeFields(Object owner)
        {
            using (_PRF_InitializeFields.Auto())
            {
                _owner = owner;

                var initializer = GetInitializer();

                OnInitializeFields(initializer, owner);
            }
        }

        void IComponentData<TComponentSet>.Apply(TComponentSet comp)
        {
            using (_PRF_Apply.Auto())
            {
                Apply(comp);
            }
        }

        [ButtonGroup(GROUP_BUTTONS)]
        public void ResetData()
        {
            using (_PRF_ResetData.Auto())
            {
                InitializeFields(_owner);
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        protected static readonly ProfilerMarker _PRF_CreateOrRefresh =
            new ProfilerMarker(_PRF_PFX + nameof(CreateOrRefresh));

        private static readonly ProfilerMarker _PRF_InitializeFields =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeFields));

        protected static readonly ProfilerMarker _PRF_OnApply = new ProfilerMarker(_PRF_PFX + nameof(OnApply));

        private static readonly ProfilerMarker _PRF_OnApplyInternal =
            new ProfilerMarker(_PRF_PFX + nameof(OnApplyInternal));

        protected static readonly ProfilerMarker _PRF_OnInitializeFields =
            new ProfilerMarker(_PRF_PFX + nameof(OnInitializeFields));

        private static readonly ProfilerMarker _PRF_RefreshAndApply =
            new ProfilerMarker(_PRF_PFX + nameof(RefreshAndApply));

        private static readonly ProfilerMarker _PRF_ResetData = new ProfilerMarker(_PRF_PFX + nameof(ResetData));

        private static readonly ProfilerMarker _PRF_SubscribeAndApply =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeAndApply));

        #endregion
    }
}
