namespace Appalachia.Core.Transitions.Methods.Transform
{
    /// <summary>This component allows you to transition the Transform's localPosition.xy value.</summary>
    [UnityEngine.AddComponentMenu(
        AppaTransition.MethodsMenuPrefix +
        "Transform/Transform.localPosition.xy" +
        AppaTransition.MethodsMenuSuffix +
        nameof(AppaTransformLocalPosition_xy)
    )]
    public class AppaTransformLocalPosition_xy : AppaMethodWithStateAndTarget
    {
        #region Fields and Autoproperties

        public State Data;

        #endregion

        public static AppaTransitionState Register(
            UnityEngine.Transform target,
            UnityEngine.Vector2 value,
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
            public UnityEngine.Vector2 Value;

            [System.NonSerialized] private UnityEngine.Vector2 oldValue;

            #endregion

            public override int CanFill =>
                (Target != null) &&
                ((Target.localPosition.x != Value.x) || (Target.localPosition.y != Value.y))
                    ? 1
                    : 0;

            public override void BeginWithTarget()
            {
                var vector = Target.localPosition;

                oldValue.x = vector.x;
                oldValue.y = vector.y;
            }

            public override void Despawn()
            {
                Pool.Push(this);
            }

            public override void FillWithTarget()
            {
                var vector = Target.localPosition;

                Value.x = vector.x;
                Value.y = vector.y;
            }

            public override void UpdateWithTarget(float progress)
            {
                var vector = Target.localPosition;
                var smooth = Smooth(Ease, progress);

                vector.x = UnityEngine.Mathf.LerpUnclamped(oldValue.x, Value.x, smooth);
                vector.y = UnityEngine.Mathf.LerpUnclamped(oldValue.y, Value.y, smooth);

                Target.localPosition = vector;
            }
        }

        #endregion
    }
}
