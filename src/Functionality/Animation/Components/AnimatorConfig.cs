using System;
using System.Collections.Generic;
using Appalachia.Core.ControlModel.Components;
using Appalachia.Core.Objects.Initialization;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Functionality.Animation.Components
{
    [Serializable]
    public class AnimatorConfig : AppaComponentConfig<Animator, AnimatorConfig>
    {
        public AnimatorConfig()
        {
        }

        public AnimatorConfig(Object owner) : base(owner)
        {
        }

        #region Fields and Autoproperties

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        [DisableIf(nameof(DisableAllFields))]
        [HideIf(nameof(HideAllFields))]
        private RuntimeAnimatorController _animatorController;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        [DisableIf(nameof(DisableAllFields))]
        [HideIf(nameof(HideAllFields))]
        private AnimatorConfigParameter.List _parameters;

        #endregion

        public IReadOnlyList<AnimatorConfigParameter> Parameters => _parameters;

        /// <inheritdoc />
        protected override void OnApply(Animator comp)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(comp);
                
                for (var index = 0; index < _parameters.Count; index++)
                {
                    var parameter = _parameters[index];

                    parameter.Apply(comp);
                }
            }
        }

        /// <inheritdoc />
        protected override void OnInitializeFields(Initializer initializer)
        {
            using (_PRF_OnInitializeFields.Auto())
            {
                initializer.Do(
                    this,
                    nameof(_parameters),
                    _parameters == null,
                    () =>
                    {
                        _parameters = new AnimatorConfigParameter.List();
                        _parameters.Added += (param, _) =>
                        {
                            param.SetOwner(Owner);
                            param.SubscribeToChanges(OnChanged);
                        };
                    }
                );
            }
        }

        /// <inheritdoc />
        protected override void SubscribeResponsiveConfigs()
        {
            using (_PRF_SubscribeResponsiveConfigs.Auto())
            {
                base.SubscribeResponsiveConfigs();

                for (var i = 0; i < _parameters.Count; i++)
                {
                    _parameters[i].SubscribeToChanges(OnChanged);
                }
            }
        }
        
        protected override void SuspendResponsiveConfigs()
        {
            using (_PRF_SuspendResponsiveConfigs.Auto())
            {
                base.SuspendResponsiveConfigs();

                for (var i = 0; i < _parameters.Count; i++)
                {
                    _parameters[i].SuspendChanges();
                }
            }
        }
        
        protected override void UnsuspendResponsiveConfigs()
        {
            using (_PRF_UnsuspendResponsiveConfigs.Auto())
            {
                base.UnsuspendResponsiveConfigs();

                for (var i = 0; i < _parameters.Count; i++)
                {
                    _parameters[i].UnsuspendChanges();
                }
            }
        }
    }
}
