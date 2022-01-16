namespace Appalachia.Core.Transitions.Methods.RectTransform
{
    /// <summary>This component allows you to transition the RectTransform's sizeDelta.x value.</summary>
    [UnityEngine.AddComponentMenu(
        AppaTransition.MethodsMenuPrefix +
        "RectTransform/RectTransform.sizeDelta.x" +
        AppaTransition.MethodsMenuSuffix +
        nameof(AppaRectTransformSizeDelta_x)
    )]
    public class AppaRectTransformSizeDelta_x : AppaMethodWithStateAndTarget
    {
        #region Fields and Autoproperties

        public State Data;

        #endregion

        public static AppaTransitionState Register(
            UnityEngine.RectTransform target,
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
            return typeof(UnityEngine.RectTransform);
        }

        public override void Register()
        {
            PreviousState = Register(GetAliasedTarget(Data.Target), Data.Value, Data.Duration, Data.Ease);
        }

        #region Nested type: State

        [System.Serializable]
        public class State : AppaTransitionStateWithTarget<UnityEngine.RectTransform>
        {
            #region Static Fields and Autoproperties

            public static System.Collections.Generic.Stack<State> Pool =
                new System.Collections.Generic.Stack<State>();

            #endregion

            #region Fields and Autoproperties

            [UnityEngine.Tooltip("This allows you to control how the transition will look.")]
            public AppaEase Ease = AppaEase.Smooth;

            [UnityEngine.Tooltip("The sizeDelta value will transition to this.")]
            public float Value;

            [System.NonSerialized] private float oldValue;

            #endregion

            public override int CanFill => (Target != null) && (Target.sizeDelta.x != Value) ? 1 : 0;

            public override void BeginWithTarget()
            {
                oldValue = Target.sizeDelta.x;
            }

            public override void Despawn()
            {
                Pool.Push(this);
            }

            public override void FillWithTarget()
            {
                Value = Target.sizeDelta.x;
            }

            public override void UpdateWithTarget(float progress)
            {
                var vector = Target.sizeDelta;

                vector.x = UnityEngine.Mathf.LerpUnclamped(oldValue, Value, Smooth(Ease, progress));

                Target.sizeDelta = vector;
            }
        }

        #endregion
    }
}
