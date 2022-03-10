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
    ///     <see
    ///         cref="ComponentSubset{TComponentSubset,TComponentSubsetData,TComponent1,TComponent2,TComponentN,TComponentData1,TComponentData2,TComponentDataN}" />
    ///     .
    /// </summary>
    /// <typeparam name="TComponentSubset">The type of component subset this configures.</typeparam>
    /// <typeparam name="TComponent1">The first component in the subset.</typeparam>
    /// <typeparam name="TComponent2">The second component in the subset.</typeparam>
    /// <typeparam name="TComponentN">The third component in the subset.</typeparam>
    /// <typeparam name="TComponentSubsetData">This type.</typeparam>
    /// <typeparam name="TComponentData1">The first configuration data for the corresponding component in the subset.</typeparam>
    /// <typeparam name="TComponentData2">The second configuration data for the corresponding component in the subset.</typeparam>
    /// <typeparam name="TComponentDataN">The third configuration data for the corresponding component in the subset.</typeparam>
    [Serializable]
    public abstract class ComponentSubsetData<TComponentSubset, TComponentSubsetData, TComponent1, TComponent2,
                                              TComponentN, TComponentData1, TComponentData2, TComponentDataN> :
        ComponentSubsetData<TComponentSubset, TComponentSubsetData, TComponent1, TComponent2, TComponentData1,
            TComponentData2>,
        IComponentData<TComponentSubset, TComponentSubsetData>
        where TComponentSubset : ComponentSubset<TComponentSubset, TComponentSubsetData, TComponent1, TComponent2,
            TComponentN, TComponentData1, TComponentData2, TComponentDataN>, new()
        where TComponentSubsetData : ComponentSubsetData<TComponentSubset, TComponentSubsetData, TComponent1,
            TComponent2, TComponentN, TComponentData1, TComponentData2, TComponentDataN>, new()
        where TComponent1 : Component
        where TComponent2 : Component
        where TComponentN : Component
        where TComponentData1 : ComponentData<TComponent1, TComponentData1>, new()
        where TComponentData2 : ComponentData<TComponent2, TComponentData2>, new()
        where TComponentDataN : ComponentData<TComponentN, TComponentDataN>, new()
    {
        protected ComponentSubsetData()
        {
        }

        protected ComponentSubsetData(Object owner) : base(owner)
        {
        }

        #region Fields and Autoproperties

        /// <summary>
        ///     The configuration data for the third component.
        /// </summary>
        [LabelText("$" + nameof(data3Label))]
        [SerializeField]
        [HideIf("@!ShowAllFields && (HideData3 || HideAllFields)")]
        public TComponentDataN data3;

        #endregion

        protected virtual bool HideData3 => false;
        private string data3Label => typeof(TComponentDataN).Name;

        /// <inheritdoc />
        protected override void OnApply(TComponentSubset subset)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(subset);

                ComponentData<TComponentN, TComponentDataN>.RefreshAndApply(ref data3, Owner, subset.component3);
            }
        }

        /// <inheritdoc />
        protected override void OnInitializeFields(Initializer initializer, Object owner)
        {
            using (_PRF_CreateOrRefresh.Auto())
            {
                base.OnInitializeFields(initializer, owner);

                ComponentData<TComponentN, TComponentDataN>.CreateOrRefresh(ref data3, owner);
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

                data3.ResetData();
            }
        }

        #endregion
    }
}
