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
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="component" />.
        /// </summary>
        /// <remarks>The primary API for applying component config to components.</remarks>
        /// <param name="config">The optional component config to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="component">The component to apply the <paramref name="config" /> to.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void Apply(
            Optional config,
            Object owner,
            TComponent component,
            Action<Optional, TComponent> before,
            Action<Optional, TComponent> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                config.Value.SetOwner(owner);
                config.Apply(component, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="component" />.
        /// </summary>
        /// <remarks>The primary API for applying component config to components.</remarks>
        /// <param name="config">The overridable component config to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="component">The component to apply the <paramref name="config" /> to.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void Apply(
            Override config,
            Object owner,
            TComponent component,
            Action<Override, TComponent> before,
            Action<Override, TComponent> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                config.Value.SetOwner(owner);
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
        public static void Apply(
            TConfig config,
            Object owner,
            TComponent component,
            Action<TConfig, TComponent> before,
            Action<TConfig, TComponent> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
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
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="component">The component to apply the <paramref name="config" /> to.</param>
        public static void Apply(Optional config, Object owner, TComponent component)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                config.Value.SetOwner(owner);
                config.Apply(component);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="component" />.
        /// </summary>
        /// <remarks>The primary API for applying component config to components.</remarks>
        /// <param name="config">The overridable component config to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="component">The component to apply the <paramref name="config" /> to.</param>
        public static void Apply(Override config, Object owner, TComponent component)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                config.Value.SetOwner(owner);
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
        public static void Apply(TConfig config, Object owner, TComponent component)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                config.SetOwner(owner);
                config.Apply(component);
            }
        }

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
                    TConfig value = null;
                    Refresh(ref value, owner);

                    config = new Override(overriding, value);
                }
                else if (config.Value == null)
                {
                    TConfig value = null;
                    Refresh(ref value, owner);

                    config.Value = value;
                }
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
                }
                else if (owner != null)
                {
                    config.SetOwner(owner);
                }

                config.InitializeFields(owner);
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
                    TConfig value = null;
                    Refresh(ref value, owner);

                    config = new Optional(isElected, value);
                }
                else if (config.Value == null)
                {
                    TConfig value = null;
                    Refresh(ref value, owner);

                    config.Value = value;
                }
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
        public static void RefreshAndApply(
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
                config.Value.SetOwner(owner);
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
        public static void RefreshAndApply(
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
                config.Value.SetOwner(owner);
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
        public static void RefreshAndApply(
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
        public static void RefreshAndApply(ref Optional config, bool isElected, Object owner, TComponent component)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                Refresh(ref config, isElected, owner);
                config.Value.SetOwner(owner);
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
        public static void RefreshAndApply(ref Override config, bool overriding, Object owner, TComponent component)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                Refresh(ref config, overriding, owner);
                config.Value.SetOwner(owner);
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
        public static void RefreshAndApply(ref TConfig config, Object owner, TComponent component)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                Refresh(ref config, owner);
                config.SetOwner(owner);
                config.Apply(component);
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
        internal void Apply(TComponent component, Action<TConfig, TComponent> before, Action<TConfig, TComponent> after)
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

        private void InitializeFields(Object owner)
        {
            using (_PRF_InitializeFields.Auto())
            {
                var initializer = GetInitializer();

                initializer.Do(this, nameof(_enabled), () => _enabled = true);

                OnInitializeFields(initializer, owner);
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
                        OnApply(comp);
                    }
                }
                catch (AppaInitializationException ex)
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

        #region Profiling

        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

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

        protected static readonly ProfilerMarker _PRF_SubscribeResponsiveChildren =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeResponsiveChildren));

        #endregion
    }
}
