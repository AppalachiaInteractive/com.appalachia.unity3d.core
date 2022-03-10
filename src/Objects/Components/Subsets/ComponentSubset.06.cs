using System;
using Appalachia.Core.Objects.Components.Core;
using UnityEngine;

namespace Appalachia.Core.Objects.Components.Subsets
{
    /// <summary>
    ///     A subset around six components, so that they can be put into collections together.
    /// </summary>
    /// <typeparam name="TComponentSubset">The subset.</typeparam>
    /// <typeparam name="TComponent1">The first subset component.</typeparam>
    /// <typeparam name="TComponent2">The second subset component.</typeparam>
    /// <typeparam name="TComponent3">The third subset component.</typeparam>
    /// <typeparam name="TComponent4">The fourth subset component.</typeparam>
    /// <typeparam name="TComponent5">The fifth subset component.</typeparam>
    /// <typeparam name="TComponentN">The sixth subset component.</typeparam>
    /// <typeparam name="TComponentSubsetData">Configuration data for the subset.</typeparam>
    /// <typeparam name="TComponentData1">Configuration data for first subset component.</typeparam>
    /// <typeparam name="TComponentData2">Configuration data for second subset component.</typeparam>
    /// <typeparam name="TComponentData3">Configuration data for third subset component.</typeparam>
    /// <typeparam name="TComponentData4">Configuration data for fourth subset component.</typeparam>
    /// <typeparam name="TComponentData5">Configuration data for fifth subset component.</typeparam>
    /// <typeparam name="TComponentDataN">Configuration data for sixth subset component.</typeparam>
    [Serializable]
    public abstract class ComponentSubset<TComponentSubset, TComponentSubsetData, TComponent1, TComponent2, TComponent3,
                                          TComponent4, TComponent5, TComponentN, TComponentData1, TComponentData2,
                                          TComponentData3, TComponentData4, TComponentData5, TComponentDataN> :
        ComponentSubset<TComponentSubset, TComponentSubsetData, TComponent1, TComponent2, TComponent3, TComponent4,
            TComponent5, TComponentData1, TComponentData2, TComponentData3, TComponentData4, TComponentData5>
        where TComponentSubset : ComponentSubset<TComponentSubset, TComponentSubsetData, TComponent1, TComponent2,
            TComponent3, TComponent4, TComponent5, TComponentN, TComponentData1, TComponentData2, TComponentData3,
            TComponentData4, TComponentData5, TComponentDataN>, new()
        where TComponentSubsetData : ComponentSubsetData<TComponentSubset, TComponentSubsetData, TComponent1,
            TComponent2, TComponent3, TComponent4, TComponent5, TComponentN, TComponentData1, TComponentData2,
            TComponentData3, TComponentData4, TComponentData5, TComponentDataN>, new()
        where TComponent1 : Component
        where TComponent2 : Component
        where TComponent3 : Component
        where TComponent4 : Component
        where TComponent5 : Component
        where TComponentN : Component
        where TComponentData1 : ComponentData<TComponent1, TComponentData1>, new()
        where TComponentData2 : ComponentData<TComponent2, TComponentData2>, new()
        where TComponentData3 : ComponentData<TComponent3, TComponentData3>, new()
        where TComponentData4 : ComponentData<TComponent4, TComponentData4>, new()
        where TComponentData5 : ComponentData<TComponent5, TComponentData5>, new()
        where TComponentDataN : ComponentData<TComponentN, TComponentDataN>, new()
    {
        #region Fields and Autoproperties

        [SerializeField] public TComponentN component6;

        #endregion

        /// <inheritdoc />
        public override void DestroySafely(bool includeGameObject = true)
        {
            using (_PRF_DestroySafely.Auto())
            {
                DestroyComponent(component6);

                base.DestroySafely(includeGameObject);
            }
        }

        /// <inheritdoc />
        public override void Disable()
        {
            using (_PRF_DisableSubset.Auto())
            {
                base.Disable();

                DisableComponent(component6);
            }
        }

        /// <param name="data"></param>
        /// <inheritdoc />
        public override void Enable(TComponentSubsetData data)
        {
            using (_PRF_EnableSubset.Auto())
            {
                base.Enable(data);

                EnableComponent(data, component6, data.data6);
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
                GetOrAddComponent(ref component6);
            }
        }

        /// <inheritdoc />
        protected override void GetOrAddComponents(TComponentSubsetData data, GameObject subsetParent)
        {
            using (_PRF_GetOrAddComponents.Auto())
            {
                base.GetOrAddComponents(data, subsetParent);
                GetOrAddComponent(ref component6);
            }
        }
    }
}
