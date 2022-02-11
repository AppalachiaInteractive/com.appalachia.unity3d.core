using System.Collections.Generic;
using UnityEngine;

namespace Appalachia.Core.Transitions.Methods.GameObject
{
    /// <summary>This component will call <b>GameObject.SetActive</b> with the specified <b>Active</b> state when this transition completes.</summary>
    [AddComponentMenu(
        AppaTransition.MethodsMenuPrefix +
        "GameObject/GameObject.SetActive" +
        AppaTransition.MethodsMenuSuffix +
        nameof(AppaGameObjectSetActive)
    )]
    public class AppaGameObjectSetActive : AppaMethodWithStateAndTarget
    {
        #region Fields and Autoproperties

        public State Data;

        #endregion

        public static AppaTransitionState Register(UnityEngine.GameObject target, bool active, float duration)
        {
            var state = AppaTransition.SpawnWithTarget(State.Pool, target);

            state.Active = active;

            return AppaTransition.Register(state, duration);
        }

        public override System.Type GetTargetType()
        {
            return typeof(UnityEngine.GameObject);
        }

        public override void Register()
        {
            PreviousState = Register(GetAliasedTarget(Data.Target), Data.Active, Data.Duration);
        }

        #region Nested type: State

        [System.Serializable]
        public class State : AppaTransitionStateWithTarget<UnityEngine.GameObject>
        {
            #region Static Fields and Autoproperties

            public static Stack<State> Pool = new Stack<State>();

            #endregion

            #region Fields and Autoproperties

            [PropertyTooltip("The state we will transition to.")]
            public bool Active;

            #endregion

            public override int CanFill => (Target != null) && (Target.activeSelf != Active) ? 1 : 0;

            public override void Despawn()
            {
                Pool.Push(this);
            }

            public override void FillWithTarget()
            {
                Active = Target.activeSelf;
            }

            public override void UpdateWithTarget(float progress)
            {
                if (progress == 1.0f)
                {
                    Target.SetActive(Active);
                }
            }
        }

        #endregion
    }
}
