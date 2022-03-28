using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Functionality.Animation.Components
{
    [Serializable]
    public class AnimatorConfigParameter : AppalachiaBase<AnimatorConfigParameter>
    {
        public AnimatorConfigParameter()
        {
        }

        public AnimatorConfigParameter(Object owner) : base(owner)
        {
        }

        #region Fields and Autoproperties

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public string name;

        /// <summary>
        ///     One of:
        ///     <list type="bullet">
        ///         <item>Float = Float type parameter</item>
        ///         <item>Int = Int type parameter</item>
        ///         <item>Bool = Boolean type parameter</item>
        ///     </list>
        /// </summary>
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public AnimatorControllerParameterType type;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        [LabelText("Value")]
        public bool onlyApplyOnStart;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        [LabelText("Value")]
        [HideIf(nameof(_hideFloatValue))]
        public float floatValue;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        [LabelText("Value")]
        [HideIf(nameof(_hideIntValue))]
        public int intValue;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        [LabelText("Value")]
        [HideIf(nameof(_hideBoolValue))]
        public bool boolValue;

        #endregion

        private bool _hideBoolValue => type != AnimatorControllerParameterType.Bool;
        private bool _hideFloatValue => type != AnimatorControllerParameterType.Float;
        private bool _hideIntValue => type != AnimatorControllerParameterType.Int;

        public void Apply(Animator animator)
        {
            using (_PRF_Apply.Auto())
            {
                switch (type)
                {
                    case AnimatorControllerParameterType.Float:
                        animator.SetFloat(name, floatValue);
                        break;
                    case AnimatorControllerParameterType.Int:
                        animator.SetInteger(name, intValue);
                        break;
                    case AnimatorControllerParameterType.Bool:
                        animator.SetBool(name, boolValue);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(onlyApplyOnStart), () => onlyApplyOnStart = true);
            }
        }

        /// <inheritdoc />
        protected override void OnChanged()
        {
            using (_PRF_OnChanged.Auto())
            {
                if (type == AnimatorControllerParameterType.Trigger)
                {
                    type = AnimatorControllerParameterType.Bool;
                }
                else
                {
                    base.OnChanged();
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        #endregion
    }
}
