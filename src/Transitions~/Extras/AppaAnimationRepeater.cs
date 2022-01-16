using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using UnityEngine;
using UnityEngine.Events;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;

namespace Appalachia.Core.Transitions.Extras
{
    /// <summary>This component executes the specified transitions at regular intervals.</summary>
    [AddComponentMenu(PKG.Prefix + nameof(AppaAnimationRepeater))]
    public class AppaAnimationRepeater : AppaManualAnimation
    {
        #region Fields and Autoproperties

        [SerializeField] protected int remainingCount = -1;

        [SerializeField]
        [FSA("RemainingTime")]
        protected float remainingTime = 1.0f;

        [SerializeField]
        [FSA("TimeInterval")]
        private float timeInterval = 3.0f;

        [SerializeField]
        [FSA("OnAnimation")]
        protected UnityEvent onAnimation;

        #endregion

        /// <summary>The event will execute when the transitions begin.</summary>
        public UnityEvent OnAnimation
        {
            get
            {
                if (onAnimation == null)
                {
                    onAnimation = new UnityEvent();
                }

                return onAnimation;
            }
        }

        /// <summary>When this reaches 0, the transitions will begin.</summary>
        public float RemainingTime
        {
            set => remainingTime = value;
            get => remainingTime;
        }

        /// <summary>When <b>RemainingTime</b> reaches 0, it will bet set to this value.</summary>
        public float TimeInterval
        {
            set => timeInterval = value;
            get => timeInterval;
        }

        /// <summary>
        ///     The amount of times this component can begin the specified transitions.
        ///     -1 = Unlimited.
        /// </summary>
        public int RemainingCount
        {
            set => remainingCount = value;
            get => remainingCount;
        }

        #region Event Functions

        protected virtual void Update()
        {
            using (_PRF_Update.Auto())
            {
                remainingTime -= Time.deltaTime;

                if (remainingTime <= 0.0f)
                {
                    TryBegin();
                }
            }
        }

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                if (remainingTime <= 0.0f)
                {
                    TryBegin();
                }
            }
        }

        private void TryBegin()
        {
            remainingTime = timeInterval + (remainingTime % timeInterval);

            if (remainingCount >= 0)
            {
                if (remainingCount == 0)
                {
                    return;
                }

                remainingCount -= 1;
            }

            BeginTransitions();

            if (onAnimation != null)
            {
                onAnimation.Invoke();
            }
        }
    }
}
