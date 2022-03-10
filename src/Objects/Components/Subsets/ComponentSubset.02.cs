using System;
using Appalachia.Core.Objects.Components.Core;
using UnityEngine;

namespace Appalachia.Core.Objects.Components.Subsets
{
    /// <summary>
    ///     A subset around two components, so that they can be put into collections together.
    /// </summary>
    /// <typeparam name="TComponentSubset">The subset.</typeparam>
    /// <typeparam name="TComponent1">The first subset component.</typeparam>
    /// <typeparam name="TComponent2">The second subset component.</typeparam>
    /// <typeparam name="TComponentSubsetData">Configuration data for the subset.</typeparam>
    /// <typeparam name="TComponentData1">Configuration data for first subset component.</typeparam>
    /// <typeparam name="TComponentData2">Configuration data for second subset component.</typeparam>
    [Serializable]
    public abstract class ComponentSubset<TComponentSubset, TComponentSubsetData, TComponent1, TComponent2,
                                          TComponentData1, TComponentData2> : ComponentSubset<TComponentSubset,
        TComponentSubsetData>
        where TComponentSubset : ComponentSubset<TComponentSubset, TComponentSubsetData, TComponent1, TComponent2,
            TComponentData1, TComponentData2>, new()
        where TComponentSubsetData : ComponentSubsetData<TComponentSubset, TComponentSubsetData, TComponent1,
            TComponent2, TComponentData1, TComponentData2>, new()
        where TComponent1 : Component
        where TComponentData1 : ComponentData<TComponent1, TComponentData1>, new()
        where TComponent2 : Component
        where TComponentData2 : ComponentData<TComponent2, TComponentData2>, new()
    {
        #region Fields and Autoproperties

        [SerializeField] public TComponent1 component1;
        [SerializeField] public TComponent2 component2;

        #endregion

        /// <inheritdoc />
        public override void DestroySafely(bool includeGameObject = true)
        {
            using (_PRF_DestroySafely.Auto())
            {
                DestroyComponent(component1);
                DestroyComponent(component2);

                base.DestroySafely(includeGameObject);
            }
        }

        /// <inheritdoc />
        public override void Disable()
        {
            using (_PRF_DisableSubset.Auto())
            {
                DisableComponent(component1);
                DisableComponent(component2);

                base.Disable();
            }
        }

        /// <param name="data"></param>
        /// <inheritdoc />
        public override void Enable(TComponentSubsetData data)
        {
            using (_PRF_EnableSubset.Auto())
            {
                base.Enable(data);

                EnableComponent(data, component1, data.data1);
                EnableComponent(data, component2, data.data2);
            }
        }

        /// <inheritdoc />
        protected override void GetOrAddComponents(TComponentSubsetData data, GameObject subsetParent)
        {
            using (_PRF_GetOrAddComponents.Auto())
            {
                base.GetOrAddComponents(data, subsetParent);

                GetOrAddComponent(ref component1);
                GetOrAddComponent(ref component2);
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

                GetOrAddComponent(ref component1);
                GetOrAddComponent(ref component2);
            }
        }
    }
}
