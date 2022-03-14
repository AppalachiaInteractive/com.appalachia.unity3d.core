using System;
using Appalachia.Core.ControlModel.Components;
using Appalachia.Core.ControlModel.Exceptions;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Events.Collections;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.ControlModel.Extensions
{
    public static class AppaComponentConfigExtensions
    {
        #region Static Fields and Autoproperties

        private static ReusableDelegateCollection<IOverridable, Component> _delegates;

        #endregion

        /// <summary>
        ///     Updates the component by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="component" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the component control is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current config to the component.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="config">The config to apply to the <see cref="component" />.</param>
        /// <param name="component">The component.</param>
        public static void Apply<TComponent, TComponentConfig>(
            this AppaComponentConfig<TComponent, TComponentConfig>.Override config,
            TComponent component)
            where TComponent : Component
            where TComponentConfig : AppaComponentConfig<TComponent, TComponentConfig>, new()
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

                        if (component == null)
                        {
                            return;
                        }

                        ApplyInternal(config, component);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(config, component, CreateSubscribableDelegate);
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
        /// <param name="config">The config to apply to the <see cref="component" />.</param>
        /// <param name="component">The component.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void Apply<TComponent, TComponentConfig>(
            this AppaComponentConfig<TComponent, TComponentConfig>.Optional config,
            TComponent component,
            Action<AppaComponentConfig<TComponent, TComponentConfig>.Optional, TComponent> before,
            Action<AppaComponentConfig<TComponent, TComponentConfig>.Optional, TComponent> after)
            where TComponent : Component
            where TComponentConfig : AppaComponentConfig<TComponent, TComponentConfig>, new()
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

                        if (component == null)
                        {
                            return;
                        }

                        before?.Invoke(config, component);
                        ApplyInternal(config, component);
                        after?.Invoke(config, component);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(config, component, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Updates the component by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="component" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the component control is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current config to the component.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="config">The config to apply to the <see cref="component" />.</param>
        /// <param name="component">The component.</param>
        public static void Apply<TComponent, TComponentConfig>(
            this AppaComponentConfig<TComponent, TComponentConfig>.Optional config,
            TComponent component)
            where TComponent : Component
            where TComponentConfig : AppaComponentConfig<TComponent, TComponentConfig>, new()
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

                        if (component == null)
                        {
                            return;
                        }

                        ApplyInternal(config, component);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(config, component, CreateSubscribableDelegate);
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
        /// <param name="config">The config to apply to the <see cref="component" />.</param>
        /// <param name="component">The component.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void Apply<TComponent, TComponentConfig>(
            this AppaComponentConfig<TComponent, TComponentConfig>.Override config,
            TComponent component,
            Action<AppaComponentConfig<TComponent, TComponentConfig>.Override, TComponent> before,
            Action<AppaComponentConfig<TComponent, TComponentConfig>.Override, TComponent> after)
            where TComponent : Component
            where TComponentConfig : AppaComponentConfig<TComponent, TComponentConfig>, new()
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

                        if (component == null)
                        {
                            return;
                        }

                        before?.Invoke(config, component);
                        ApplyInternal(config, component);
                        after?.Invoke(config, component);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(config, component, CreateSubscribableDelegate);
            }
        }

        private static void ApplyInternal<TComponent, TComponentConfig>(
            AppaComponentConfig<TComponent, TComponentConfig>.Optional config,
            TComponent component)
            where TComponent : Component
            where TComponentConfig : AppaComponentConfig<TComponent, TComponentConfig>, new()
        {
            using (_PRF_ApplyInternal.Auto())
            {
                var value = config.Value;

                value.Apply(component, false);

                if (config.IsElected)
                {
                    if (component is Behaviour behaviour)
                    {
                        behaviour.enabled = true;
                    }

                    component.MarkAsShowInHierarchyAndInspector();
                }
                else
                {
                    component.MarkAsHideInHierarchyAndInspector();

                    if (component is Behaviour behaviour)
                    {
                        behaviour.enabled = false;
                    }
                }
            }
        }

        private static void ApplyInternal<TComponent, TComponentConfig>(
            AppaComponentConfig<TComponent, TComponentConfig>.Override config,
            TComponent component)
            where TComponent : Component
            where TComponentConfig : AppaComponentConfig<TComponent, TComponentConfig>, new()
        {
            using (_PRF_ApplyInternal.Auto())
            {
                if (!config.Overriding)
                {
                    return;
                }

                var value = config.Value;

                value.Apply(component, false);
            }
        }

        /// <summary>
        ///     Updates the component by applying the configuration values to the component fields.
        /// </summary>
        /// <param name="config">The config to apply to the component.</param>
        /// <param name="component">The component to update.</param>
        /// <param name="delegateCreator">A delegate to create the update/subscribe delegate.</param>
        /// <exception cref="NotSupportedException">Thrown whenever <paramref name="component" /> is null.</exception>
        private static void SubscribeAndApply<TComponent, TComponentConfig, TO>(
            AppaComponentConfig<TComponent, TComponentConfig>.Overridable<TO> config,
            TComponent component,
            Func<Action> delegateCreator)
            where TComponent : Component
            where TComponentConfig : AppaComponentConfig<TComponent, TComponentConfig>, new()
            where TO : AppaComponentConfig<TComponent, TComponentConfig>.Overridable<TO>, new()
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
                _delegates.SubscribeAndInvoke(config, component, ref config.Changed, delegateCreator);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AppaComponentConfigExtensions) + ".";

        private static readonly ProfilerMarker _PRF_ApplyInternal =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyInternal));

        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        private static readonly ProfilerMarker _PRF_SubscribeAndApply =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeAndApply));

        #endregion
    }
}
