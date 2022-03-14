using System;
using Appalachia.Core.ControlModel.Controls;
using Appalachia.Core.ControlModel.Controls.Contracts;
using Appalachia.Core.ControlModel.Exceptions;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Events.Collections;
using Appalachia.Utility.Extensions;
using Unity.Profiling;

namespace Appalachia.Core.ControlModel.Extensions
{
    public static class AppaControlConfigExtensions
    {
        #region Static Fields and Autoproperties

        private static ReusableDelegateCollection<IOverridable, IAppaControl> _delegates;

        #endregion

        /// <summary>
        ///     Updates the control by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="control" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the control control is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current config to the control.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="config">The config to apply to the <see cref="control" />.</param>
        /// <param name="control">The control.</param>
        public static void Apply<TControl, TConfig>(
            this AppaControlConfig<TControl, TConfig>.Override config,
            TControl control)
            where TControl : AppaControl<TControl, TConfig>
            where TConfig : AppaControlConfig<TControl, TConfig>, new()
        {
            using (_PRF_Apply.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (config == null)
                        {
                            return;
                        }

                        if (control == null)
                        {
                            return;
                        }

                        ApplyInternal(config, control);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(config, control, CreateSubscribableDelegate);
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
        /// <param name="config">The config to apply to the <see cref="control" />.</param>
        /// <param name="control">The control.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void Apply<TControl, TConfig>(
            this AppaControlConfig<TControl, TConfig>.Optional config,
            TControl control,
            Action<AppaControlConfig<TControl, TConfig>.Optional, TControl> before,
            Action<AppaControlConfig<TControl, TConfig>.Optional, TControl> after)
            where TControl : AppaControl<TControl, TConfig>
            where TConfig : AppaControlConfig<TControl, TConfig>, new()
        {
            using (_PRF_Apply.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (config == null)
                        {
                            return;
                        }

                        if (control == null)
                        {
                            return;
                        }

                        before?.Invoke(config, control);
                        ApplyInternal(config, control);
                        after?.Invoke(config, control);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(config, control, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Updates the control by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="control" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the control control is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current config to the control.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="config">The config to apply to the <see cref="control" />.</param>
        /// <param name="control">The control.</param>
        public static void Apply<TControl, TConfig>(
            this AppaControlConfig<TControl, TConfig>.Optional config,
            TControl control)
            where TControl : AppaControl<TControl, TConfig>
            where TConfig : AppaControlConfig<TControl, TConfig>, new()
        {
            using (_PRF_Apply.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (config == null)
                        {
                            return;
                        }

                        if (control == null)
                        {
                            return;
                        }

                        ApplyInternal(config, control);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(config, control, CreateSubscribableDelegate);
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
        /// <param name="config">The config to apply to the <see cref="control" />.</param>
        /// <param name="control">The control.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void Apply<TControl, TConfig>(
            this AppaControlConfig<TControl, TConfig>.Override config,
            TControl control,
            Action<AppaControlConfig<TControl, TConfig>.Override, TControl> before,
            Action<AppaControlConfig<TControl, TConfig>.Override, TControl> after)
            where TControl : AppaControl<TControl, TConfig>
            where TConfig : AppaControlConfig<TControl, TConfig>, new()
        {
            using (_PRF_Apply.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (config == null)
                        {
                            return;
                        }

                        if (control == null)
                        {
                            return;
                        }

                        before?.Invoke(config, control);
                        ApplyInternal(config, control);
                        after?.Invoke(config, control);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(config, control, CreateSubscribableDelegate);
            }
        }

        private static void ApplyInternal<TControl, TConfig>(
            AppaControlConfig<TControl, TConfig>.Optional config,
            TControl control)
            where TControl : AppaControl<TControl, TConfig>
            where TConfig : AppaControlConfig<TControl, TConfig>, new()
        {
            using (_PRF_ApplyInternal.Auto())
            {
                //if (config.IsElected)
                {
                    var value = config.Value;

                    value.Apply(control, false);
                }

                if (config.IsElected)
                {
                    control.Enable(config);
                    control.GameObject.MarkAsShowInHierarchyAndInspector();
                }
                else
                {
                    control.Disable();
                    control.GameObject.MarkAsHideInHierarchyAndInspector();
                }
            }
        }

        private static void ApplyInternal<TControl, TConfig>(
            AppaControlConfig<TControl, TConfig>.Override config,
            TControl control)
            where TControl : AppaControl<TControl, TConfig>
            where TConfig : AppaControlConfig<TControl, TConfig>, new()
        {
            using (_PRF_ApplyInternal.Auto())
            {
                if (!config.Overriding)
                {
                    return;
                }

                var value = config.Value;

                value.Apply(control, false);

                control.Enable(config.Value);
                control.GameObject.MarkAsShowInHierarchyAndInspector();
            }
        }

        /// <summary>
        ///     Updates the control by applying the configuration values to the control fields.
        /// </summary>
        /// <param name="config">The config to apply to the control.</param>
        /// <param name="control">The control to update.</param>
        /// <param name="delegateCreator">A delegate to create the update/subscribe delegate.</param>
        /// <exception cref="NotSupportedException">Thrown whenever <paramref name="control" /> is null.</exception>
        private static void SubscribeAndApply<TControl, TConfig, TO>(
            AppaControlConfig<TControl, TConfig>.Overridable<TO> config,
            TControl control,
            Func<Action> delegateCreator)
            where TControl : AppaControl<TControl, TConfig>
            where TConfig : AppaControlConfig<TControl, TConfig>, new()
            where TO : AppaControlConfig<TControl, TConfig>.Overridable<TO>, new()
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
                _delegates.SubscribeAndInvoke(config, control, ref config.Changed, delegateCreator);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AppaControlConfigExtensions) + ".";

        private static readonly ProfilerMarker _PRF_ApplyInternal =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyInternal));

        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        private static readonly ProfilerMarker _PRF_SubscribeAndApply =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeAndApply));

        #endregion
    }
}
