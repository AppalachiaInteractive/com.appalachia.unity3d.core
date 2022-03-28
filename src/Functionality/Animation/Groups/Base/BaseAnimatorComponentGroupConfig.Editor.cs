#if UNITY_EDITOR
using Appalachia.Core.Functionality.Animation.Components;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Core.Functionality.Animation.Groups.Base
{
    public partial class BaseAnimatorComponentGroupConfig<TGroup, TConfig>
    {
        #region Fields and Autoproperties

        /// <summary>
        ///     The config for the <see cref="AnimationRemapper" /> component.
        /// </summary>
        [LabelText("$" + nameof(AnimationRemapperLabel))]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        [HideIf("@!ShowAllFields && (HideAnimationRemapper || HideAllFields)")]
        public AnimationRemapperConfig.Optional animationRemapper;

        #endregion

        protected virtual bool HideAnimationRemapper => false;
        public AnimationRemapperConfig.Optional AnimationRemapper => animationRemapper;

        private string AnimationRemapperLabel => nameof(AnimationRemapperConfig);
    }
}

#endif
