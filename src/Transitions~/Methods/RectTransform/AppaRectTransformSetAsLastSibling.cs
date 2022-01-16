using System.Collections.Generic;
using UnityEngine;

namespace Appalachia.Core.Transitions.Methods.RectTransform
{
    /// <summary>This component calls the <b>RectTransform.SetAsLastSibling</b> method when this transition completes.</summary>
    [AddComponentMenu(
        AppaTransition.MethodsMenuPrefix +
        "RectTransform/RectTransform.SetAsLastSibling" +
        AppaTransition.MethodsMenuSuffix +
        nameof(AppaRectTransformSetAsLastSibling)
    )]
    public class AppaRectTransformSetAsLastSibling : AppaMethodWithStateAndTarget
    {
        #region Fields and Autoproperties

        public State Data;

        #endregion

        public static AppaTransitionState Register(UnityEngine.RectTransform target, float duration)
        {
            var state = AppaTransition.SpawnWithTarget(State.Pool, target);

            return AppaTransition.Register(state, duration);
        }

        public override System.Type GetTargetType()
        {
            return typeof(UnityEngine.RectTransform);
        }

        public override void Register()
        {
            PreviousState = Register(GetAliasedTarget(Data.Target), Data.Duration);
        }

        #region Nested type: State

        [System.Serializable]
        public class State : AppaTransitionStateWithTarget<UnityEngine.RectTransform>
        {
            #region Static Fields and Autoproperties

            public static Stack<State> Pool = new Stack<State>();

            #endregion

            public override void Despawn()
            {
                Pool.Push(this);
            }

            public override void UpdateWithTarget(float progress)
            {
                if (progress == 1.0f)
                {
                    Target.SetAsLastSibling();
                }
            }
        }

        #endregion
    }
}
