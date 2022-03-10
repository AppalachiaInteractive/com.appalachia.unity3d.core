using System;
using Appalachia.Core.Objects.Components.Core;
using UnityEngine;

namespace Appalachia.Core.Objects.Components.Subsets
{
    /// <summary>
    ///     A subset around four components, so that they can be put into collections together.
    /// </summary>
    /// <typeparam name="TComponentSubset">The subset.</typeparam>
    /// <typeparam name="TComponent1">The first subset component.</typeparam>
    /// <typeparam name="TComponent2">The second subset component.</typeparam>
    /// <typeparam name="TComponent3">The third subset component.</typeparam>
    /// <typeparam name="TComponentN">The fourth subset component.</typeparam>
    /// <typeparam name="TComponentSubsetData">Configuration data for the subset.</typeparam>
    /// <typeparam name="TComponentData1">Configuration data for first subset component.</typeparam>
    /// <typeparam name="TComponentData2">Configuration data for second subset component.</typeparam>
    /// <typeparam name="TComponentData3">Configuration data for third subset component.</typeparam>
    /// <typeparam name="TComponentDataN">Configuration data for fourth subset component.</typeparam>
    [Serializable]
    public abstract class ComponentSubset<TComponentSubset, TComponentSubsetData, TComponent1, TComponent2, TComponent3,
                                          TComponentN, TComponentData1, TComponentData2, TComponentData3,
                                          TComponentDataN> : ComponentSubset<TComponentSubset, TComponentSubsetData,
        TComponent1, TComponent2, TComponent3, TComponentData1, TComponentData2, TComponentData3>
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
        #region Fields and Autoproperties

        [SerializeField] public TComponentN component4;

        #endregion

        /// <inheritdoc />
        public override void DestroySafely(bool includeGameObject = true)
        {
            using (_PRF_DestroySafely.Auto())
            {
                DestroyComponent(component4);

                base.DestroySafely(includeGameObject);
            }
        }

        /// <inheritdoc />
        public override void Disable()
        {
            using (_PRF_DisableSubset.Auto())
            {
                base.Disable();

                DisableComponent(component4);
            }
        }

        /// <param name="data"></param>
        /// <inheritdoc />
        public override void Enable(TComponentSubsetData data)
        {
            using (_PRF_EnableSubset.Auto())
            {
                base.Enable(data);

                EnableComponent(data, component4, data.data4);
            }
        }

        /// <inheritdoc />
        protected override void GetOrAddComponents(
            TComponentSubsetData data,
            GameObject subsetParent,
            string subsetGameObjectName)
        {
            using (_PRF_GetOrAddComponents.Auto())
            {
                base.GetOrAddComponents(data, subsetParent, subsetGameObjectName);
                GetOrAddComponent(ref component4);
            }
        }

        /// <inheritdoc />
        protected override void GetOrAddComponents(TComponentSubsetData data, GameObject subsetParent)
        {
            using (_PRF_GetOrAddComponents.Auto())
            {
                base.GetOrAddComponents(data, subsetParent);
                GetOrAddComponent(ref component4);
            }
        }
    }
}
