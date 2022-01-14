namespace Appalachia.Core.Transitions.Methods.AudioSource
{
    /// <summary>This component allows you to transition the AudioSource's panStereo value.</summary>
    [UnityEngine.AddComponentMenu(
        AppaTransition.MethodsMenuPrefix +
        "AudioSource/AudioSource.panStereo" +
        AppaTransition.MethodsMenuSuffix +
        nameof(AppaAudioSourcePanStereo)
    )]
    public class AppaAudioSourcePanStereo : AppaMethodWithStateAndTarget
    {
        #region Fields and Autoproperties

        public State Data;

        #endregion

        public static AppaTransitionState Register(
            UnityEngine.AudioSource target,
            float value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            var state = AppaTransition.SpawnWithTarget(State.Pool, target);

            state.Value = value;

            state.Ease = ease;

            return AppaTransition.Register(state, duration);
        }

        public override System.Type GetTargetType()
        {
            return typeof(UnityEngine.AudioSource);
        }

        public override void Register()
        {
            PreviousState = Register(GetAliasedTarget(Data.Target), Data.Value, Data.Duration, Data.Ease);
        }

        #region Nested type: State

        [System.Serializable]
        public class State : AppaTransitionStateWithTarget<UnityEngine.AudioSource>
        {
            #region Static Fields and Autoproperties

            public static System.Collections.Generic.Stack<State> Pool =
                new System.Collections.Generic.Stack<State>();

            #endregion

            #region Fields and Autoproperties

            [UnityEngine.Tooltip("This allows you to control how the transition will look.")]
            public AppaEase Ease = AppaEase.Smooth;

            [UnityEngine.Tooltip("The panStereo value will transition to this.")]
            [UnityEngine.Range(-1.0f, 1.0f)]
            public float Value;

            [System.NonSerialized] private float oldValue;

            #endregion

            public override int CanFill => (Target != null) && (Target.panStereo != Value) ? 1 : 0;

            public override void BeginWithTarget()
            {
                oldValue = Target.panStereo;
            }

            public override void Despawn()
            {
                Pool.Push(this);
            }

            public override void FillWithTarget()
            {
                Value = Target.panStereo;
            }

            public override void UpdateWithTarget(float progress)
            {
                Target.panStereo = UnityEngine.Mathf.LerpUnclamped(oldValue, Value, Smooth(Ease, progress));
            }
        }

        #endregion
    }
}
