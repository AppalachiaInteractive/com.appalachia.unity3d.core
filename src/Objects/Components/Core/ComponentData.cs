using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Objects.Components.Contracts;
using Appalachia.Core.Objects.Components.Exceptions;
using Appalachia.Core.Objects.Components.Extensions;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Events.Collections;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

// ReSharper disable StaticMemberInGenericType

namespace Appalachia.Core.Objects.Components.Core
{
    [DoNotReorderFields]
    [Serializable]
    [HideReferenceObjectPicker]
    public abstract partial class ComponentData<TComponent, TComponentData> : AppalachiaBase<TComponentData>,
                                                                              IComponentData<TComponent,
                                                                                  TComponentData>,
                                                                              IInspectorVisibility,
                                                                              IFieldLockable,
                                                                              IEnabler
        where TComponent : Component
        where TComponentData : ComponentData<TComponent, TComponentData>, new()
    {
        #region Constants and Static Readonly

        private const string UPDATE_FAILURE_LOG_FORMAT = "Failed to update the {0}: {1}";

        #endregion

        protected ComponentData()
        {
        }

        protected ComponentData(Object owner) : base(owner)
        {
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used.
        /// </summary>
        /// <param name="data">The overridable component data to refresh.</param>
        /// <param name="overriding">Whether the optional should default to overriding.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="data" /> lives within.</param>
        public static void CreateOrRefresh(ref Override data, bool overriding, Object owner)
        {
            using (_PRF_CreateOrRefresh.Auto())
            {
                if (data == null)
                {
                    TComponentData value = null;
                    CreateOrRefresh(ref value, owner);

                    data = new Override(overriding, value);
                }
                else if (data.Value == null)
                {
                    TComponentData value = null;
                    CreateOrRefresh(ref value, owner);

                    data.Value = value;
                }
            }
        }

        /// <summary>
        ///     Creates or refreshes the <see cref="data" /> to ensure it can be used.
        /// </summary>
        /// <param name="data">The component data to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="data" /> lives within.</param>
        public static void CreateOrRefresh(ref TComponentData data, Object owner)
        {
            using (_PRF_CreateOrRefresh.Auto())
            {
                if (data == default)
                {
                    data = CreateWithOwner(owner);
                }
                else if (owner != null)
                {
                    data.SetOwner(owner);
                }

                data.InitializeFields(owner);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="component" />.
        /// </summary>
        /// <remarks>The primary API for applying component data to components.</remarks>
        /// <param name="data">The optional component data to refresh.</param>
        /// <param name="isElected">Whether the optional should default to elected.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="data" /> lives within.</param>
        /// <param name="component">The component to apply the <paramref name="data" /> to.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void RefreshAndApply(
            ref Optional data,
            bool isElected,
            Object owner,
            TComponent component,
            Action<Optional, TComponent> before,
            Action<Optional, TComponent> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, isElected, owner);
                data.Value.SetOwner(owner);
                data.Apply(component, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="component" />.
        /// </summary>
        /// <remarks>The primary API for applying component data to components.</remarks>
        /// <param name="data">The overridable component data to refresh.</param>
        /// <param name="overriding">Whether the optional should default to overriding.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="data" /> lives within.</param>
        /// <param name="component">The component to apply the <paramref name="data" /> to.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void RefreshAndApply(
            ref Override data,
            bool overriding,
            Object owner,
            TComponent component,
            Action<Override, TComponent> before,
            Action<Override, TComponent> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, overriding, owner);
                data.Value.SetOwner(owner);
                data.Apply(component, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="component" />.
        /// </summary>
        /// <remarks>The primary API for applying component data to components.</remarks>
        /// <param name="data">The component data to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="data" /> lives within.</param>
        /// <param name="component">The component to apply the <paramref name="data" /> to.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void RefreshAndApply(
            ref TComponentData data,
            Object owner,
            TComponent component,
            Action<TComponentData, TComponent> before,
            Action<TComponentData, TComponent> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, owner);
                data.SetOwner(owner);
                data.Apply(component, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="component" />.
        /// </summary>
        /// <remarks>The primary API for applying component data to components.</remarks>
        /// <param name="data">The optional component data to refresh.</param>
        /// <param name="isElected">Whether the optional should default to elected.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="data" /> lives within.</param>
        /// <param name="component">The component to apply the <paramref name="data" /> to.</param>
        public static void RefreshAndApply(ref Optional data, bool isElected, Object owner, TComponent component)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, isElected, owner);
                data.Value.SetOwner(owner);
                data.Apply(component);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="component" />.
        /// </summary>
        /// <remarks>The primary API for applying component data to components.</remarks>
        /// <param name="data">The overridable component data to refresh.</param>
        /// <param name="overriding">Whether the optional should default to overriding.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="data" /> lives within.</param>
        /// <param name="component">The component to apply the <paramref name="data" /> to.</param>
        public static void RefreshAndApply(ref Override data, bool overriding, Object owner, TComponent component)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, overriding, owner);
                data.Value.SetOwner(owner);
                data.Apply(component);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="component" />.
        /// </summary>
        /// <remarks>The primary API for applying component data to components.</remarks>
        /// <param name="data">The component data to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="data" /> lives within.</param>
        /// <param name="component">The component to apply the <paramref name="data" /> to.</param>
        public static void RefreshAndApply(ref TComponentData data, Object owner, TComponent component)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                CreateOrRefresh(ref data, owner);
                data.SetOwner(owner);
                data.Apply(component);
            }
        }

        /// <summary>
        ///     Updates the component by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="component" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="before" /> function.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the component set is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current configuration data to the component.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="after" /> function.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        internal void Apply(
            TComponent component,
            Action<TComponentData, TComponent> before,
            Action<TComponentData, TComponent> after)
        {
            using (_PRF_Apply.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (component == null)
                        {
                            return;
                        }

                        OnApplyInternal(component, before, after);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(component, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Updates the component by applying the configuration values to the component fields.
        /// </summary>
        /// <param name="component">The component to update.</param>
        /// <param name="subscribe">Should the component be subscribed for subsequent updates?</param>
        /// <exception cref="NotSupportedException">Thrown whenever <paramref name="component" /> is null.</exception>
        internal void Apply(TComponent component, bool subscribe = true)
        {
            using (_PRF_Apply.Auto())
            {
                if (!subscribe)
                {
                    OnApplyInternal(component);
                    return;
                }

                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (component == null)
                        {
                            return;
                        }

                        OnApplyInternal(component);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(component, CreateSubscribableDelegate);
            }
        }

        protected abstract void OnApply(TComponent target);

        protected abstract void OnInitializeFields(Initializer initializer, Object owner);

        protected virtual void SubscribeResponsiveChildren()
        {
            using (_PRF_SubscribeResponsiveChildren.Auto())
            {
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
        ///     Updates the component by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="component" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="before" /> function.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the component set is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current configuration data to the component.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="after" /> function.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        private void OnApplyInternal(
            TComponent component,
            Action<TComponentData, TComponent> before,
            Action<TComponentData, TComponent> after)
        {
            using (_PRF_OnApplyInternal.Auto())
            {
                before?.Invoke(this as TComponentData, component);
                OnApplyInternal(component);
                after?.Invoke(this as TComponentData, component);
            }
        }

        /// <summary>
        ///     Updates the component by applying the configuration values to the component fields.
        /// </summary>
        /// <param name="comp">The component to update.</param>
        /// <exception cref="NotSupportedException">Thrown whenever <see cref="comp" /> is null.</exception>
        private void OnApplyInternal(TComponent comp)
        {
            using (_PRF_OnApplyInternal.Auto())
            {
                if (!HasInitializationStarted)
                {
                    InitializeSynchronous();
                }

                try
                {
                    ComponentDataTracker.RegisterComponentData(comp, this);
                    
                    if (Enabled)
                    {
                        if (comp is Behaviour b)
                        {
                            b.enabled = true;
                        }
                    }
                    else
                    {
                        comp.Disable();
                    }
                    
                    OnApply(comp);
                }
                catch (ComponentInitializationException ex)
                {
                    Context.Log.Error(
                        string.Format(UPDATE_FAILURE_LOG_FORMAT, typeof(TComponent).FormatForLogging(), ex.Message)
                    );

                    throw;
                }

                SubscribeResponsiveChildren();
            }
        }

        /// <summary>
        ///     Updates the component by applying the configuration values to the component fields.
        /// </summary>
        /// <param name="component">The component to update.</param>
        /// <param name="delegateCreator">A delegate to create the update/subscribe delegate.</param>
        /// <exception cref="NotSupportedException">Thrown whenever <paramref name="component" /> is null.</exception>
        private void SubscribeAndApply(TComponent component, Func<Action> delegateCreator)
        {
            using (_PRF_SubscribeAndApply.Auto())
            {
                if (component == null)
                {
                    throw new ComponentNotInitializedException(
                        typeof(TComponent),
                        $"You must initialize the {typeof(TComponent).FormatForLogging()} prior to configuring it!"
                    );
                }

                _delegates ??= new();
                _delegates.SubscribeAndInvoke(component, ref Changed, delegateCreator);
            }
        }

        #region IComponentData<TComponent,TComponentData> Members

        public void InitializeFields(Object owner)
        {
            using (_PRF_InitializeFields.Auto())
            {
                var initializer = GetInitializer();

                initializer.Do(this, nameof(_enabled), () => _enabled = true);

                OnInitializeFields(initializer, owner);
            }
        }

        void IComponentData<TComponent>.Apply(TComponent comp)
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

        #endregion

        #region IEnabler Members

        public bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }

        public Color EnabledToggleColor => Enabled ? EnabledToggleColorOn.v : EnabledToggleColorOff.v;

        #endregion

        #region IFieldLockable Members

        public bool DisableAllFields
        {
            get => _disableAllFields;
            set => _disableAllFields = value;
        }

        public bool SuspendFieldApplication
        {
            get => _suspendFieldApplication;
            set => _suspendFieldApplication = value;
        }

        public Color LockToggleColor => DisableAllFields ? LockToggleColorOn.v : LockToggleColorOff.v;

        public Color SuspendToggleColor => SuspendFieldApplication ? SuspendToggleColorOn.v : SuspendToggleColorOff.v;

        #endregion

        #region IInspectorVisibility Members

        public Color ShowToggleColor => ShowAllFields ? ShowToggleColorOn.v : ShowToggleColorOff.v;

        public Color HideToggleColor => HideAllFields ? HideToggleColorOn.v : HideToggleColorOff.v;

        public bool ShowAllFields
        {
            get => _showAllFields;
            set => _showAllFields = value;
        }

        public bool HideAllFields
        {
            get => _hideAllFields && !_showAllFields;
            set => _hideAllFields = value;
        }

        public bool ShowAdvancedOptions
        {
            get =>
                _showAdvancedOptions || HideAllFields || ShowAllFields || SuspendFieldApplication || DisableAllFields;
            set => _showAdvancedOptions = value;
        }

        public Color AdvancedToggleColor => ShowAdvancedOptions ? AdvancedToggleColorOn.v : AdvancedToggleColorOff.v;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        private static readonly ProfilerMarker _PRF_CreateOrRefresh =
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

        protected static readonly ProfilerMarker _PRF_SubscribeResponsiveChildren =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeResponsiveChildren));

        #endregion

        #region Fields and Autoproperties

        [NonSerialized] private ReusableDelegateCollection<TComponent> _delegates;

        [NonSerialized] private bool _showAllFields;

        [SerializeField, HideInInspector]
        private bool _hideAllFields;

        [SerializeField, HideInInspector]
        private bool _disableAllFields;

        [SerializeField, HideInInspector]
        private bool _suspendFieldApplication;

        [SerializeField, HideInInspector]
        private bool _showAdvancedOptions;

        [NonSerialized] private bool _sharesControlError;
        [NonSerialized] private string _sharesControlWith;

        [SerializeField, HideInInspector]
        private bool _enabled;

        #endregion

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used.
        /// </summary>
        /// <param name="data">The optional component data to refresh.</param>
        /// <param name="isElected">Whether the optional should default to elected.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="data" /> lives within.</param>
        public static void CreateOrRefresh(ref Optional data, bool isElected, Object owner)
        {
            using (_PRF_CreateOrRefresh.Auto())
            {
                if (data == null)
                {
                    TComponentData value = null;
                    CreateOrRefresh(ref value, owner);

                    data = new Optional(isElected, value);
                }
                else if (data.Value == null)
                {
                    TComponentData value = null;
                    CreateOrRefresh(ref value, owner);

                    data.Value = value;
                }
            }
        }
    }
}
