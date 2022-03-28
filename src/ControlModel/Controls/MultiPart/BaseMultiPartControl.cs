using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Collections;
using Appalachia.Core.ControlModel.ComponentGroups;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Core.ControlModel.Controls.MultiPart
{
    /// <summary>
    ///     Defines the members necessary for creating and configuring
    ///     the components of a multi-part control.
    /// </summary>
    /// <typeparam name="TControl">The control.</typeparam>
    /// <typeparam name="TConfig">Configuration for the control.</typeparam>
    /// <typeparam name="TGroup">The primary multi-part component.</typeparam>
    /// <typeparam name="TGroupList">A list of the multi-part component.</typeparam>
    /// <typeparam name="TGroupConfig">Data to configure the multi-part components.</typeparam>
    /// <typeparam name="TGroupConfigList">A list of the multi-part component data.</typeparam>
    [Serializable]
    [SmartLabelChildren]
    public abstract class BaseMultiPartControl<TControl, TConfig, TGroup, TGroupList, TGroupConfig, TGroupConfigList> :
        AppaControl<TControl, TConfig>,
        IMultiPartControl<TGroup, TGroupList, TGroupConfig, TGroupConfigList>
        where TControl : BaseMultiPartControl<TControl, TConfig, TGroup, TGroupList, TGroupConfig, TGroupConfigList>
        where TConfig : BaseMultiPartControlConfig<TControl, TConfig, TGroup, TGroupList, TGroupConfig,
            TGroupConfigList>, new()
        where TGroup : AppaComponentGroup<TGroup, TGroupConfig>, new()
        where TGroupList : AppaList<TGroup>, new()
        where TGroupConfig : AppaComponentGroupConfig<TGroup, TGroupConfig>, new()
        where TGroupConfigList : AppaList<TGroupConfig>, new()
    {
        #region Fields and Autoproperties

        [FoldoutGroup(GROUP_COMP)]
        [FormerlySerializedAs("_groupList")]
        [FormerlySerializedAs("_subsetList")]
        [FormerlySerializedAs("_componentList")]
        [PropertyOrder(-230)]
        [SerializeField]
        [ReadOnly]
        private TGroupList _groups;

        #endregion

        public abstract GameObject GetMultiPartParent();

        /// <inheritdoc />
        public override void DestroySafely()
        {
            using (_PRF_DestroySafely.Auto())
            {
                CleanComponentList();

                for (var i = 0; i < _groups.Count; i++)
                {
                    _groups[i].DestroySafely(true);
                }

                _groups.Clear();
            }
        }

        /// <inheritdoc />
        public override void Disable()
        {
            using (_PRF_Disable.Auto())
            {
                base.Disable();

                CleanComponentList();

                for (var i = 0; i < _groups.Count; i++)
                {
                    _groups[i].Disable();
                }
            }
        }

        /// <inheritdoc />
        public override void Enable(TConfig config)
        {
            using (_PRF_Enable.Auto())
            {
                base.Enable(config);

                CleanComponentList();

                for (var i = 0; i < _groups.Count; i++)
                {
                    var groupConfig = config.ConfigList[i];
                    _groups[i].Enable(groupConfig);
                }
            }
        }

        /// <inheritdoc />
        protected override void OnRefresh()
        {
            using (_PRF_OnRefresh.Auto())
            {
                _groups ??= new TGroupList();

                //base.OnRefresh();

                var parent = GetMultiPartParent();

                for (var i = 0; i < _groups.Count; i++)
                {
                    var group = _groups[i];
                    var prefix = AppaControlNamer.GetElementPrefix(i);

                    AppaComponentGroup<TGroup, TGroupConfig>.Refresh(ref group, parent, prefix);
                }
            }
        }

        private void CleanComponentList()
        {
            using (_PRF_CleanComponentList.Auto())
            {
                for (var i = _groups.Count - 1; i >= 0; i--)
                {
                    var group = _groups[i];

                    if (group == null)
                    {
                        _groups.RemoveAt(i);
                    }
                }
            }
        }

        #region IMultiPartControl<TGroup,TGroupList,TGroupConfig,TGroupConfigList> Members

        public TGroupList GroupList => _groups;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_CleanComponentList =
            new ProfilerMarker(_PRF_PFX + nameof(CleanComponentList));

        protected static readonly ProfilerMarker _PRF_GetMultiPartParent =
            new ProfilerMarker(_PRF_PFX + nameof(GetMultiPartParent));

        #endregion
    }
}
