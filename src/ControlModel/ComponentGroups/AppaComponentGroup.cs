using System;
using System.Collections.Generic;
using Appalachia.Core.ControlModel.ComponentGroups.Contracts;
using Appalachia.Core.ControlModel.Components;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

// ReSharper disable UnusedParameter.Global

namespace Appalachia.Core.ControlModel.ComponentGroups
{
    /// <summary>
    ///     A group around multiple components, so that they can be put into collections together.
    /// </summary>
    /// <typeparam name="TGroup">The group.</typeparam>
    /// <typeparam name="TConfig">Config for the group.</typeparam>
    [Serializable]
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public abstract class AppaComponentGroup<TGroup, TConfig> : AppalachiaBehaviour<TGroup>,
                                                                IAppaComponentGroup<TGroup, TConfig>
        where TGroup : AppaComponentGroup<TGroup, TConfig>
        where TConfig : AppaComponentGroupConfig<TGroup, TConfig>, new()
    {
        #region Fields and Autoproperties

        private IReadOnlyList<Component> _components;
        private TConfig _config;

        [SerializeField, HideInInspector]
        private string _namePrefix;

        #endregion

        protected static string NamePostfix => typeof(TGroup).Name;

        public string NamePrefix
        {
            get => _namePrefix;
            set => _namePrefix = value;
        }

        public virtual void Disable()
        {
            using (_PRF_DisableGroup.Auto())
            {
                gameObject.SetActive(false);
            }
        }

        public virtual void Enable(TConfig config)
        {
            using (_PRF_EnableGroup.Auto())
            {
                if (config.ShouldEnable)
                {
                    gameObject.SetActive(true);
                }
            }
        }

        public static void Refresh(ref TGroup group, GameObject root)
        {
            using (_PRF_Refresh.Auto())
            {
                root.GetOrAddComponent(ref group);

                group.Refresh();
            }
        }

        public static void Refresh(ref TGroup group, GameObject parent, string namePrefix)
        {
            using (_PRF_Refresh.Auto())
            {
                var root = group == null ? null : group.gameObject;
                var fullName = AppaControlNamer.GetStyledName(namePrefix, NamePostfix);
                parent.GetOrAddChild(ref root, fullName, false);

                Refresh(ref group, root);
                group.NamePrefix = namePrefix;
            }
        }

        protected abstract void CollectComponents(List<Component> components);

        protected abstract void OnRefresh();

        protected virtual void DestroyComponent(Component c)
        {
            using (_PRF_DestroyComponent.Auto())
            {
                if (c is not UnityEngine.Transform)
                {
                    c.DestroySafely();
                }
            }
        }

        protected virtual void DisableComponent(Component c)
        {
            using (_PRF_DisableComponent.Auto())
            {
                if (c is Behaviour b && (b != null))
                {
                    b.enabled = false;
                }
            }
        }

        protected virtual void EnableComponent<TComponent, TComponentConfig>(
            TConfig config,
            TComponent c,
            AppaComponentConfig<TComponent, TComponentConfig>.Optional d)
            where TComponent : Component
            where TComponentConfig : AppaComponentConfig<TComponent, TComponentConfig>, new()
        {
            using (_PRF_EnableComponent.Auto())
            {
                if (d.IsElected && d.Value.Enabled && c is Behaviour b && (b != null))
                {
                    b.enabled = true;
                }
            }
        }

        protected virtual void EnableComponent<TComponent, TComponentConfig>(
            TConfig config,
            TComponent c,
            TComponentConfig d)
            where TComponent : Component
            where TComponentConfig : AppaComponentConfig<TComponent, TComponentConfig>, new()
        {
            using (_PRF_EnableComponent.Auto())
            {
                if (d.Enabled && c is Behaviour b && (b != null))
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

        #region IAppaComponentGroup<TGroup,TConfig> Members

        [ButtonGroup(nameof(Refresh))]

        /// <summary>
        ///     Populates the group by adding the required components.
        /// </summary>
        public void Refresh()
        {
            using (_PRF_Refresh.Auto())
            {
                OnRefresh();
            }
        }

        public IReadOnlyList<Component> Components
        {
            get
            {
                if ((_components == null) || (_components.Count == 0))
                {
                    var list = new List<Component>();

                    CollectComponents(list);

                    _components = list;
                }

                return _components;
            }
        }

        [ButtonGroup(nameof(Refresh))]
        public void ApplyConfig()
        {
            using (_PRF_ApplyConfig.Auto())
            {
                _config.Apply(this as TGroup);
            }
        }

        string IAppaComponentGroup.NamePostfix => NamePostfix;

        public virtual void DestroySafely(bool includeGameObject)
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

        public TConfig Config
        {
            get => _config;
            set => _config = value;
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_ApplyConfig = new ProfilerMarker(_PRF_PFX + nameof(ApplyConfig));

        protected static readonly ProfilerMarker _PRF_CollectComponents =
            new ProfilerMarker(_PRF_PFX + nameof(CollectComponents));

        private static readonly ProfilerMarker _PRF_DestroyComponent =
            new ProfilerMarker(_PRF_PFX + nameof(DestroyComponent));

        protected static readonly ProfilerMarker _PRF_DestroySafely =
            new ProfilerMarker(_PRF_PFX + nameof(DestroySafely));

        private static readonly ProfilerMarker _PRF_DisableComponent =
            new ProfilerMarker(_PRF_PFX + nameof(DisableComponent));

        protected static readonly ProfilerMarker _PRF_DisableGroup = new ProfilerMarker(_PRF_PFX + nameof(Disable));

        private static readonly ProfilerMarker _PRF_EnableComponent =
            new ProfilerMarker(_PRF_PFX + nameof(EnableComponent));

        protected static readonly ProfilerMarker _PRF_EnableGroup = new ProfilerMarker(_PRF_PFX + nameof(Enable));

        private static readonly ProfilerMarker _PRF_GetOrAddComponent =
            new ProfilerMarker(_PRF_PFX + nameof(GetOrAddComponent));

        protected static readonly ProfilerMarker _PRF_OnRefresh = new ProfilerMarker(_PRF_PFX + nameof(OnRefresh));

        protected static readonly ProfilerMarker _PRF_Refresh = new ProfilerMarker(_PRF_PFX + nameof(Refresh));

        #endregion
    }
}
