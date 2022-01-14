namespace Appalachia.Core.Transitions.Methods.AudioSource
{
    /// <summary>This component allows you to transition the AudioSource's pitch value.</summary>
    [UnityEngine.AddComponentMenu(
        AppaTransition.MethodsMenuPrefix +
        "AudioSource/AudioSource.pitch" +
        AppaTransition.MethodsMenuSuffix +
        nameof(AppaAudioSourcePitch)
    )]
    public class AppaAudioSourcePitch : AppaMethodWithStateAndTarget
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

            [UnityEngine.Tooltip("The pitch value will transition to this.")]
            [UnityEngine.Serialization.FormerlySerializedAs("Pitch")]
            [UnityEngine.Range(-3.0f, 3.0f)]
            public float Value = 1.0f;

            [System.NonSerialized] private float oldValue;

            #endregion

            public override int CanFill => (Target != null) && (Target.pitch != Value) ? 1 : 0;

            public override void BeginWithTarget()
            {
                oldValue = Target.pitch;
            }

            public override void Despawn()
            {
                Pool.Push(this);
            }

            public override void FillWithTarget()
            {
                Value = Target.pitch;
            }

            public override void UpdateWithTarget(float progress)
            {
                Target.pitch = UnityEngine.Mathf.LerpUnclamped(oldValue, Value, Smooth(Ease, progress));
            }
        }

        #endregion
    }
}
