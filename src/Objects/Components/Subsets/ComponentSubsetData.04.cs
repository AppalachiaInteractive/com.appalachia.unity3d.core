using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Objects.Components.Subsets
{
    /// <summary>
    ///     Configuration data for a
    ///     <see
    ///         cref="ComponentSubset{TComponentSubset,TComponentSubsetData,TComponent1,TComponent2,TComponent3,TComponentN,TComponentData1,TComponentData2,TComponentData3,TComponentDataN}" />
    ///     .
    /// </summary>
    /// <typeparam name="TComponentSubset">The type of component subset this configures.</typeparam>
    /// <typeparam name="TComponent1">The first component in the subset.</typeparam>
    /// <typeparam name="TComponent2">The second component in the subset.</typeparam>
    /// <typeparam name="TComponent3">The third component in the subset.</typeparam>
    /// <typeparam name="TComponentN">The fourth component in the subset.</typeparam>
    /// <typeparam name="TComponentSubsetData">This type.</typeparam>
    /// <typeparam name="TComponentData1">The first configuration data for the corresponding component in the subset.</typeparam>
    /// <typeparam name="TComponentData2">The second configuration data for the corresponding component in the subset.</typeparam>
    /// <typeparam name="TComponentData3">The third configuration data for the corresponding component in the subset.</typeparam>
    /// <typeparam name="TComponentDataN">The fourth configuration data for the corresponding component in the subset.</typeparam>
    [Serializable]
    public abstract class ComponentSubsetData<TComponentSubset, TComponentSubsetData, TComponent1, TComponent2,
                                              TComponent3, TComponentN, TComponentData1, TComponentData2,
                                              TComponentData3, TComponentDataN> :
        ComponentSubsetData<TComponentSubset, TComponentSubsetData, TComponent1, TComponent2, TComponent3,
            TComponentData1, TComponentData2, TComponentData3>,
        IComponentData<TComponentSubset, TComponentSubsetData>
        where TComponentSubset : ComponentSubset<TComponentSubset, TComponentSubsetData, TComponent1, TComponent2,
            TComponent3, TComponentN, TComponentData1, TComponentData2, TComponentData3, TComponentDataN>, new()
        where TComponentSubsetData : ComponentSubsetData<TComponentSubset, TComponentSubsetData, TComponent1,
            TComponent2, TComponent3, TComponentN, TComponentData1, TComponentData2, TComponentData3, TComponentDataN>,
        new()
        where TComponent1 : Component
        where TComponent2 : Component
        where TComponent3 : Component
        where TComponentN : Component
        where TComponentData1 : ComponentData<TComponent1, TComponentData1>, new()
        where TComponentData2 : ComponentData<TComponent2, TComponentData2>, new()
        where TComponentData3 : ComponentData<TComponent3, TComponentData3>, new()
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
        ///     The configuration data for the fourth component.
        /// </summary>
        [LabelText("$" + nameof(data4Label))]
        [SerializeField]
        [ShowIf(nameof(ShowData4))]
        [HideIf(nameof(HideAllFields))]
        public TComponentDataN data4;

        #endregion

        protected virtual bool ShowData4 => true;
        private string data4Label => typeof(TComponentDataN).Name;

        /// <inheritdoc />
        protected override void OnApply(TComponentSubset subset)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(subset);

                ComponentData<TComponentN, TComponentDataN>.RefreshAndApply(ref data4, Owner, subset.component4);
            }
        }

        /// <inheritdoc />
        protected override void CreateOrRefresh(Object owner)
        {
            using (_PRF_CreateOrRefresh.Auto())
            {
                base.CreateOrRefresh(owner);

                ComponentData<TComponentN, TComponentDataN>.CreateOrRefresh(ref data4, owner);
            }
        }

        #region IComponentData<TComponentSubset,TComponentSubsetData> Members

        void IComponentData<TComponentSubset>.Apply(TComponentSubset comp)
        {
            OnApply(comp);
        }

        void IComponentData.InitializeFields(Object owner)
        {
        }

        /// <inheritdoc />
        public override void ResetData()
        {
            using (_PRF_ResetData.Auto())
            {
                base.ResetData();

                data4.ResetData();
            }
        }

        #endregion
    }
}
