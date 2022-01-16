namespace Appalachia.Core.Transitions.Methods.CanvasGroup
{
    /// <summary>This component allows you to transition the CanvasGroup's interactable value.</summary>
    [UnityEngine.AddComponentMenu(
        AppaTransition.MethodsMenuPrefix +
        "CanvasGroup/CanvasGroup.interactable" +
        AppaTransition.MethodsMenuSuffix +
        nameof(AppaCanvasGroupInteractable)
    )]
    public class AppaCanvasGroupInteractable : AppaMethodWithStateAndTarget
    {
        #region Fields and Autoproperties

        public State Data;

        #endregion

        public static AppaTransitionState Register(UnityEngine.CanvasGroup target, bool value, float duration)
        {
            var state = AppaTransition.SpawnWithTarget(State.Pool, target);

            state.Value = value;

            return AppaTransition.Register(state, duration);
        }

        public override System.Type GetTargetType()
        {
            return typeof(UnityEngine.CanvasGroup);
        }

        public override void Register()
        {
            PreviousState = Register(GetAliasedTarget(Data.Target), Data.Value, Data.Duration);
        }

        #region Nested type: State

        [System.Serializable]
        public class State : AppaTransitionStateWithTarget<UnityEngine.CanvasGroup>
        {
            #region Static Fields and Autoproperties

            public static System.Collections.Generic.Stack<State> Pool =
                new System.Collections.Generic.Stack<State>();

            #endregion

            #region Fields and Autoproperties

            [UnityEngine.Tooltip("The interactable value will transition to this.")]
            [UnityEngine.Serialization.FormerlySerializedAs("Interactable")]
            public bool Value = true;

            #endregion

            public override int CanFill => (Target != null) && (Target.interactable != Value) ? 1 : 0;

            public override void BeginWithTarget()
            {
            }

            public override void Despawn()
            {
                Pool.Push(this);
            }

            public override void FillWithTarget()
            {
                Value = Target.interactable;
            }

            public override void UpdateWithTarget(float progress)
            {
                if (progress == 1.0f)
                {
                    Target.interactable = Value;
                }
            }
        }

        #endregion
    }
}
