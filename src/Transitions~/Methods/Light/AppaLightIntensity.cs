namespace Appalachia.Core.Transitions.Methods.Light
{
    /// <summary>This component allows you to transition the Light's intensity value.</summary>
    [UnityEngine.AddComponentMenu(
        AppaTransition.MethodsMenuPrefix +
        "Light/Light.intensity" +
        AppaTransition.MethodsMenuSuffix +
        nameof(AppaLightIntensity)
    )]
    public class AppaLightIntensity : AppaMethodWithStateAndTarget
    {
        #region Fields and Autoproperties

        public State Data;

        #endregion

        public static AppaTransitionState Register(
            UnityEngine.Light target,
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
            return typeof(UnityEngine.Light);
        }

        public override void Register()
        {
            PreviousState = Register(GetAliasedTarget(Data.Target), Data.Value, Data.Duration, Data.Ease);
        }

        #region Nested type: State

        [System.Serializable]
        public class State : AppaTransitionStateWithTarget<UnityEngine.Light>
        {
            #region Static Fields and Autoproperties

            public static System.Collections.Generic.Stack<State> Pool =
                new System.Collections.Generic.Stack<State>();

            #endregion

            #region Fields and Autoproperties

            [UnityEngine.Tooltip("This allows you to control how the transition will look.")]
            public AppaEase Ease = AppaEase.Smooth;

            [UnityEngine.Tooltip("The intensity value will transition to this.")]
            [UnityEngine.Serialization.FormerlySerializedAs("Intensity")]
            public float Value = 1.0f;

            [System.NonSerialized] private float oldValue;

            #endregion

            public override int CanFill => (Target != null) && (Target.intensity != Value) ? 1 : 0;

            public override void BeginWithTarget()
            {
                oldValue = Target.intensity;
            }

            public override void Despawn()
            {
                Pool.Push(this);
            }

            public override void FillWithTarget()
            {
                Value = Target.intensity;
            }

            public override void UpdateWithTarget(float progress)
            {
                Target.intensity = UnityEngine.Mathf.LerpUnclamped(oldValue, Value, Smooth(Ease, progress));
            }
        }

        #endregion
    }
}
