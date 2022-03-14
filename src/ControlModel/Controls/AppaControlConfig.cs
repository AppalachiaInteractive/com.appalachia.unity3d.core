using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.ControlModel.Components.Contracts;
using Appalachia.Core.ControlModel.Controls.Contracts;
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

namespace Appalachia.Core.ControlModel.Controls
{
    [CallStaticConstructorInEditor]
    [Serializable]
    public abstract partial class AppaControlConfig<TControl, TConfig> : AppalachiaBase<TConfig>,
                                                                         IAppaControlConfig<TControl, TConfig>
        where TControl : AppaControl<TControl, TConfig>
        where TConfig : AppaControlConfig<TControl, TConfig>, IAppaControlConfig<TControl, TConfig>, new()
    {
        protected AppaControlConfig()
        {
        }

        protected AppaControlConfig(Object owner) : base(owner)
        {
        }

        #region Constants and Static Readonly

        private const string UPDATE_FAILURE_LOG_FORMAT = "Failed to update the {0}: {1}";

        #endregion

        #region Fields and Autoproperties

        protected const int ORDER_ROOT = -250;
        
        [NonSerialized] private bool _sharesControlError;
        [NonSerialized] private string _sharesControlWith;

        [NonSerialized] private ReusableDelegateCollection<TControl> _delegates;

        #endregion

        public static string NamePostfix => typeof(TControl).Name;

        public static void Refresh(ref TConfig config, Object owner)
        {
            using (_PRF_Refresh.Auto())
            {
                config ??= new();

                config.Refresh(owner);
            }
        }

        public static void Refresh(ref Optional config, bool isElected, Object owner)
        {
            using (_PRF_Refresh.Auto())
            {
                config ??= new(isElected, new());
                config.Value ??= new();

                config.Value.Refresh(owner);
            }
        }

        public static void Refresh(ref Override config, bool overriding, Object owner)
        {
            using (_PRF_Refresh.Auto())
            {
                config ??= new(overriding, new());
                config.Value ??= new();

                config.Value.Refresh(owner);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="control" />.
        /// </summary>
        /// <remarks>The primary API for applying component control config to component controls.</remarks>
        /// <param name="config">The optional component control config to refresh.</param>
        /// <param name="isElected">Whether the optional should default to elected.</param>
        /// <param name="control">The component control to apply the <paramref name="config" /> to.</param>
        /// <param name="parent">The <see cref="GameObject" /> that the control should be a child of.</param>
        /// <param name="gameObjectNamePostfix">The name of the control.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        /// <param name="owner"></param>
        public static void RefreshAndApply(
            ref Optional config,
            bool isElected,
            ref TControl control,
            GameObject parent,
            string gameObjectNamePostfix,
            Action<Optional, TControl> before,
            Action<Optional, TControl> after,
            Object owner)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                Refresh(ref config, isElected, owner);
                AppaControl<TControl, TConfig>.Refresh(ref control, parent, gameObjectNamePostfix);
                config.Apply(control, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="control" />.
        /// </summary>
        /// <remarks>The primary API for applying component control config to component controls.</remarks>
        /// <param name="config">The overridable component control config to refresh.</param>
        /// <param name="overriding">Whether the optional should default to overriding.</param>
        /// <param name="control">The component control to apply the <paramref name="config" /> to.</param>
        /// <param name="parent">The <see cref="GameObject" /> that the control should be a child of.</param>
        /// <param name="gameObjectNamePostfix">The name of the control.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        /// <param name="owner"></param>
        public static void RefreshAndApply(
            ref Override config,
            bool overriding,
            ref TControl control,
            GameObject parent,
            string gameObjectNamePostfix,
            Action<Override, TControl> before,
            Action<Override, TControl> after,
            Object owner)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                Refresh(ref config, overriding, owner);
                AppaControl<TControl, TConfig>.Refresh(ref control, parent, gameObjectNamePostfix);
                config.Apply(control, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="control" />.
        /// </summary>
        /// <remarks>The primary API for applying component control config to component controls.</remarks>
        /// <param name="config">The component control config to refresh.</param>
        /// <param name="control">The component control to apply the <paramref name="config" /> to.</param>
        /// <param name="parent">The <see cref="GameObject" /> that the control should be a child of.</param>
        /// <param name="gameObjectNamePostfix">The name of the control.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        /// <param name="owner"></param>
        public static void RefreshAndApply(
            ref TConfig config,
            ref TControl control,
            GameObject parent,
            string gameObjectNamePostfix,
            Action<TConfig, TControl> before,
            Action<TConfig, TControl> after,
            Object owner)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                Refresh(ref config, owner);
                AppaControl<TControl, TConfig>.Refresh(ref control, parent, gameObjectNamePostfix);
                config.Apply(control, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="control" />.
        /// </summary>
        /// <remarks>The primary API for applying component control config to component controls.</remarks>
        /// <param name="config">The optional component control config to refresh.</param>
        /// <param name="isElected">Whether the optional should default to elected.</param>
        /// <param name="control">The component control to apply the <paramref name="config" /> to.</param>
        /// <param name="parent">The <see cref="GameObject" /> that the control should be a child of.</param>
        /// <param name="gameObjectNamePostfix">The name of the control.</param>
        /// <param name="owner"></param>
        public static void RefreshAndApply(
            ref Optional config,
            bool isElected,
            ref TControl control,
            GameObject parent,
            string gameObjectNamePostfix,
            Object owner)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                Refresh(ref config, isElected, owner);
                AppaControl<TControl, TConfig>.Refresh(ref control, parent, gameObjectNamePostfix);
                config.Apply(control);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="control" />.
        /// </summary>
        /// <remarks>The primary API for applying component control config to component controls.</remarks>
        /// <param name="config">The overridable component control config to refresh.</param>
        /// <param name="overriding">Whether the optional should default to overriding.</param>
        /// <param name="control">The component control to apply the <paramref name="config" /> to.</param>
        /// <param name="parent">The <see cref="GameObject" /> that the control should be a child of.</param>
        /// <param name="gameObjectNamePostfix">The name of the control.</param>
        /// <param name="owner"></param>
        public static void RefreshAndApply(
            ref Override config,
            bool overriding,
            ref TControl control,
            GameObject parent,
            string gameObjectNamePostfix,
            Object owner)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                Refresh(ref config, overriding, owner);
                AppaControl<TControl, TConfig>.Refresh(ref control, parent, gameObjectNamePostfix);
                config.Apply(control);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="control" />.
        /// </summary>
        /// <remarks>The primary API for applying component control config to component controls.</remarks>
        /// <param name="config">The component control config to refresh.</param>
        /// <param name="control">The component control to apply the <paramref name="config" /> to.</param>
        /// <param name="parent">The <see cref="GameObject" /> that the control should be a child of.</param>
        /// <param name="gameObjectNamePostfix">The name of the control.</param>
        /// <param name="owner"></param>
        public static void RefreshAndApply(
            ref TConfig config,
            ref TControl control,
            GameObject parent,
            string gameObjectNamePostfix,
            Object owner)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                Refresh(ref config, owner);
                AppaControl<TControl, TConfig>.Refresh(ref control, parent, gameObjectNamePostfix);
                config.Apply(control);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="control" />.
        /// </summary>
        /// <remarks>The primary API for applying component control config to component controls.</remarks>
        /// <param name="config">The optional component control config to refresh.</param>
        /// <param name="isElected">Whether the optional should default to elected.</param>
        /// <param name="control">The component control to apply the <paramref name="config" /> to.</param>
        /// <param name="controlRoot">The <see cref="GameObject" /> that the control should be rooted on.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        /// <param name="owner"></param>
        public static void RefreshAndApply(
            ref Optional config,
            bool isElected,
            ref TControl control,
            GameObject controlRoot,
            Action<Optional, TControl> before,
            Action<Optional, TControl> after,
            Object owner)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                Refresh(ref config, isElected, owner);
                AppaControl<TControl, TConfig>.Refresh(ref control, controlRoot);
                config.Apply(control, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="control" />.
        /// </summary>
        /// <remarks>The primary API for applying component control config to component controls.</remarks>
        /// <param name="config">The overridable component control config to refresh.</param>
        /// <param name="overriding">Whether the optional should default to overriding.</param>
        /// <param name="control">The component control to apply the <paramref name="config" /> to.</param>
        /// <param name="controlRoot">The <see cref="GameObject" /> that the control should be rooted on.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        /// <param name="owner"></param>
        public static void RefreshAndApply(
            ref Override config,
            bool overriding,
            ref TControl control,
            GameObject controlRoot,
            Action<Override, TControl> before,
            Action<Override, TControl> after,
            Object owner)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                Refresh(ref config, overriding, owner);
                AppaControl<TControl, TConfig>.Refresh(ref control, controlRoot);
                config.Apply(control, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="control" />.
        /// </summary>
        /// <remarks>The primary API for applying component control config to component controls.</remarks>
        /// <param name="config">The component control config to refresh.</param>
        /// <param name="control">The component control to apply the <paramref name="config" /> to.</param>
        /// <param name="controlRoot">The <see cref="GameObject" /> that the control should be rooted on.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        /// <param name="owner"></param>
        public static void RefreshAndApply(
            ref TConfig config,
            ref TControl control,
            GameObject controlRoot,
            Action<TConfig, TControl> before,
            Action<TConfig, TControl> after,
            Object owner)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                Refresh(ref config, owner);
                AppaControl<TControl, TConfig>.Refresh(ref control, controlRoot);
                config.Apply(control, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="control" />.
        /// </summary>
        /// <remarks>The primary API for applying component control config to component controls.</remarks>
        /// <param name="config">The optional component control config to refresh.</param>
        /// <param name="isElected">Whether the optional should default to elected.</param>
        /// <param name="control">The component control to apply the <paramref name="config" /> to.</param>
        /// <param name="controlRoot">The <see cref="GameObject" /> that the control should be rooted on.</param>
        /// <param name="owner"></param>
        public static void RefreshAndApply(
            ref Optional config,
            bool isElected,
            ref TControl control,
            GameObject controlRoot,
            Object owner)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                Refresh(ref config, isElected, owner);
                AppaControl<TControl, TConfig>.Refresh(ref control, controlRoot);
                config.Apply(control);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="control" />.
        /// </summary>
        /// <remarks>The primary API for applying component control config to component controls.</remarks>
        /// <param name="config">The overridable component control config to refresh.</param>
        /// <param name="overriding">Whether the optional should default to overriding.</param>
        /// <param name="control">The component control to apply the <paramref name="config" /> to.</param>
        /// <param name="controlRoot">The <see cref="GameObject" /> that the control should be rooted on.</param>
        /// <param name="owner"></param>
        public static void RefreshAndApply(
            ref Override config,
            bool overriding,
            ref TControl control,
            GameObject controlRoot,
            Object owner)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                Refresh(ref config, overriding, owner);
                AppaControl<TControl, TConfig>.Refresh(ref control, controlRoot);
                config.Apply(control);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="control" />.
        /// </summary>
        /// <remarks>The primary API for applying component control config to component controls.</remarks>
        /// <param name="config">The component control config to refresh.</param>
        /// <param name="control">The component control to apply the <paramref name="config" /> to.</param>
        /// <param name="controlRoot">The <see cref="GameObject" /> that the control should be rooted on.</param>
        /// <param name="owner"></param>
        public static void RefreshAndApply(
            ref TConfig config,
            ref TControl control,
            GameObject controlRoot,
            Object owner)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                Refresh(ref config, owner);
                AppaControl<TControl, TConfig>.Refresh(ref control, controlRoot);
                config.Apply(control);
            }
        }

        /// <summary>
        ///     Updates the control by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="control" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="before" /> function.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the control is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current config to the control.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="after" /> function.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public void Apply(TControl control, Action<TConfig, TControl> before, Action<TConfig, TControl> after)
        {
            using (_PRF_Apply.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (control == null)
                        {
                            return;
                        }

                        OnApplyInternal(control, before, after);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(control, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Updates the control by applying the configuration values to the control fields.
        /// </summary>
        /// <param name="control">The control to update.</param>
        /// <param name="subscribe">Should the control be subscribed for subsequent updates?</param>
        /// <exception cref="NotSupportedException">Thrown whenever <paramref name="control" /> is null.</exception>
        public void Apply(TControl control, bool subscribe = true)
        {
            using (_PRF_Apply.Auto())
            {
                if (!subscribe)
                {
                    OnApplyInternal(control);
                    return;
                }

                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (control == null)
                        {
                            return;
                        }

                        OnApplyInternal(control);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(control, CreateSubscribableDelegate);
            }
        }

        protected virtual void OnApply(TControl control)
        {
            using (_PRF_OnApply.Auto())
            {
                control.Refresh();
            }
        }

        protected abstract void OnInitializeFields(Initializer initializer, Object owner);

        [ButtonGroup(GROUP_BUTTONS)]
        protected virtual void Refresh(Object owner)
        {
            using (_PRF_Refresh.Auto())
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

        private void InitializeFields(Object owner)
        {
            using (_PRF_InitializeFields.Auto())
            {
                _owner = owner;

                var initializer = GetInitializer();

                OnInitializeFields(initializer, owner);
            }
        }

        /// <summary>
        ///     Updates the control by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="control" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="before" /> function.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the control control is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current config to the control.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="after" /> function.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        private void OnApplyInternal(
            TControl control,
            Action<TConfig, TControl> before,
            Action<TConfig, TControl> after)
        {
            using (_PRF_OnApplyInternal.Auto())
            {
                before?.Invoke(this as TConfig, control);
                OnApplyInternal(control);
                after?.Invoke(this as TConfig, control);
            }
        }

        /// <summary>
        ///     Updates the control by applying the configuration values to the control fields.
        /// </summary>
        /// <param name="control">The control to update.</param>
        /// <exception cref="NotSupportedException">Thrown whenever <see cref="control" /> is null.</exception>
        private void OnApplyInternal(TControl control)
        {
            using (_PRF_OnApplyInternal.Auto())
            {
                if (!FullyInitialized)
                {
                    return;
                }

                try
                {
                    control.Config = this as TConfig;

                    AppaConfigTracker.Store(control, this);
                    OnApply(control);
                }
                catch (AppaInitializationException controlEx)
                {
                    Context.Log.Error(
                        string.Format(UPDATE_FAILURE_LOG_FORMAT, typeof(TControl).FormatForLogging(), controlEx.Message)
                    );

                    throw;
                }

                if (ShouldEnable)
                {
                    control.Enable(this as TConfig);
                    control.GameObject.MarkAsShowInHierarchyAndInspector();
                }
                else
                {
                    control.Disable();
                    control.GameObject.MarkAsHideInHierarchyAndInspector();
                }
            }
        }

        /// <summary>
        ///     Updates the control by applying the configuration values to the control fields.
        /// </summary>
        /// <param name="control">The control to update.</param>
        /// <param name="delegateCreator">A delegate to create the update/subscribe delegate.</param>
        /// <exception cref="NotSupportedException">Thrown whenever <paramref name="control" /> is null.</exception>
        private void SubscribeAndApply(TControl control, Func<Action> delegateCreator)
        {
            using (_PRF_SubscribeAndApply.Auto())
            {
                if (control == null)
                {
                    throw new AppaControlNotInitializedException(
                        typeof(TControl),
                        $"You must initialize the {typeof(TControl).FormatForLogging()} prior to configuring it!"
                    );
                }

                _delegates ??= new();
                _delegates.SubscribeAndInvoke(control, ref Changed, delegateCreator);
            }
        }

        #region IAppaControlConfig<TControl,TConfig> Members

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

        void IAppaComponentConfig<TControl>.Apply(TControl comp)
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

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        protected static readonly ProfilerMarker _PRF_Refresh =
            new ProfilerMarker(_PRF_PFX + nameof(Refresh));

        private static readonly ProfilerMarker _PRF_InitializeFields =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeFields));

        protected static readonly ProfilerMarker _PRF_OnApply = new ProfilerMarker(_PRF_PFX + nameof(OnApply));

        private static readonly ProfilerMarker _PRF_OnApplyInternal =
            new ProfilerMarker(_PRF_PFX + nameof(OnApplyInternal));

        protected static readonly ProfilerMarker _PRF_OnInitializeFields =
            new ProfilerMarker(_PRF_PFX + nameof(OnInitializeFields));

        private static readonly ProfilerMarker _PRF_RefreshAndApply =
            new ProfilerMarker(_PRF_PFX + nameof(RefreshAndApply));

        private static readonly ProfilerMarker _PRF_ResetConfig = new ProfilerMarker(_PRF_PFX + nameof(ResetConfig));

        private static readonly ProfilerMarker _PRF_SubscribeAndApply =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeAndApply));

        #endregion
    }
}
