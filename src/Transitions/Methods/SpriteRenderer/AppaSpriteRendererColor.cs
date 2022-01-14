namespace Appalachia.Core.Transitions.Methods.SpriteRenderer
{
    /// <summary>This component allows you to transition the SpriteRenderer's color value.</summary>
    [UnityEngine.AddComponentMenu(
        AppaTransition.MethodsMenuPrefix +
        "SpriteRenderer/SpriteRenderer.color" +
        AppaTransition.MethodsMenuSuffix +
        nameof(AppaSpriteRendererColor)
    )]
    public class AppaSpriteRendererColor : AppaMethodWithStateAndTarget
    {
        #region Fields and Autoproperties

        public State Data;

        #endregion

        public static AppaTransitionState Register(
            UnityEngine.SpriteRenderer target,
            UnityEngine.Color value,
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
            return typeof(UnityEngine.SpriteRenderer);
        }

        public override void Register()
        {
            PreviousState = Register(GetAliasedTarget(Data.Target), Data.Value, Data.Duration, Data.Ease);
        }

        #region Nested type: State

        [System.Serializable]
        public class State : AppaTransitionStateWithTarget<UnityEngine.SpriteRenderer>
        {
            #region Static Fields and Autoproperties

            public static System.Collections.Generic.Stack<State> Pool =
                new System.Collections.Generic.Stack<State>();

            #endregion

            #region Fields and Autoproperties

            [UnityEngine.Tooltip("This allows you to control how the transition will look.")]
            public AppaEase Ease = AppaEase.Smooth;

            [UnityEngine.Tooltip("The color value will transition to this.")]
            [UnityEngine.Serialization.FormerlySerializedAs("Color")]
            public UnityEngine.Color Value = UnityEngine.Color.white;

            [System.NonSerialized] private UnityEngine.Color oldValue;

            #endregion

            public override int CanFill => (Target != null) && (Target.color != Value) ? 1 : 0;

            public override void BeginWithTarget()
            {
                oldValue = Target.color;
            }

            public override void Despawn()
            {
                Pool.Push(this);
            }

            public override void FillWithTarget()
            {
                Value = Target.color;
            }

            public override void UpdateWithTarget(float progress)
            {
                Target.color = UnityEngine.Color.LerpUnclamped(oldValue, Value, Smooth(Ease, progress));
            }
        }

        #endregion
    }
}
