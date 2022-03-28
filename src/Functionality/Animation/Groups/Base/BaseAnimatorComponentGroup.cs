using System.Collections.Generic;
using Appalachia.Core.ControlModel.ComponentGroups;
using UnityEngine;

namespace Appalachia.Core.Functionality.Animation.Groups.Base
{
    public abstract partial class BaseAnimatorComponentGroup<TGroup, TConfig> : AppaComponentGroup<TGroup, TConfig>
        where TGroup : BaseAnimatorComponentGroup<TGroup, TConfig>
        where TConfig : BaseAnimatorComponentGroupConfig<TGroup, TConfig>, new()
    {
        #region Fields and Autoproperties

        [SerializeField] public Animator animator;

        #endregion

        public Animator Animator => animator;

        /// <inheritdoc />
        public override void DestroySafely(bool includeGameObject)
        {
            using (_PRF_DestroySafely.Auto())
            {
                DestroyComponent(animator);
#if UNITY_EDITOR
                DestroyComponent(animationRemapper);
#endif

                base.DestroySafely(includeGameObject);
            }
        }

        /// <inheritdoc />
        public override void Disable()
        {
            using (_PRF_DisableGroup.Auto())
            {
                DisableComponent(animator);
#if UNITY_EDITOR
                DisableComponent(animationRemapper);
#endif

                base.Disable();
            }
        }

        /// <inheritdoc />
        public override void Enable(TConfig config)
        {
            using (_PRF_EnableGroup.Auto())
            {
                base.Enable(config);

                EnableComponent(config, animator, config.animator);
#if UNITY_EDITOR
                EnableComponent(config, animationRemapper, config.animationRemapper);
#endif
            }
        }

        /// <inheritdoc />
        protected override void OnRefresh()
        {
            using (_PRF_OnRefresh.Auto())
            {
                //base.OnRefresh();

                GetOrAddComponent(ref animator);
#if UNITY_EDITOR
                GetOrAddComponent(ref animationRemapper);
#endif
            }
        }

        protected override void CollectComponents(List<Component> components)
        {
            using (_PRF_CollectComponents.Auto())
            {
                //base.CollectComponents(components);

                components.Add(animator);
#if UNITY_EDITOR
                components.Add(animationRemapper);
#endif
            }
        }
    }
}
