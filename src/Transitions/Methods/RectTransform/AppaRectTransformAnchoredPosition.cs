namespace Appalachia.Core.Transitions.Methods.RectTransform
{
    /// <summary>This component allows you to transition the RectTransform's anchoredPosition value.</summary>
    [UnityEngine.AddComponentMenu(
        AppaTransition.MethodsMenuPrefix +
        "RectTransform/RectTransform.anchoredPosition" +
        AppaTransition.MethodsMenuSuffix +
        nameof(AppaRectTransformAnchoredPosition)
    )]
    public class AppaRectTransformAnchoredPosition : AppaMethodWithStateAndTarget
    {
        #region Fields and Autoproperties

        public State Data;

        #endregion

        public static AppaTransitionState Register(
            UnityEngine.RectTransform target,
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

            [UnityEngine.Tooltip("The anchoredPosition value will transition to this.")]
            [UnityEngine.Serialization.FormerlySerializedAs("Position")]
            public UnityEngine.Vector2 Value;

            [System.NonSerialized] private UnityEngine.Vector2 oldValue;

            #endregion

            public override int CanFill => (Target != null) && (Target.anchoredPosition != Value) ? 1 : 0;

            public override void BeginWithTarget()
            {
                oldValue = Target.anchoredPosition;
            }

            public override void Despawn()
            {
                Pool.Push(this);
            }

            public override void FillWithTarget()
            {
                Value = Target.anchoredPosition;
            }

            public override void UpdateWithTarget(float progress)
            {
                Target.anchoredPosition = UnityEngine.Vector2.LerpUnclamped(
                    oldValue,
                    Value,
                    Smooth(Ease, progress)
                );
            }
        }

        #endregion
    }
}
