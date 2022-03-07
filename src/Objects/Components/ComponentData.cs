using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Objects.Components.Contracts;
using Appalachia.Core.Objects.Components.Exceptions;
using Appalachia.Core.Objects.Components.Extensions;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Preferences;
using Appalachia.Utility.Async;
using Appalachia.Utility.Colors;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Events.Collections;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

// ReSharper disable StaticMemberInGenericType

namespace Appalachia.Core.Objects.Components
{
    [DoNotReorderFields]
    [Serializable]
    [HideReferenceObjectPicker]
    public abstract class ComponentData<TComponent, TComponentData> : AppalachiaBase<TComponentData>,
                                                                      IComponentData<TComponent, TComponentData>,
                                                                      IInspectorVisibility,
                                                                      IFieldLockable,
                                                                      IRemotelyEnabled
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

        #region Preferences

        [NonSerialized] private PREF<Color> _advancedToggleColorOff;
        [NonSerialized] private PREF<Color> _advancedToggleColorOn;

        [NonSerialized] private PREF<Color> _hideToggleColorOff;
        [NonSerialized] private PREF<Color> _hideToggleColorOn;
        [NonSerialized] private PREF<Color> _lockToggleColorOff;
        [NonSerialized] private PREF<Color> _lockToggleColorOn;
        [NonSerialized] private PREF<Color> _showToggleColorOff;
        [NonSerialized] private PREF<Color> _showToggleColorOn;
        [NonSerialized] private PREF<Color> _suspendToggleColorOff;

        [NonSerialized] private PREF<Color> _suspendToggleColorOn;

        private PREF<Color> AdvancedToggleColorOff
        {
            get
            {
                if (_advancedToggleColorOff == null)
                {
                    _advancedToggleColorOff = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(AdvancedToggleColorOff),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _advancedToggleColorOff;
            }
        }

        private PREF<Color> AdvancedToggleColorOn
        {
            get
            {
                if (_advancedToggleColorOn == null)
                {
                    _advancedToggleColorOn = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(AdvancedToggleColorOn),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _advancedToggleColorOn;
            }
        }

        private PREF<Color> HideToggleColorOff
        {
            get
            {
                if (_hideToggleColorOff == null)
                {
                    _hideToggleColorOff = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(HideToggleColorOff),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _hideToggleColorOff;
            }
        }

        private PREF<Color> HideToggleColorOn
        {
            get
            {
                if (_hideToggleColorOn == null)
                {
                    _hideToggleColorOn = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(HideToggleColorOn),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _hideToggleColorOn;
            }
        }

        private PREF<Color> LockToggleColorOff
        {
            get
            {
                if (_lockToggleColorOff == null)
                {
                    _lockToggleColorOff = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(LockToggleColorOff),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _lockToggleColorOff;
            }
        }

        private PREF<Color> LockToggleColorOn
        {
            get
            {
                if (_lockToggleColorOn == null)
                {
                    _lockToggleColorOn = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(LockToggleColorOn),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _lockToggleColorOn;
            }
        }

        private PREF<Color> ShowToggleColorOff
        {
            get
            {
                if (_showToggleColorOff == null)
                {
                    _showToggleColorOff = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(ShowToggleColorOff),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _showToggleColorOff;
            }
        }

        private PREF<Color> ShowToggleColorOn
        {
            get
            {
                if (_showToggleColorOn == null)
                {
                    _showToggleColorOn = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(ShowToggleColorOn),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _showToggleColorOn;
            }
        }

        private PREF<Color> SuspendToggleColorOff
        {
            get
            {
                if (_suspendToggleColorOff == null)
                {
                    _suspendToggleColorOff = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(SuspendToggleColorOff),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _suspendToggleColorOff;
            }
        }

        private PREF<Color> SuspendToggleColorOn
        {
            get
            {
                if (_suspendToggleColorOn == null)
                {
                    _suspendToggleColorOn = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(SuspendToggleColorOn),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _suspendToggleColorOn;
            }
        }

        #endregion

        #region Fields and Autoproperties

        private ReusableDelegateCollection<TComponent> _delegates;

        [NonSerialized] private bool _showAllFields;

        [SerializeField, HideInInspector]
        private bool _hideAllFields;

        [SerializeField, HideInInspector]
        private bool _disableAllFields;

        [SerializeField, HideInInspector]
        private bool _suspendFieldApplication;

        private Func<bool> _shouldEnableFunction;

        [SerializeField, HideInInspector]
        private bool _showAdvancedOptions;

        #endregion

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

        public override void InitializePreferences()
        {
            using (_PRF_InitializePreferences.Auto())
            {
                base.InitializePreferences();

                _hideToggleColorOff = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(HideToggleColorOff),
                    Colors.WhiteSmokeGray96
                );

                _hideToggleColorOn = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(HideToggleColorOn),
                    Colors.WhiteSmokeGray96
                );

                _lockToggleColorOff = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(LockToggleColorOff),
                    Colors.WhiteSmokeGray96
                );

                _lockToggleColorOn = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(LockToggleColorOn),
                    Colors.WhiteSmokeGray96
                );

                _showToggleColorOff = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(ShowToggleColorOff),
                    Colors.WhiteSmokeGray96
                );

                _showToggleColorOn = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(ShowToggleColorOn),
                    Colors.WhiteSmokeGray96
                );

                _suspendToggleColorOff = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(SuspendToggleColorOff),
                    Colors.WhiteSmokeGray96
                );

                _suspendToggleColorOn = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(SuspendToggleColorOn),
                    Colors.WhiteSmokeGray96
                );
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used.
        /// </summary>
        /// <param name="data">The optional component data to refresh.</param>
        /// <param name="isElected">Whether the optional should default to elected.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="data" /> lives within.</param>
        internal static void CreateOrRefresh(ref Optional data, bool isElected, Object owner)
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

        /// <summary>
        ///     Creates or refreshes the <paramref name="data" /> to ensure it can be used.
        /// </summary>
        /// <param name="data">The overridable component data to refresh.</param>
        /// <param name="overriding">Whether the optional should default to overriding.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="data" /> lives within.</param>
        internal static void CreateOrRefresh(ref Override data, bool overriding, Object owner)
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
        internal static void CreateOrRefresh(ref TComponentData data, Object owner)
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
                    OnApply(comp);
                }
                catch (ComponentInitializationException ex)
                {
                    Context.Log.Error(
                        string.Format(UPDATE_FAILURE_LOG_FORMAT, typeof(TComponent).FormatForLogging(), ex.Message)
                    );

                    throw;
                }

                if (ShouldEnable)
                {
                    comp.Enable(this as TComponentData);
                }
                else
                {
                    comp.Disable();
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
            get => _showAdvancedOptions || HideAllFields || ShowAllFields || SuspendFieldApplication || DisableAllFields;
            set => _showAdvancedOptions = value;
        }

        public Color AdvancedToggleColor => ShowAdvancedOptions ? AdvancedToggleColorOn.v : AdvancedToggleColorOff.v;

        #endregion

        #region IRemotelyEnabled Members

        public Func<bool> ShouldEnableFunction
        {
            get => _shouldEnableFunction;
            set => _shouldEnableFunction = value;
        }

        public bool ShouldEnable => _shouldEnableFunction?.Invoke() ?? true;

        public void BindEnabledStateTo(IRemotelyEnabledController controller)
        {
            using (_PRF_BindEnabledStateTo.Auto())
            {
                ShouldEnableFunction = () => controller.ShouldEnable;
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        private static readonly ProfilerMarker _PRF_BindEnabledStateTo =
            new ProfilerMarker(_PRF_PFX + nameof(BindEnabledStateTo));

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
    }
}
