using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Collections;
using Appalachia.Core.ControlModel.ComponentGroups;
using Appalachia.Core.Objects.Initialization;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Appalachia.Core.ControlModel.Controls.MultiPart
{
    /// <summary>
    ///     Defines the metadata necessary for configuring a
    ///     <see
    ///         cref="BaseMultiPartControl{TControl,TConfig,TComponentGroup,TComponentGroupList,TComponentGroupConfig,TComponentGroupConfigList}" />
    ///     .
    /// </summary>
    /// <typeparam name="TControl">The control.</typeparam>
    /// <typeparam name="TConfig">Configuration for the control.</typeparam>
    /// <typeparam name="TGroup">The primary multi-part component.</typeparam>
    /// <typeparam name="TGroupList">A list of the multi-part component.</typeparam>
    /// <typeparam name="TGroupConfig">Data to configure the multi-part components.</typeparam>
    /// <typeparam name="TGroupConfigList">A list of the multi-part component data.</typeparam>
    [Serializable]
    [SmartLabelChildren]
    public abstract class BaseMultiPartControlConfig<TControl, TConfig, TGroup, TGroupList,
                                                     TGroupConfig, TGroupConfigList> :
        AppaControlConfig<TControl, TConfig>,
        IMultiPartControlConfig<TGroup, TGroupList, TGroupConfig, TGroupConfigList>
        where TControl : BaseMultiPartControl<TControl, TConfig, TGroup, TGroupList, TGroupConfig, TGroupConfigList>
        where TConfig : BaseMultiPartControlConfig<TControl, TConfig, TGroup, TGroupList, TGroupConfig,
            TGroupConfigList>, new()
        where TGroup : AppaComponentGroup<TGroup, TGroupConfig>, new()
        where TGroupList : AppaList<TGroup>, new()
        where TGroupConfig : AppaComponentGroupConfig<TGroup, TGroupConfig>, new()
        where TGroupConfigList : AppaList<TGroupConfig>, new()
    {
        #region Fields and Autoproperties

        [FormerlySerializedAs("_configs")]
        [FormerlySerializedAs("_configList")]
        [FormerlySerializedAs("_componentGroupConfigList")]
        [FormerlySerializedAs("_componentDataList")]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public TGroupConfigList configs;

        #endregion

        /// <inheritdoc />
        protected override void OnApply(TControl control)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(control);

                SyncComponentList(control);

                for (var i = 0; i < configs.Count; i++)
                {
                    var groupConfig = configs[i];
                    var group = control.GroupList[i];

                    groupConfig.Apply(group);
                }
            }
        }

        protected override void OnInitializeFields(Initializer initializer)
        {
            using (_PRF_OnInitializeFields.Auto())
            {
                InitializeElements();
            }
        }

        protected override void OnRefresh(UnityEngine.Object owner)
        {
            using (_PRF_OnRefresh.Auto())
            {
                //base.OnRefresh(owner);

                InitializeElements();
            }
        }

        protected override void SubscribeResponsiveConfigs()
        {
            using (_PRF_SubscribeResponsiveConfigs.Auto())
            {
                base.SubscribeResponsiveConfigs();

                for (var index = 0; index < configs.Count; index++)
                {
                    var groupConfig = configs[index];

                    RefreshElement(ref groupConfig);

                    groupConfig.SubscribeToChanges(OnChanged);
                }
            }
        }
        protected override void UnsuspendResponsiveConfigs()
        {
            using (_PRF_UnsuspendResponsiveConfigs.Auto())
            {
                base.UnsuspendResponsiveConfigs();

                for (var index = 0; index < configs.Count; index++)
                {
                    var groupConfig = configs[index];

                    groupConfig.UnsuspendChanges();
                }
            }
        }
        protected override void SuspendResponsiveConfigs()
        {
            using (_PRF_SuspendResponsiveConfigs.Auto())
            {
                base.SuspendResponsiveConfigs();

                for (var index = 0; index < configs.Count; index++)
                {
                    var groupConfig = configs[index];

                    groupConfig.SuspendChanges();
                }
            }
        }

        private void InitializeElements()
        {
            using (_PRF_InitializeElements.Auto())
            {
                configs ??= new TGroupConfigList();

                configs.Added += (element, _) => { RefreshElement(ref element); };

                for (var index = 0; index < configs.Count; index++)
                {
                    var groupConfig = configs[index];

                    RefreshElement(ref groupConfig);
                }
            }
        }

        private void RefreshElement(ref TGroupConfig groupConfig)
        {
            using (_PRF_RefreshElement.Auto())
            {
                AppaComponentGroupConfig<TGroup, TGroupConfig>.Refresh(ref groupConfig, Owner);

                groupConfig.ResetConfig();
            }
        }

        private void SyncComponentList(TControl control)
        {
            using (_PRF_SynComponentList.Auto())
            {
                var requiredChildCount = ConfigList.Count;

                for (var i = 0; i < control.GroupList.Count; i++)
                {
                    var current = control.GroupList[i];
                    
                    for (var j = control.GroupList.Count - 1; j >= 0; j--)
                    {
                        if (i == j) continue;

                        var comparingWith = control.GroupList[j];

                        if (current == comparingWith)
                        {
                            control.GroupList.RemoveAt(j);
                        }
                    }
                }

                var childCount = control.GroupList.Count;

                var difference = requiredChildCount - childCount;

                if (difference != 0)
                {
                    if (difference > 0)
                    {
                        for (var i = 0; i < difference; i++)
                        {
                            var prefix = AppaControlNamer.GetElementPrefix(i);
                            var parent = control.GetMultiPartParent();

                            TGroup group = null;

                            AppaComponentGroup<TGroup, TGroupConfig>.Refresh(ref group, parent, prefix);

                            control.GroupList.Add(group);
                        }
                    }
                    else
                    {
                        for (var i = childCount - 1; i >= requiredChildCount; i--)
                        {
                            control.GroupList[i].DestroySafely(true);
                            control.GroupList.RemoveAt(i);
                        }
                    }
                }

                for (var i = 0; i < control.GroupList.Count; i++)
                {
                    var prefix = AppaControlNamer.GetElementPrefix(i);
                    var parent = control.GetMultiPartParent();

                    var group = control.GroupList[i];
                    var config = ConfigList[i];

                    AppaComponentGroup<TGroup, TGroupConfig>.Refresh(ref group, parent, prefix);

                    AppaComponentGroupConfig<TGroup, TGroupConfig>.Refresh(ref config, Owner);
                }
            }
        }

        #region IMultiPartControlConfig<TGroup,TGroupList,TGroupConfig,TGroupConfigList> Members

        public TGroupConfigList ConfigList
        {
            get => configs;
            protected set => configs = value;
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_InitializeElements =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeElements));

        private static readonly ProfilerMarker _PRF_RefreshElement =
            new ProfilerMarker(_PRF_PFX + nameof(RefreshElement));

        private static readonly ProfilerMarker _PRF_SynComponentList =
            new ProfilerMarker(_PRF_PFX + nameof(SyncComponentList));

        #endregion
    }
}
