using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Objects.Components.Subsets
{
    /// <summary>
    ///     Configuration data for a
    ///     <see
    ///         cref="ComponentSubset{TComponentSubset,TComponentSubsetData,TComponent1,TComponent2,TComponent3,TComponent4,TComponentN,TComponentData1,TComponentData2,TComponentData3,TComponentData4,TComponentDataN}" />
    ///     .
    /// </summary>
    /// <typeparam name="TComponentSubset">The type of component subset this configures.</typeparam>
    /// <typeparam name="TComponent1">The first component in the subset.</typeparam>
    /// <typeparam name="TComponent2">The second component in the subset.</typeparam>
    /// <typeparam name="TComponent3">The third component in the subset.</typeparam>
    /// <typeparam name="TComponent4">The fourth component in the subset.</typeparam>
    /// <typeparam name="TComponentN">The fifth component in the subset.</typeparam>
    /// <typeparam name="TComponentSubsetData">This type.</typeparam>
    /// <typeparam name="TComponentData1">The first configuration data for the corresponding component in the subset.</typeparam>
    /// <typeparam name="TComponentData2">The second configuration data for the corresponding component in the subset.</typeparam>
    /// <typeparam name="TComponentData3">The third configuration data for the corresponding component in the subset.</typeparam>
    /// <typeparam name="TComponentData4">The fourth configuration data for the corresponding component in the subset.</typeparam>
    /// <typeparam name="TComponentDataN">The fifth configuration data for the corresponding component in the subset.</typeparam>
    [Serializable]
    public abstract class ComponentSubsetData<TComponentSubset, TComponentSubsetData, TComponent1, TComponent2,
                                              TComponent3, TComponent4, TComponentN, TComponentData1, TComponentData2,
                                              TComponentData3, TComponentData4, TComponentDataN> :
        ComponentSubsetData<TComponentSubset, TComponentSubsetData, TComponent1, TComponent2, TComponent3, TComponent4,
            TComponentData1, TComponentData2, TComponentData3, TComponentData4>,
        IComponentData<TComponentSubset, TComponentSubsetData>
        where TComponentSubset : ComponentSubset<TComponentSubset, TComponentSubsetData, TComponent1, TComponent2,
            TComponent3, TComponent4, TComponentN, TComponentData1, TComponentData2, TComponentData3, TComponentData4,
            TComponentDataN>, new()
        where TComponentSubsetData : ComponentSubsetData<TComponentSubset, TComponentSubsetData, TComponent1,
            TComponent2, TComponent3, TComponent4, TComponentN, TComponentData1, TComponentData2, TComponentData3,
            TComponentData4, TComponentDataN>, new()
        where TComponent1 : Component
        where TComponent2 : Component
        where TComponent3 : Component
        where TComponent4 : Component
        where TComponentN : Component
        where TComponentData1 : ComponentData<TComponent1, TComponentData1>, new()
        where TComponentData2 : ComponentData<TComponent2, TComponentData2>, new()
        where TComponentData3 : ComponentData<TComponent3, TComponentData3>, new()
        where TComponentData4 : ComponentData<TComponent4, TComponentData4>, new()
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
        ///     The configuration data for the fifth component.
        /// </summary>
        [LabelText("$" + nameof(data5Label))]
        [SerializeField]
        [ShowIf(nameof(ShowData5))]
        [HideIf(nameof(HideAllFields))]
        public TComponentDataN data5;

        #endregion

        protected virtual bool ShowData5 => true;
        private string data5Label => typeof(TComponentDataN).Name;

        /// <inheritdoc />
        protected override void OnApply(TComponentSubset subset)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(subset);

                ComponentData<TComponentN, TComponentDataN>.RefreshAndApply(ref data5, Owner, subset.component5);
            }
        }

        /// <inheritdoc />
        protected override void CreateOrRefresh(Object owner)
        {
            using (_PRF_CreateOrRefresh.Auto())
            {
                base.CreateOrRefresh(owner);

                ComponentData<TComponentN, TComponentDataN>.CreateOrRefresh(ref data5, owner);
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

                data5.ResetData();
            }
        }

        #endregion
    }
}
