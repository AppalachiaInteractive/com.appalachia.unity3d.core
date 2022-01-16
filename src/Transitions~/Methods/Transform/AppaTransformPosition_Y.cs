namespace Appalachia.Core.Transitions.Methods.Transform
{
    /// <summary>This component allows you to transition the Transform's position.y value.</summary>
    [UnityEngine.AddComponentMenu(
        AppaTransition.MethodsMenuPrefix +
        "Transform/Transform.position.y" +
        AppaTransition.MethodsMenuSuffix +
        nameof(AppaTransformPosition_Y)
    )]
    public class AppaTransformPosition_Y : AppaMethodWithStateAndTarget
    {
        #region Fields and Autoproperties

        public State Data;

        #endregion

        public static AppaTransitionState Register(
            UnityEngine.Transform target,
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
            return typeof(UnityEngine.Transform);
        }

        public override void Register()
        {
            PreviousState = Register(GetAliasedTarget(Data.Target), Data.Value, Data.Duration, Data.Ease);
        }

        #region Nested type: State

        [System.Serializable]
        public class State : AppaTransitionStateWithTarget<UnityEngine.Transform>
        {
            #region Static Fields and Autoproperties

            public static System.Collections.Generic.Stack<State> Pool =
                new System.Collections.Generic.Stack<State>();

            #endregion

            #region Fields and Autoproperties

            [UnityEngine.Tooltip("This allows you to control how the transition will look.")]
            public AppaEase Ease = AppaEase.Smooth;

            [UnityEngine.Tooltip("The position value will transition to this.")]
            [UnityEngine.Serialization.FormerlySerializedAs("Position")]
            public float Value;

            [System.NonSerialized] private float oldValue;

            #endregion

            public override int CanFill => (Target != null) && (Target.position.y != Value) ? 1 : 0;

            public override void BeginWithTarget()
            {
                oldValue = Target.position.y;
            }

            public override void Despawn()
            {
                Pool.Push(this);
            }

            public override void FillWithTarget()
            {
                Value = Target.position.y;
            }

            public override void UpdateWithTarget(float progress)
            {
                var vector = Target.position;

                vector.y = UnityEngine.Mathf.LerpUnclamped(oldValue, Value, Smooth(Ease, progress));

                Target.position = vector;
            }
        }

        #endregion
    }
}
