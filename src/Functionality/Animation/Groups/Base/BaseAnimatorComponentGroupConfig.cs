using System;
using System.Collections.Generic;
using Appalachia.Core.ControlModel.ComponentGroups;
using Appalachia.Core.ControlModel.Components.Contracts;
using Appalachia.Core.ControlModel.Extensions;
using Appalachia.Core.Functionality.Animation.Components;
using Appalachia.Core.Objects.Initialization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Core.Functionality.Animation.Groups.Base
{
    [Serializable]
    public partial class BaseAnimatorComponentGroupConfig<TGroup, TConfig> : AppaComponentGroupConfig<TGroup, TConfig>
        where TGroup : BaseAnimatorComponentGroup<TGroup, TConfig>
        where TConfig : BaseAnimatorComponentGroupConfig<TGroup, TConfig>, new()
    {
        #region Fields and Autoproperties

        /// <summary>
        ///     The config for the <see cref="Animator" /> component.
        /// </summary>
        [LabelText("$" + nameof(AnimatorLabel))]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        [HideIf("@!ShowAllFields && (HideAnimator || HideAllFields)")]
        public AnimatorConfig animator;

        #endregion

        protected virtual bool HideAnimator => false;

        public AnimatorConfig Animator => animator;
        private string AnimatorLabel => nameof(AnimatorConfig);

        /// <inheritdoc />
        public override void ResetConfig()
        {
            using (_PRF_ResetConfig.Auto())
            {
                base.ResetConfig();

                AnimatorConfig.Refresh(ref animator, Owner);
                animator.ResetConfig();

#if UNITY_EDITOR
                AnimationRemapperConfig.Refresh(ref animationRemapper, false, Owner);
                animationRemapper.Value.ResetConfig();
#endif
            }
        }

        protected override void CollectConfigs(List<IAppaComponentConfig> configs)
        {
            using (_PRF_CollectConfigs.Auto())
            {
                //base.CollectConfigs(configs);

                configs.Add(animator);
#if UNITY_EDITOR
                configs.Add(animationRemapper.Value);
#endif
            }
        }

        /// <inheritdoc />
        protected override void OnApply(TGroup group)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(group);

                animator.Apply(group.Animator);
#if UNITY_EDITOR
                animationRemapper.Apply(group.AnimationRemapper);
#endif
            }
        }
        
        /// <inheritdoc />
        protected override void OnInitializeFields(Initializer initializer)
        {
            using (_PRF_Refresh.Auto())
            {
                // base.OnInitializeFields(initializer, owner);

                AnimatorConfig.Refresh(ref animator, Owner);
#if UNITY_EDITOR
                AnimationRemapperConfig.Refresh(ref animationRemapper, false, Owner);
#endif
            }
        }

        protected override void SubscribeResponsiveConfigs()
        {
            using (_PRF_SubscribeResponsiveConfigs.Auto())
            {
                base.SubscribeResponsiveConfigs();

                animator.SubscribeToChanges(OnChanged);
            }
        }
        
        
        protected override void UnsuspendResponsiveConfigs()
        {
            using (_PRF_UnsuspendResponsiveConfigs.Auto())
            {
                base.UnsuspendResponsiveConfigs();

                animator.UnsuspendChanges();
            }
        }
        
        
        protected override void SuspendResponsiveConfigs()
        {
            using (_PRF_SuspendResponsiveConfigs.Auto())
            {
                base.SuspendResponsiveConfigs();

                animator.SuspendChanges();
            }
        }
    }
}
