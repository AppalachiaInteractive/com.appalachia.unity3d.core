namespace Appalachia.Core.Transitions.Methods.AudioSource
{
    /// <summary>This component allows you to transition the AudioSource's volume value.</summary>
    [UnityEngine.AddComponentMenu(
        AppaTransition.MethodsMenuPrefix +
        "AudioSource/AudioSource.volume" +
        AppaTransition.MethodsMenuSuffix +
        nameof(AppaAudioSourceVolume)
    )]
    public class AppaAudioSourceVolume : AppaMethodWithStateAndTarget
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

            [UnityEngine.Tooltip("The volume value will transition to this.")]
            [UnityEngine.Serialization.FormerlySerializedAs("Volume")]
            [UnityEngine.Range(0.0f, 1.0f)]
            public float Value = 1.0f;

            [System.NonSerialized] private float oldValue;

            #endregion

            public override int CanFill => (Target != null) && (Target.volume != Value) ? 1 : 0;

            public override void BeginWithTarget()
            {
                oldValue = Target.volume;
            }

            public override void Despawn()
            {
                Pool.Push(this);
            }

            public override void FillWithTarget()
            {
                Value = Target.volume;
            }

            public override void UpdateWithTarget(float progress)
            {
                Target.volume = UnityEngine.Mathf.LerpUnclamped(oldValue, Value, Smooth(Ease, progress));
            }
        }

        #endregion
    }
}
