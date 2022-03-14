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
    public abstract class BaseMultiPartControl<TControl, TConfig, TGroup, TGroupList, TGroupConfig,
                                               TGroupConfigList> : AppaControl<TControl, TConfig>,
                                                                   IMultiPartControl<TGroup, TGroupList, TGroupConfig,
                                                                       TGroupConfigList>
        where TControl : BaseMultiPartControl<TControl, TConfig, TGroup, TGroupList, TGroupConfig,
            TGroupConfigList>
        where TConfig: BaseMultiPartControlConfig<TControl, TConfig, TGroup, TGroupList, TGroupConfig,
            TGroupConfigList>, new()
        where TGroup : AppaComponentGroup<TGroup, TGroupConfig>, new()
        where TGroupList : AppaList<TGroup>, new()
        where TGroupConfig : AppaComponentGroupConfig<TGroup, TGroupConfig>, new()
        where TGroupConfigList : AppaList<TGroupConfig>, new()
    {
        #region Fields and Autoproperties

        [FormerlySerializedAs("_subsetList")]
        [FormerlySerializedAs("_componentList")]
        [PropertyOrder(-230)]
        [SerializeField]
        [ReadOnly]
        private TGroupList _groupList;

        #endregion

        public abstract GameObject GetMultiPartParent();

        /// <inheritdoc />
        public override void DestroySafely()
        {
            using (_PRF_DestroySafely.Auto())
            {
                CleanComponentList();

                for (var i = 0; i < _groupList.Count; i++)
                {
                    _groupList[i].DestroySafely(true);
                }

                _groupList.Clear();
            }
        }

        /// <inheritdoc />
        public override void Disable()
        {
            using (_PRF_Disable.Auto())
            {
                base.Disable();

                CleanComponentList();

                for (var i = 0; i < _groupList.Count; i++)
                {
                    _groupList[i].Disable();
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

                for (var i = 0; i < _groupList.Count; i++)
                {
                    var groupConfig = config.ConfigList[i];
                    _groupList[i].Enable(groupConfig);
                }
            }
        }

        private void CleanComponentList()
        {
            using (_PRF_CleanComponentList.Auto())
            {
                for (var i = _groupList.Count - 1; i >= 0; i--)
                {
                    var group = _groupList[i];

                    if (group == null)
                    {
                        _groupList.RemoveAt(i);
                    }
                }
            }
        }

        #region IMultiPartControl<TGroup,TGroupList,TGroupConfig,TGroupConfigList> Members

        /// <inheritdoc />
        public override void Refresh()
        {
            using (_PRF_Refresh.Auto())
            {
                _groupList ??= new TGroupList();

                base.Refresh();
            }
        }

        public TGroupList GroupList => _groupList;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_CleanComponentList =
            new ProfilerMarker(_PRF_PFX + nameof(CleanComponentList));

        protected static readonly ProfilerMarker _PRF_GetMultiPartParent =
            new ProfilerMarker(_PRF_PFX + nameof(GetMultiPartParent));

        #endregion
    }
}
