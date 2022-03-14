using System;
using Appalachia.Core.ControlModel.ComponentGroups;
using Appalachia.Core.ControlModel.ComponentGroups.Contracts;
using Appalachia.Core.ControlModel.Exceptions;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Events.Collections;
using Appalachia.Utility.Extensions;
using Unity.Profiling;

namespace Appalachia.Core.ControlModel.Extensions
{
    public static class AppaComponentGroupConfigExtensions
    {
        #region Static Fields and Autoproperties

        private static ReusableDelegateCollection<IOverridable, IAppaComponentGroup> _delegates;

        #endregion

        /// <summary>
        ///     Updates the group by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="group" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the group control is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current config to the group.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="config">The config to apply to the <see cref="group" />.</param>
        /// <param name="group">The group.</param>
        public static void Apply<TComponentGroup, TComponentGroupConfig>(
            this AppaComponentGroupConfig<TComponentGroup, TComponentGroupConfig>.Override config,
            TComponentGroup group)
            where TComponentGroup : AppaComponentGroup<TComponentGroup, TComponentGroupConfig>, new()
            where TComponentGroupConfig : AppaComponentGroupConfig<TComponentGroup, TComponentGroupConfig>, new()
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

                        if (group == null)
                        {
                            return;
                        }

                        ApplyInternal(config, group);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(config, group, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Updates the group by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="group" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="before" /> function.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the group control is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current config to the group.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="after" /> function.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="config">The config to apply to the <see cref="group" />.</param>
        /// <param name="group">The group.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void Apply<TComponentGroup, TComponentGroupConfig>(
            this AppaComponentGroupConfig<TComponentGroup, TComponentGroupConfig>.Optional config,
            TComponentGroup group,
            Action<TComponentGroupConfig, TComponentGroup> before,
            Action<TComponentGroupConfig, TComponentGroup> after)
            where TComponentGroup : AppaComponentGroup<TComponentGroup, TComponentGroupConfig>, new()
            where TComponentGroupConfig : AppaComponentGroupConfig<TComponentGroup, TComponentGroupConfig>, new()
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

                        if (group == null)
                        {
                            return;
                        }

                        before?.Invoke(config, group);
                        ApplyInternal(config, group);
                        after?.Invoke(config, group);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(config, group, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Updates the group by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="group" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the group control is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current config to the group.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="config">The config to apply to the <see cref="group" />.</param>
        /// <param name="group">The group.</param>
        public static void Apply<TComponentGroup, TComponentGroupConfig>(
            this AppaComponentGroupConfig<TComponentGroup, TComponentGroupConfig>.Optional config,
            TComponentGroup group)
            where TComponentGroup : AppaComponentGroup<TComponentGroup, TComponentGroupConfig>, new()
            where TComponentGroupConfig : AppaComponentGroupConfig<TComponentGroup, TComponentGroupConfig>, new()
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

                        if (group == null)
                        {
                            return;
                        }

                        ApplyInternal(config, group);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(config, group, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Updates the group by performing the following:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Creates or refreshes the <see cref="group" /> if necessary.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="before" /> function.</description>
        ///         </item>
        ///         <item>
        ///             <description>Ensures the group control is subscribed to configuration changes.</description>
        ///         </item>
        ///         <item>
        ///             <description>Applies the current config to the group.</description>
        ///         </item>
        ///         <item>
        ///             <description>Calls the <see cref="after" /> function.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="config">The config to apply to the <see cref="group" />.</param>
        /// <param name="group">The group.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public static void Apply<TComponentGroup, TComponentGroupConfig>(
            this AppaComponentGroupConfig<TComponentGroup, TComponentGroupConfig>.Override config,
            TComponentGroup group,
            Action<TComponentGroupConfig, TComponentGroup> before,
            Action<TComponentGroupConfig, TComponentGroup> after)
            where TComponentGroup : AppaComponentGroup<TComponentGroup, TComponentGroupConfig>, new()
            where TComponentGroupConfig : AppaComponentGroupConfig<TComponentGroup, TComponentGroupConfig>, new()
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

                        if (group == null)
                        {
                            return;
                        }

                        before?.Invoke(config, group);
                        ApplyInternal(config, group);
                        after?.Invoke(config, group);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(config, group, CreateSubscribableDelegate);
            }
        }

        private static void ApplyInternal<TComponentGroup, TComponentGroupConfig>(
            AppaComponentGroupConfig<TComponentGroup, TComponentGroupConfig>.Optional config,
            TComponentGroup group)
            where TComponentGroup : AppaComponentGroup<TComponentGroup, TComponentGroupConfig>, new()
            where TComponentGroupConfig : AppaComponentGroupConfig<TComponentGroup, TComponentGroupConfig>, new()
        {
            using (_PRF_ApplyInternal.Auto())
            {
                var value = config.Value;

                value.Apply(group, false);

                if (config.IsElected)
                {
                    group.Enable(config);
                    group.gameObject.MarkAsShowInHierarchyAndInspector();
                }
                else
                {
                    group.Disable();
                    group.gameObject.MarkAsHideInHierarchyAndInspector();
                }
            }
        }

        private static void ApplyInternal<TComponentGroup, TComponentGroupConfig>(
            AppaComponentGroupConfig<TComponentGroup, TComponentGroupConfig>.Override config,
            TComponentGroup group)
            where TComponentGroup : AppaComponentGroup<TComponentGroup, TComponentGroupConfig>, new()
            where TComponentGroupConfig : AppaComponentGroupConfig<TComponentGroup, TComponentGroupConfig>, new()
        {
            using (_PRF_ApplyInternal.Auto())
            {
                if (!config.Overriding)
                {
                    return;
                }

                var value = config.Value;

                value.Apply(group, false);
                group.Enable(config);
                group.gameObject.MarkAsShowInHierarchyAndInspector();
            }
        }

        /// <summary>
        ///     Updates the group by applying the configuration values to the group fields.
        /// </summary>
        /// <param name="config">The config to apply to the group.</param>
        /// <param name="group">The group to update.</param>
        /// <param name="delegateCreator">A delegate to create the update/subscribe delegate.</param>
        /// <exception cref="NotSupportedException">Thrown whenever <paramref name="group" /> is null.</exception>
        private static void SubscribeAndApply<TComponentGroup, TComponentGroupConfig, TO>(
            AppaComponentGroupConfig<TComponentGroup, TComponentGroupConfig>.Overridable<TO> config,
            TComponentGroup group,
            Func<Action> delegateCreator)
            where TComponentGroup : AppaComponentGroup<TComponentGroup, TComponentGroupConfig>, new()
            where TComponentGroupConfig : AppaComponentGroupConfig<TComponentGroup, TComponentGroupConfig>, new()
            where TO : AppaComponentGroupConfig<TComponentGroup, TComponentGroupConfig>.Overridable<TO>, new()
        {
            using (_PRF_SubscribeAndApply.Auto())
            {
                if (group == null)
                {
                    throw new AppaComponentGroupNotInitializedException(
                        typeof(TComponentGroup),
                        $"You must initialize the {typeof(TComponentGroup).FormatForLogging()} prior to configuring it!"
                    );
                }

                _delegates ??= new();
                _delegates.SubscribeAndInvoke(config, group, ref config.Changed, delegateCreator);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AppaComponentGroupConfigExtensions) + ".";

        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        private static readonly ProfilerMarker _PRF_ApplyInternal =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyInternal));

        private static readonly ProfilerMarker _PRF_SubscribeAndApply =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeAndApply));

        #endregion
    }
}
