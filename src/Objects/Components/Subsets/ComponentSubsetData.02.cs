using System;
using Appalachia.Core.Objects.Components.Core;
using Appalachia.Core.Objects.Initialization;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Objects.Components.Subsets
{
    /// <summary>
    ///     Configuration data for a
    ///     <see cref="ComponentSubset{TComponentSubset,TComponentSubsetData,TComponent1,TComponent2,TComponentSubsetData,TComponentData1,TComponentData2}" />
    ///     .
    /// </summary>
    /// <typeparam name="TComponentSubset">The type of component subset this configures.</typeparam>
    /// <typeparam name="TComponent1">The first component in the subset.</typeparam>
    /// <typeparam name="TComponent2">The second component in the subset.</typeparam>
    /// <typeparam name="TComponentSubsetData">This type.</typeparam>
    /// <typeparam name="TComponentData1">The first configuration data for the corresponding component in the subset.</typeparam>
    /// <typeparam name="TComponentData2">The second configuration data for the corresponding component in the subset.</typeparam>
    [Serializable]
    public abstract class ComponentSubsetData<TComponentSubset, TComponentSubsetData, TComponent1, TComponent2,
                                              TComponentData1, TComponentData2> :
        ComponentSubsetData<TComponentSubset, TComponentSubsetData>,
        IComponentData<TComponentSubset, TComponentSubsetData>
        where TComponentSubset : ComponentSubset<TComponentSubset, TComponentSubsetData, TComponent1, TComponent2,
            TComponentData1, TComponentData2>, new()
        where TComponentSubsetData : ComponentSubsetData<TComponentSubset, TComponentSubsetData, TComponent1,
            TComponent2, TComponentData1, TComponentData2>, new()
        where TComponent1 : Component
        where TComponentData1 : ComponentData<TComponent1, TComponentData1>, new()
        where TComponent2 : Component
        where TComponentData2 : ComponentData<TComponent2, TComponentData2>, new()
    {
        protected ComponentSubsetData()
        {
        }

        protected ComponentSubsetData(Object owner) : base(owner)
        {
        }

        #region Fields and Autoproperties

        /// <summary>
        ///     The configuration data for the first component.
        /// </summary>
        [LabelText("$" + nameof(data1Label))]
        [SerializeField]
        [HideIf("@!ShowAllFields && (HideData1 || HideAllFields)")]
        public TComponentData1 data1;

        /// <summary>
        ///     The configuration data for the second component.
        /// </summary>
        [LabelText("$" + nameof(data2Label))]
        [SerializeField]
        [HideIf("@!ShowAllFields && (HideData2 || HideAllFields)")]
        public TComponentData2 data2;

        #endregion

        protected virtual bool HideData1 => false;
        protected virtual bool HideData2 => false;
        private string data1Label => typeof(TComponentData1).Name;
        private string data2Label => typeof(TComponentData2).Name;

        /// <inheritdoc />
        protected override void OnApply(TComponentSubset subset)
        {
            using (_PRF_OnApply.Auto())
            {
                ComponentData<TComponent1, TComponentData1>.RefreshAndApply(ref data1, Owner, subset.component1);

                ComponentData<TComponent2, TComponentData2>.RefreshAndApply(ref data2, Owner, subset.component2);
            }
        }

        /// <inheritdoc />
        protected override void OnInitializeFields(Initializer initializer, Object owner)
        {
            using (_PRF_CreateOrRefresh.Auto())
            {
                ComponentData<TComponent1, TComponentData1>.CreateOrRefresh(ref data1, owner);
                ComponentData<TComponent2, TComponentData2>.CreateOrRefresh(ref data2, owner);
            }
        }

        #region IComponentData<TComponentSubset,TComponentSubsetData> Members

        void IComponentData<TComponentSubset>.Apply(TComponentSubset comp)
        {
            OnApply(comp);
        }

        /// <inheritdoc />
        public override void ResetData()
        {
            using (_PRF_ResetData.Auto())
            {
                base.ResetData();

                data1.ResetData();
                data2.ResetData();
            }
        }

        #endregion
    }
}
