using System;
using System.Diagnostics;
using Appalachia.Core.Objects.Components.Core;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;

// ReSharper disable UnusedParameter.Global

namespace Appalachia.Core.Objects.Components.Subsets
{
    /// <summary>
    ///     A subset around multiple components, so that they can be put into collections together.
    /// </summary>
    /// <typeparam name="TComponentSubset">The subset.</typeparam>
    /// <typeparam name="TComponentSubsetData">Configuration data for the subset.</typeparam>
    [Serializable]
    [DebuggerDisplay("{Name} (ComponentSubset)")]
    public abstract class ComponentSubset<TComponentSubset, TComponentSubsetData> : AppalachiaBase<TComponentSubset>,
        IComponentSubset
        where TComponentSubset : ComponentSubset<TComponentSubset, TComponentSubsetData>, new()
        where TComponentSubsetData : ComponentSubsetData<TComponentSubset, TComponentSubsetData>, new()
    {
        #region Fields and Autoproperties

        [SerializeField] public GameObject gameObject;

        #endregion

        protected abstract bool IsUI { get; }

        protected virtual string ComponentSubsetName => typeof(TComponentSubset).Name;

        public virtual void Disable()
        {
            using (_PRF_DisableSubset.Auto())
            {
                gameObject.SetActive(false);
            }
        }

        public virtual void Enable(TComponentSubsetData data)
        {
            using (_PRF_EnableSubset.Auto())
            {
                if (data.ShouldEnable)
                {
                    gameObject.SetActive(true);
                }
            }
        }

        internal static void GetOrAddComponents(
            ref TComponentSubset subset,
            TComponentSubsetData data,
            GameObject subsetObject)
        {
            using (_PRF_GetOrAddComponents.Auto())
            {
                subset ??= new TComponentSubset();

                subset.GetOrAddComponents(data, subsetObject);
            }
        }

        internal static void GetOrAddComponents(
            ref TComponentSubset subset,
            TComponentSubsetData data,
            GameObject subsetParent,
            string gameObjectNamePostfix)
        {
            using (_PRF_GetOrAddComponents.Auto())
            {
                subset ??= new TComponentSubset();

                subset.GetOrAddComponents(data, subsetParent, gameObjectNamePostfix);
            }
        }

        protected virtual void DestroyComponent(Component c)
        {
            using (_PRF_DestroyComponent.Auto())
            {
                if (c is not Transform)
                {
                    c.DestroySafely();
                }
            }
        }

        protected virtual void DisableComponent(Component c)
        {
            using (_PRF_DisableComponent.Auto())
            {
                if (c is Behaviour b)
                {
                    b.enabled = false;
                }
            }
        }

        protected virtual void EnableComponent<TComponent,TComponentData>(TComponentSubsetData data, TComponent c, TComponentData d)
        where TComponent : Component
        where TComponentData : ComponentData<TComponent, TComponentData>, new()
        {
            using (_PRF_EnableComponent.Auto())
            {
                if (d.Enabled && c is Behaviour b)
                {
                    b.enabled = true;
                }
            }
        }

        protected virtual void GetOrAddComponent<T>(ref T component, GameObject target = null)
            where T : Component
        {
            using (_PRF_GetOrAddComponent.Auto())
            {
                if (target == null)
                {
                    target = gameObject;
                }

                if (component == null)
                {
                    target.GetOrAddComponent(ref component);
                }
                else if (component.gameObject != target)
                {
                    DestroyComponent(component);

                    component = target.AddComponent<T>();
                }
            }
        }

        /// <summary>
        ///     Creates the set on the <paramref name="subsetObject" /> and adds the required components.
        /// </summary>
        /// <param name="data">The subset data.</param>
        /// <param name="subsetObject">The subset object.</param>
        protected virtual void GetOrAddComponents(TComponentSubsetData data, GameObject subsetObject)
        {
            using (_PRF_GetOrAddComponents.Auto())
            {
                gameObject = subsetObject;

                gameObject.SetActive(data.ShouldEnable);
            }
        }

        /// <summary>
        ///     Creates the set underneath the <paramref name="subsetParent" />, using the provided <paramref name="subsetGameObjectName" />, and adds the
        ///     required
        ///     components.
        /// </summary>
        /// <param name="data">The subset data.</param>
        /// <param name="subsetParent">The parent of the set.</param>
        /// <param name="subsetGameObjectName">The name of the set.</param>
        protected virtual void GetOrAddComponents(

            // ReSharper disable once UnusedParameter.Global
            TComponentSubsetData data,
            GameObject subsetParent,
            string subsetGameObjectName)
        {
            using (_PRF_GetOrAddComponents.Auto())
            {
                subsetParent.GetOrAddChild(ref gameObject, subsetGameObjectName, IsUI);

                gameObject.name = subsetGameObjectName;

                gameObject.SetParentTo(subsetParent);

                gameObject.SetActive(data.ShouldEnable);
            }
        }

        #region IComponentSubset Members

        public override string Name => gameObject.name;

        public virtual void DestroySafely(bool includeGameObject = true)
        {
            using (_PRF_DestroySafely.Auto())
            {
                var go = gameObject;

                if (includeGameObject)
                {
                    go.DestroySafely();
                }
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_DestroyComponent =
            new ProfilerMarker(_PRF_PFX + nameof(DestroyComponent));

        protected static readonly ProfilerMarker _PRF_DestroySafely =
            new ProfilerMarker(_PRF_PFX + nameof(DestroySafely));

        private static readonly ProfilerMarker _PRF_DisableComponent =
            new ProfilerMarker(_PRF_PFX + nameof(DisableComponent));

        protected static readonly ProfilerMarker _PRF_DisableSubset = new ProfilerMarker(_PRF_PFX + nameof(Disable));

        private static readonly ProfilerMarker _PRF_EnableComponent =
            new ProfilerMarker(_PRF_PFX + nameof(EnableComponent));

        protected static readonly ProfilerMarker _PRF_EnableSubset = new ProfilerMarker(_PRF_PFX + nameof(Enable));

        private static readonly ProfilerMarker _PRF_GetOrAddComponent =
            new ProfilerMarker(_PRF_PFX + nameof(GetOrAddComponent));

        protected static readonly ProfilerMarker _PRF_GetOrAddComponents =
            new ProfilerMarker(_PRF_PFX + nameof(GetOrAddComponents));

        #endregion
    }
}
