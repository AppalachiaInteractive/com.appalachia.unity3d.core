using System.Collections.Generic;
using UnityEngine;

namespace Appalachia.Core.Transitions.Methods
{
    /// <summary>This component allows you to transition <b>Time.timeScale</b> to the target value.</summary>
    [AddComponentMenu(
        AppaTransition.MethodsMenuPrefix +
        "Time.timeScale" +
        AppaTransition.MethodsMenuSuffix +
        nameof(AppaTimeScale)
    )]
    public class AppaTimeScale : AppaMethodWithState
    {
        #region Fields and Autoproperties

        public State Data;

        #endregion

        public static AppaTransitionState Register(
            float fillAmount,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            var state = AppaTransition.Spawn(State.Pool);

            state.TimeScale = fillAmount;
            state.Ease = ease;

            return AppaTransition.Register(state, duration);
        }

        public override void Register()
        {
            PreviousState = Register(Data.TimeScale, Data.Duration, Data.Ease);
        }

        #region Nested type: State

        [System.Serializable]
        public class State : AppaTransitionState
        {
            #region Static Fields and Autoproperties

            public static Stack<State> Pool = new Stack<State>();

            #endregion

            #region Fields and Autoproperties

            [Tooltip("The ease method that will be used for the transition.")]
            public AppaEase Ease = AppaEase.Smooth;

            [Tooltip("The timeScale we will transition to.")]
            public float TimeScale = 1.0f;

            [System.NonSerialized] private float oldTimeScale;

            #endregion

            public override int CanFill => Time.timeScale != TimeScale ? 1 : 0;

            public override void Begin()
            {
                oldTimeScale = Time.timeScale;
            }

            public override void Despawn()
            {
                Pool.Push(this);
            }

            public override void Fill()
            {
                TimeScale = Time.timeScale;
            }

            public override void Update(float progress)
            {
                Time.timeScale = Mathf.LerpUnclamped(oldTimeScale, TimeScale, Smooth(Ease, progress));
            }
        }

        #endregion
    }
}
