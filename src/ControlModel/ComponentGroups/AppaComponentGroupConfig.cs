using System;
using System.Collections.Generic;
using Appalachia.Core.ControlModel.ComponentGroups.Contracts;
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

namespace Appalachia.Core.ControlModel.ComponentGroups
{
    /// <summary>
    ///     Config for a
    ///     <see cref="AppaComponentGroup{TComponentGroup,TComponentGroupConfig}" />
    ///     .
    /// </summary>
    /// <typeparam name="TGroup">The type of group this configures.</typeparam>
    /// <typeparam name="TConfig">This type.</typeparam>
    [Serializable]
    public abstract partial class AppaComponentGroupConfig<TGroup, TConfig> : AppalachiaBase<TConfig>,
                                                                              IAppaComponentGroupConfig<TGroup, TConfig>
        where TGroup : AppaComponentGroup<TGroup, TConfig>
        where TConfig : AppaComponentGroupConfig<TGroup, TConfig>, new()
    {
        #region Constants and Static Readonly

        private const string UPDATE_FAILURE_LOG_FORMAT = "Failed to update the {0}: {1}";

        #endregion

        protected AppaComponentGroupConfig()
        {
        }

        protected AppaComponentGroupConfig(Object owner) : base(owner)
        {
        }

        #region Fields and Autoproperties

        [NonSerialized] private ReusableDelegateCollection<TGroup> _delegates;

        [NonSerialized] private bool _sharesControlError;
        [NonSerialized] private string _sharesControlWith;
        [NonSerialized] private IReadOnlyList<IAppaComponentConfig> _componentConfigs;

        #endregion

        protected virtual string NamePostfix => typeof(TGroup).Name;

        /// <summary>
        ///     Creates or refreshes the <see cref="config" /> to ensure they are ready to use.
        /// </summary>
        /// <param name="config">The config group.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        public static void Refresh(ref TConfig config, Object owner)
        {
            using (_PRF_Refresh.Auto())
            {
                if (config == null)
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
        ///     Creates or refreshes the <paramref name="config" /> to ensure it is ready to use.
        /// </summary>
        /// <param name="config">The config group.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        public static void Refresh(ref Optional config, Object owner)
        {
            using (_PRF_Refresh.Auto())
            {
                if (config == null)
                {
                    config = new(false, default);
                    owner.MarkAsModified();
                }

                Refresh(ref config.value, owner);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it is ready to use.
        /// </summary>
        /// <param name="config">The config group.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        public static void Refresh(ref Override config, Object owner)
        {
            using (_PRF_Refresh.Auto())
            {
                if (config == null)
                {
                    config = new(false, default);
                    owner.MarkAsModified();
                }

                Refresh(ref config.value, owner);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> and <paramref name="group" /> to ensure they are ready to use.
        /// </summary>
        /// <param name="config">The config group.</param>
        /// <param name="group">The group.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="root"></param>
        public static void Refresh(ref Optional config, ref TGroup group, Object owner, GameObject root)
        {
            using (_PRF_Refresh.Auto())
            {
                AppaComponentGroup<TGroup, TConfig>.Refresh(ref group, root);

                if (config == null)
                {
                    config = new(false, default);
                }

                Refresh(ref config.value, owner);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> and <paramref name="group" /> to ensure they are ready to use.
        /// </summary>
        /// <param name="config">The config group.</param>
        /// <param name="group">The group.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="root"></param>
        public static void Refresh(ref Override config, ref TGroup group, Object owner, GameObject root)
        {
            using (_PRF_Refresh.Auto())
            {
                AppaComponentGroup<TGroup, TConfig>.Refresh(ref group, root);

                if (config == null)
                {
                    config = new(false, default);
                }

                Refresh(ref config.value, owner);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> and <paramref name="group" /> to ensure they are ready to use.
        /// </summary>
        /// <param name="config">The config group.</param>
        /// <param name="group">The group.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="root"></param>
        public static void Refresh(ref TConfig config, ref TGroup group, Object owner, GameObject root)
        {
            using (_PRF_Refresh.Auto())
            {
                AppaComponentGroup<TGroup, TConfig>.Refresh(ref group, root);
                Refresh(ref config, owner);
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
        /// <param name="group">The group.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        public void Apply(TGroup group, Action<TConfig, TGroup> before, Action<TConfig, TGroup> after)
        {
            using (_PRF_Apply.Auto())
            {
                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (group == null)
                        {
                            return;
                        }

                        OnApplyInternal(group, before, after);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(group, CreateSubscribableDelegate);
            }
        }

        /// <summary>
        ///     Updates the group by applying the configuration values to the group fields.
        /// </summary>
        /// <param name="group">The group to update.</param>
        /// <param name="subscribe">Should the group be subscribed for subsequent updates?</param>
        /// <exception cref="NotSupportedException">Thrown whenever <paramref name="group" /> is null.</exception>
        public void Apply(TGroup group, bool subscribe = true)
        {
            using (_PRF_Apply.Auto())
            {
                if (!subscribe)
                {
                    OnApplyInternal(group);
                    return;
                }

                Action CreateSubscribableDelegate()
                {
                    void SubscribableDelegate()
                    {
                        if (group == null)
                        {
                            return;
                        }

                        OnApplyInternal(group);
                    }

                    return SubscribableDelegate;
                }

                SubscribeAndApply(group, CreateSubscribableDelegate);
            }
        }

        protected abstract void CollectConfigs(List<IAppaComponentConfig> configs);

        //protected abstract void OnInitializeFields(Initializer initializer, Object owner);

        /// <summary>
        ///     Creates or refreshes this config group to ensure it is ready to use.
        /// </summary>
        /// <param name="initializer"></param>

        // ReSharper disable once UnusedParameter.Global
        protected abstract void OnInitializeFields(Initializer initializer);

        protected virtual void AfterApplying(TGroup group)
        {
            using (_PRF_AfterApplying.Auto())
            {
                Changed.Unsuspend();
                UnsuspendResponsiveConfigs();
            }
        }

        protected virtual void BeforeApplying(TGroup group)
        {
            using (_PRF_BeforeApplying.Auto())
            {
                group.Config = this as TConfig;
                Changed.Suspend();
                SuspendResponsiveConfigs();
                group.Refresh();
            }
        }

        protected virtual void OnApply(TGroup group)
        {
            using (_PRF_OnApply.Auto())
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
        ///     then applies it to the <paramref name="group" />.
        /// </summary>
        /// <remarks>The primary API for applying component group config to component groups.</remarks>
        /// <param name="config">The component group config to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="group">The component group to apply the <paramref name="config" /> to.</param>
        /// <param name="parent">The <see cref="GameObject" /> that the group should be a child of.</param>
        /// <param name="namePrefix">The name of the group.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        private static void RefreshAndApply(
            ref Override config,
            Object owner,
            ref TGroup group,
            GameObject parent,
            string namePrefix,
            Action<TConfig, TGroup> before,
            Action<TConfig, TGroup> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                AppaComponentGroup<TGroup, TConfig>.Refresh(ref group, parent, namePrefix);
                Refresh(ref config, owner);
                config.Apply(group, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="group" />.
        /// </summary>
        /// <remarks>The primary API for applying component group config to component groups.</remarks>
        /// <param name="config">The component group config to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="group">The component group to apply the <paramref name="config" /> to.</param>
        /// <param name="parent">The <see cref="GameObject" /> that the group should be a child of.</param>
        /// <param name="namePrefix">The name of the group.</param>
        private static void RefreshAndApply(
            ref Override config,
            Object owner,
            ref TGroup group,
            GameObject parent,
            string namePrefix)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                AppaComponentGroup<TGroup, TConfig>.Refresh(ref group, parent, namePrefix);
                Refresh(ref config, owner);
                config.Apply(group);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="group" />.
        /// </summary>
        /// <remarks>The primary API for applying component group config to component groups.</remarks>
        /// <param name="config">The component group config to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="group">The component group to apply the <paramref name="config" /> to.</param>
        /// <param name="parent">The <see cref="GameObject" /> that the group should be a child of.</param>
        /// <param name="namePrefix">The name of the group.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        private static void RefreshAndApply(
            ref Optional config,
            Object owner,
            ref TGroup group,
            GameObject parent,
            string namePrefix,
            Action<TConfig, TGroup> before,
            Action<TConfig, TGroup> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                AppaComponentGroup<TGroup, TConfig>.Refresh(ref group, parent, namePrefix);
                Refresh(ref config, owner);
                config.Apply(group, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="group" />.
        /// </summary>
        /// <remarks>The primary API for applying component group config to component groups.</remarks>
        /// <param name="config">The component group config to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="group">The component group to apply the <paramref name="config" /> to.</param>
        /// <param name="parent">The <see cref="GameObject" /> that the group should be a child of.</param>
        /// <param name="namePrefix">The name of the group.</param>
        private static void RefreshAndApply(
            ref Optional config,
            Object owner,
            ref TGroup group,
            GameObject parent,
            string namePrefix)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                AppaComponentGroup<TGroup, TConfig>.Refresh(ref group, parent, namePrefix);
                Refresh(ref config, owner);
                config.Apply(group);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="group" />.
        /// </summary>
        /// <remarks>The primary API for applying component group config to component groups.</remarks>
        /// <param name="config">The component group config to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="group">The component group to apply the <paramref name="config" /> to.</param>
        /// <param name="parent">The <see cref="GameObject" /> that the group should be a child of.</param>
        /// <param name="namePrefix">The name of the group.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        private static void RefreshAndApply(
            ref TConfig config,
            Object owner,
            ref TGroup group,
            GameObject parent,
            string namePrefix,
            Action<TConfig, TGroup> before,
            Action<TConfig, TGroup> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                AppaComponentGroup<TGroup, TConfig>.Refresh(ref group, parent, namePrefix);
                Refresh(ref config, owner);
                config.SetOwner(owner);
                config.Apply(group, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="group" />.
        /// </summary>
        /// <remarks>The primary API for applying component group config to component groups.</remarks>
        /// <param name="config">The component group config to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="group">The component group to apply the <paramref name="config" /> to.</param>
        /// <param name="parent">The <see cref="GameObject" /> that the group should be a child of.</param>
        /// <param name="namePrefix">The name of the group.</param>
        private static void RefreshAndApply(
            ref TConfig config,
            Object owner,
            ref TGroup group,
            GameObject parent,
            string namePrefix)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                AppaComponentGroup<TGroup, TConfig>.Refresh(ref group, parent, namePrefix);
                Refresh(ref config, owner);
                config.Apply(group);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="group" />.
        /// </summary>
        /// <remarks>The primary API for applying component group config to component groups.</remarks>
        /// <param name="config">The component group config to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="group">The component group to apply the <paramref name="config" /> to.</param>
        /// <param name="groupObject">The <see cref="GameObject" /> that the group should be a child of.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        private static void RefreshAndApply(
            ref Override config,
            Object owner,
            ref TGroup group,
            GameObject groupObject,
            Action<TConfig, TGroup> before,
            Action<TConfig, TGroup> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                AppaComponentGroup<TGroup, TConfig>.Refresh(ref group, groupObject);
                Refresh(ref config, owner);
                config.Apply(group, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="group" />.
        /// </summary>
        /// <remarks>The primary API for applying component group config to component groups.</remarks>
        /// <param name="config">The component group config to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="group">The component group to apply the <paramref name="config" /> to.</param>
        /// <param name="groupObject">The <see cref="GameObject" /> that the group should be a child of.</param>
        private static void RefreshAndApply(ref Override config, Object owner, ref TGroup group, GameObject groupObject)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                AppaComponentGroup<TGroup, TConfig>.Refresh(ref group, groupObject);
                Refresh(ref config, owner);
                config.Apply(group);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="group" />.
        /// </summary>
        /// <remarks>The primary API for applying component group config to component groups.</remarks>
        /// <param name="config">The component group config to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="group">The component group to apply the <paramref name="config" /> to.</param>
        /// <param name="groupObject">The <see cref="GameObject" /> that the group should be a child of.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        private static void RefreshAndApply(
            ref Optional config,
            Object owner,
            ref TGroup group,
            GameObject groupObject,
            Action<TConfig, TGroup> before,
            Action<TConfig, TGroup> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                AppaComponentGroup<TGroup, TConfig>.Refresh(ref group, groupObject);
                Refresh(ref config, owner);
                config.Apply(group, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="group" />.
        /// </summary>
        /// <remarks>The primary API for applying component group config to component groups.</remarks>
        /// <param name="config">The component group config to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="group">The component group to apply the <paramref name="config" /> to.</param>
        /// <param name="groupObject">The <see cref="GameObject" /> that the group should be a child of.</param>
        private static void RefreshAndApply(ref Optional config, Object owner, ref TGroup group, GameObject groupObject)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                AppaComponentGroup<TGroup, TConfig>.Refresh(ref group, groupObject);
                Refresh(ref config, owner);
                config.Apply(group);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="group" />.
        /// </summary>
        /// <remarks>The primary API for applying component group config to component groups.</remarks>
        /// <param name="config">The component group config to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="group">The component group to apply the <paramref name="config" /> to.</param>
        /// <param name="groupObject">The <see cref="GameObject" /> that the group should be a child of.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        private static void RefreshAndApply(
            ref TConfig config,
            Object owner,
            ref TGroup group,
            GameObject groupObject,
            Action<TConfig, TGroup> before,
            Action<TConfig, TGroup> after)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                AppaComponentGroup<TGroup, TConfig>.Refresh(ref group, groupObject);
                Refresh(ref config, owner);
                config.SetOwner(owner);
                config.Apply(group, before, after);
            }
        }

        /// <summary>
        ///     Creates or refreshes the <paramref name="config" /> to ensure it can be used, and
        ///     then applies it to the <paramref name="group" />.
        /// </summary>
        /// <remarks>The primary API for applying component group config to component groups.</remarks>
        /// <param name="config">The component group config to refresh.</param>
        /// <param name="owner">The serializable <see cref="UnityEngine.Object" /> that this <paramref name="config" /> lives within.</param>
        /// <param name="group">The component group to apply the <paramref name="config" /> to.</param>
        /// <param name="groupObject">The <see cref="GameObject" /> that the group should be a child of.</param>
        private static void RefreshAndApply(ref TConfig config, Object owner, ref TGroup group, GameObject groupObject)
        {
            using (_PRF_RefreshAndApply.Auto())
            {
                AppaComponentGroup<TGroup, TConfig>.Refresh(ref group, groupObject);
                Refresh(ref config, owner);
                config.Apply(group);
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
        /// <param name="group">The group.</param>
        /// <param name="before">A function to execute prior to updating.</param>
        /// <param name="after">A function to execute after finishing the update.</param>
        private void OnApplyInternal(TGroup group, Action<TConfig, TGroup> before, Action<TConfig, TGroup> after)
        {
            using (_PRF_OnApplyInternal.Auto())
            {
                before?.Invoke(this as TConfig, group);
                OnApplyInternal(group);
                after?.Invoke(this as TConfig, group);
            }
        }

        /// <summary>
        ///     Updates the group by applying the configuration values to the group fields.
        /// </summary>
        /// <param name="group">The group to update.</param>
        /// <exception cref="NotSupportedException">Thrown whenever <see cref="group" /> is null.</exception>
        private void OnApplyInternal(TGroup group)
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
                    group.Config = this as TConfig;
                    AppaConfigTracker.Store(group, this);
                    BeforeApplying(group);
                    OnApply(group);
                    AfterApplying(group);
                }
                catch (AppaInitializationException ex)
                {
                    Context.Log.Error(
                        string.Format(UPDATE_FAILURE_LOG_FORMAT, typeof(TGroup).FormatForLogging(), ex.Message)
                    );

                    throw;
                }

                if (ShouldEnable)
                {
                    group.Enable(this as TConfig);
                }
                else
                {
                    group.Disable();
                }

                group.gameObject.MarkAsShowInHierarchyAndInspector();
            }
        }

        /// <summary>
        ///     Updates the group by applying the configuration values to the group fields.
        /// </summary>
        /// <param name="group">The group to update.</param>
        /// <param name="delegateCreator">A delegate to create the update/subscribe delegate.</param>
        /// <exception cref="NotSupportedException">Thrown whenever <paramref name="group" /> is null.</exception>
        private void SubscribeAndApply(TGroup group, Func<Action> delegateCreator)
        {
            using (_PRF_SubscribeAndApply.Auto())
            {
                if (group == null)
                {
                    throw new AppaComponentGroupNotInitializedException(
                        typeof(TGroup),
                        $"You must initialize the {typeof(TGroup).FormatForLogging()} prior to configuring it!"
                    );
                }

                _delegates ??= new();
                _delegates.SubscribeAndInvoke(group, ref Changed, delegateCreator);
            }
        }

        #region IAppaComponentGroupConfig<TGroup,TConfig> Members

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

        void IAppaComponentConfig<TGroup>.Apply(TGroup group)
        {
            using (_PRF_UpdateComponent.Auto())
            {
                BeforeApplying(group);
                OnApply(group);
                AfterApplying(group);
            }
        }

        [ButtonGroup(GROUP_BUTTONS)]
        public virtual void ResetConfig()
        {
            using (_PRF_ResetConfig.Auto())
            {
                InitializeFields();
                SubscribeResponsiveConfigs();
            }
        }

        public IReadOnlyList<IAppaComponentConfig> ComponentConfigs
        {
            get
            {
                if ((_componentConfigs == null) || (_componentConfigs.Count == 0))
                {
                    var list = new List<IAppaComponentConfig>();

                    CollectConfigs(list);

                    _componentConfigs = list;
                }

                return _componentConfigs;
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_AfterApplying =
            new ProfilerMarker(_PRF_PFX + nameof(AfterApplying));

        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        private static readonly ProfilerMarker _PRF_BeforeApplying =
            new ProfilerMarker(_PRF_PFX + nameof(BeforeApplying));

        protected static readonly ProfilerMarker _PRF_CollectConfigs =
            new ProfilerMarker(_PRF_PFX + nameof(CollectConfigs));

        private static readonly ProfilerMarker _PRF_InitializeFields =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeFields));

        protected static readonly ProfilerMarker _PRF_OnApply = new ProfilerMarker(_PRF_PFX + nameof(OnApply));

        private static readonly ProfilerMarker _PRF_OnApplyInternal =
            new ProfilerMarker(_PRF_PFX + nameof(OnApplyInternal));

        protected static readonly ProfilerMarker _PRF_Refresh =
            new ProfilerMarker(_PRF_PFX + nameof(OnInitializeFields));

        private static readonly ProfilerMarker _PRF_RefreshAndApply =
            new ProfilerMarker(_PRF_PFX + nameof(RefreshAndApply));

        protected static readonly ProfilerMarker _PRF_ResetConfig = new ProfilerMarker(_PRF_PFX + nameof(ResetConfig));

        private static readonly ProfilerMarker _PRF_SubscribeAndApply =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeAndApply));

        protected static readonly ProfilerMarker _PRF_SubscribeResponsiveConfigs =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeResponsiveConfigs));

        protected static readonly ProfilerMarker _PRF_SuspendResponsiveConfigs =
            new ProfilerMarker(_PRF_PFX + nameof(SuspendResponsiveConfigs));

        protected static readonly ProfilerMarker _PRF_UnsuspendResponsiveConfigs =
            new ProfilerMarker(_PRF_PFX + nameof(UnsuspendResponsiveConfigs));

        private static readonly ProfilerMarker _PRF_UpdateComponent =
            new ProfilerMarker(_PRF_PFX + nameof(IAppaComponentConfig<TGroup>.Apply));

        #endregion
    }
}
