namespace Appalachia.Core.Transitions.Methods.Transform
{
    /// <summary>This component allows you to transition the Transform's localPosition value.</summary>
    [UnityEngine.AddComponentMenu(
        AppaTransition.MethodsMenuPrefix +
        "Transform/Transform.localPosition" +
        AppaTransition.MethodsMenuSuffix +
        nameof(AppaTransformLocalPosition)
    )]
    public class AppaTransformLocalPosition : AppaMethodWithStateAndTarget
    {
        #region Fields and Autoproperties

        public State Data;

        #endregion

        public static AppaTransitionState Register(
            UnityEngine.Transform target,
            UnityEngine.Vector3 value,
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

            [UnityEngine.Tooltip("The localPosition value will transition to this.")]
            [UnityEngine.Serialization.FormerlySerializedAs("Position")]
            public UnityEngine.Vector3 Value;

            [System.NonSerialized] private UnityEngine.Vector3 oldValue;

            #endregion

            public override int CanFill => (Target != null) && (Target.localPosition != Value) ? 1 : 0;

            public override void BeginWithTarget()
            {
                oldValue = Target.localPosition;
            }

            public override void Despawn()
            {
                Pool.Push(this);
            }

            public override void FillWithTarget()
            {
                Value = Target.localPosition;
            }

            public override void UpdateWithTarget(float progress)
            {
                Target.localPosition = UnityEngine.Vector3.LerpUnclamped(
                    oldValue,
                    Value,
                    Smooth(Ease, progress)
                );
            }
        }

        #endregion
    }
}
