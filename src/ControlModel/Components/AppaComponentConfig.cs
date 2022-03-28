using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.ControlModel.Components.Contracts;
using Appalachia.Core.ControlModel.Exceptions;
using Appalachia.Core.ControlModel.Extensions;
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

// ReSharper disable StaticMemberInGenericType

namespace Appalachia.Core.ControlModel.Components
{
    [DoNotReorderFields]
    [Serializable]
    [HideReferenceObjectPicker]
    public abstract partial class AppaComponentConfig<TComponent, TConfig> : AppalachiaBase<TConfig>,
                                                                             IAppaComponentConfig<TComponent, TConfig>
        where TComponent : Component
        where TConfig : AppaComponentConfig<TComponent, TConfig>, new()
    {
        #region Constants and Static Readonly

        private const string UPDATE_FAILURE_LOG_FORMAT = "Failed to update the {0}: {1}";

        #endregion

        protected AppaComponentConfig()
        {
        }

        protected AppaComponentConfig(Object owner) : base(owner)
        {
        }

        #region Fields and Autoproperties

        [NonSerialized] private ReusableDelegateCollection<TComponent> _delegates;

        [NonSerialized] private bool _sharesControlError;
        [NonSerialized] private string _sharesControlWith;

        #endregion

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used.
        /// </summary>
        /// <param name="config">The overridable component config to refresh.</param>
        /// <param name="overriding">Whether the optional should default to overriding.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        public static void Refresh(ref Override config, bool overriding, Object owner)
        {
            using (_PRF_Refresh.Auto())
            {
                if (config == null)
                {
                    config = new(overriding, default);
                    owner.MarkAsModified();
                }

                Refresh(ref config.value, owner);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <see cref="config" /> to ensure it can be used.
        /// </summary>
        /// <param name="config">The component config to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        public static void Refresh(ref TConfig config, Object owner)
        {
            using (_PRF_Refresh.Auto())
            {
                if (config == default)
                {
                    config = CreateWithOwner(owner);
                    owner.MarkAsModified();
                }

                if (owner != null)
                {
                    config.SetOwner(owner);
                }

                config.InitializeFields();
                config.SubscribeResponsiveConfigs();
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used.
        /// </summary>
        /// <param name="config">The optional component config to refresh.</param>
        /// <param name="isElected">Whether the optional should default to elected.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        public static void Refresh(ref Optional config, bool isElected, Object owner)
        {
            using (_PRF_Refresh.Auto())
            {
                if (config == null)
                {
                    config = new(isElected, default);
                    owner.MarkAsModified();
                }

                Refresh(ref config.value, owner);
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
        ///             <description>Ensures the component control is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current config to the component.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="after" /> function.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public void Apply(TComponent component, Action<TConfig, TComponent> before, Action<TConfig, TComponent> after)
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
        public void Apply(TComponent component, bool subscribe = true)
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

        protected virtual void AfterApplying(TComponent component)
        {
            using (_PRF_AfterApplying.Auto())
            {
                Changed.Unsuspend();
                UnsuspendResponsiveConfigs();
            }
        }

        protected virtual void BeforeApplying(TComponent component)
        {
            using (_PRF_BeforeApplying.Auto())
            {
                SuspendResponsiveConfigs();
                Changed.Suspend();
            }
        }

        protected virtual void OnApply(TComponent component)
        {
            using (_PRF_OnApply.Auto())
            {
            }
        }

        protected virtual void OnInitializeFields(Initializer initializer)
        {
            using (_PRF_OnInitializeFields.Auto())
            {
            }
        }

        protected virtual void SubscribeResponsiveConfigs()
        {
            using (_PRF_SubscribeResponsiveConfigs.Auto())
            {
            }
        }

        protected virtual void SuspendResponsiveConfigs()
        {
            using (_PRF_SuspendResponsiveConfigs.Auto())
            {
            }
        }

        protected virtual void UnsuspendResponsiveConfigs()
        {
            using (_PRF_UnsuspendResponsiveConfigs.Auto())
            {
            }
        }

        public override void UnsuspendChanges()
        {
            using (_PRF_UnsuspendChanges.Auto())
            {
                base.UnsuspendChanges();
            
                UnsuspendResponsiveConfigs();
            }
        }

        public override void SuspendChanges()
        {
            using (_PRF_SuspendChanges.Auto())
            {
                base.SuspendChanges();
            
                SuspendResponsiveConfigs();
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                InitializeFields();
                SubscribeResponsiveConfigs();
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="component" />.
        /// </summary>
        /// <remarks>The primary API for applying component config to components.</remarks>
        /// <param name="config">The optional component config to refresh.</param>
        /// <param name="isElected">Whether the optional should default to elected.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="component">The component to apply the <paramref name="config" /> to.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        private static void RefreshAndApply(
            ref Optional config,
            bool isElected,
            Object owner,
            TComponent component,
            Action<Optional, TComponent> before,
            Action<Optional, TComponent> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                Refresh(ref config, isElected, owner);
                config.Apply(component, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="component" />.
        /// </summary>
        /// <remarks>The primary API for applying component config to components.</remarks>
        /// <param name="config">The overridable component config to refresh.</param>
        /// <param name="overriding">Whether the optional should default to overriding.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="component">The component to apply the <paramref name="config" /> to.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        private static void RefreshAndApply(
            ref Override config,
            bool overriding,
            Object owner,
            TComponent component,
            Action<Override, TComponent> before,
            Action<Override, TComponent> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                Refresh(ref config, overriding, owner);
                config.Apply(component, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="component" />.
        /// </summary>
        /// <remarks>The primary API for applying component config to components.</remarks>
        /// <param name="config">The component config to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="component">The component to apply the <paramref name="config" /> to.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        private static void RefreshAndApply(
            ref TConfig config,
            Object owner,
            TComponent component,
            Action<TConfig, TComponent> before,
            Action<TConfig, TComponent> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                Refresh(ref config, owner);
                config.SetOwner(owner);
                config.Apply(component, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="component" />.
        /// </summary>
        /// <remarks>The primary API for applying component config to components.</remarks>
        /// <param name="config">The optional component config to refresh.</param>
        /// <param name="isElected">Whether the optional should default to elected.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="component">The component to apply the <paramref name="config" /> to.</param>
        private static void RefreshAndApply(ref Optional config, bool isElected, Object owner, TComponent component)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                Refresh(ref config, isElected, owner);
                config.Apply(component);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="component" />.
        /// </summary>
        /// <remarks>The primary API for applying component config to components.</remarks>
        /// <param name="config">The overridable component config to refresh.</param>
        /// <param name="overriding">Whether the optional should default to overriding.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="component">The component to apply the <paramref name="config" /> to.</param>
        private static void RefreshAndApply(ref Override config, bool overriding, Object owner, TComponent component)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                Refresh(ref config, overriding, owner);
                config.Apply(component);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="component" />.
        /// </summary>
        /// <remarks>The primary API for applying component config to components.</remarks>
        /// <param name="config">The component config to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="component">The component to apply the <paramref name="config" /> to.</param>
        private static void RefreshAndApply(ref TConfig config, Object owner, TComponent component)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                Refresh(ref config, owner);
                config.Apply(component);
            }
        }

        private void InitializeFields()
        {
            using (_PRF_InitializeFields.Auto())
            {
                var initializer = GetInitializer();

                initializer.Do(this, nameof(_enabled), () => _enabled = true);

                OnInitializeFields(initializer);
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
        ///             <description>Ensures the component control is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current config to the component.</description>
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
            Action<TConfig, TComponent> before,
            Action<TConfig, TComponent> after)
        {
            using (_PRF_OnApplyInternal.Auto())
            {
                before?.Invoke(this as TConfig, component);
                OnApplyInternal(component);
                after?.Invoke(this as TConfig, component);
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

                if (!HasBeenInitialized)
                {
                    return;
                }

                if (Owner == null)
                {
                    Context.Log.Error($"{GetType().FormatForLogging()} owner cannot be null");
                }

                try
                {
                    AppaConfigTracker.Store(comp, this);

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

                    if (!SuspendFieldApplication)
                    {
                        BeforeApplying(comp);
                        OnApply(comp);
                        AfterApplying(comp);
                    }
                }
                catch (AppaInitializationException ex)
                {
                    Context.Log.Error(
                        string.Format(UPDATE_FAILURE_LOG_FORMAT, typeof(TComponent).FormatForLogging(), ex.Message)
                    );

                    throw;
                }

                SubscribeResponsiveConfigs();
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
                    throw new AppaComponentNotInitializedException(
                        typeof(TComponent),
                        $"You must initialize the {typeof(TComponent).FormatForLogging()} prior to configuring it!"
                    );
                }

                _delegates ??= new();
                _delegates.SubscribeAndInvoke(component, ref Changed, delegateCreator);
            }
        }

        #region IAppaComponentConfig<TComponent,TConfig> Members

        void IAppaComponentConfig<TComponent>.Apply(TComponent comp)
        {
            using (_PRF_Apply.Auto())
            {
                Apply(comp);
            }
        }

        [ButtonGroup(GROUP_BUTTONS)]
        public void ResetConfig()
        {
            using (_PRF_ResetConfig.Auto())
            {
                InitializeFields();
                SubscribeResponsiveConfigs();
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

        #region Profiling

        private static readonly ProfilerMarker _PRF_AfterApplying =
            new ProfilerMarker(_PRF_PFX + nameof(AfterApplying));

        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        private static readonly ProfilerMarker _PRF_BeforeApplying =
            new ProfilerMarker(_PRF_PFX + nameof(BeforeApplying));

        private static readonly ProfilerMarker _PRF_InitializeFields =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeFields));

        protected static readonly ProfilerMarker _PRF_OnApply = new ProfilerMarker(_PRF_PFX + nameof(OnApply));

        private static readonly ProfilerMarker _PRF_OnApplyInternal =
            new ProfilerMarker(_PRF_PFX + nameof(OnApplyInternal));

        protected static readonly ProfilerMarker _PRF_OnInitializeFields =
            new ProfilerMarker(_PRF_PFX + nameof(OnInitializeFields));

        private static readonly ProfilerMarker _PRF_Refresh = new ProfilerMarker(_PRF_PFX + nameof(Refresh));

        private static readonly ProfilerMarker _PRF_RefreshAndApply =
            new ProfilerMarker(_PRF_PFX + nameof(RefreshAndApply));

        private static readonly ProfilerMarker _PRF_ResetConfig = new ProfilerMarker(_PRF_PFX + nameof(ResetConfig));

        private static readonly ProfilerMarker _PRF_SubscribeAndApply =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeAndApply));

        protected static readonly ProfilerMarker _PRF_SubscribeResponsiveConfigs =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeResponsiveConfigs));

        protected static readonly ProfilerMarker _PRF_SuspendResponsiveConfigs =
            new ProfilerMarker(_PRF_PFX + nameof(SuspendResponsiveConfigs));

        protected static readonly ProfilerMarker _PRF_UnsuspendResponsiveConfigs =
            new ProfilerMarker(_PRF_PFX + nameof(UnsuspendResponsiveConfigs));

        #endregion
    }
}
