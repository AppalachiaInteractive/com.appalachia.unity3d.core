using System.Collections.Generic;
using UnityEngine;

namespace Appalachia.Core.Transitions.Methods
{
    /// <summary>This component allows you to delay for a specified duration.</summary>
    [AddComponentMenu(
        AppaTransition.MethodsMenuPrefix + "Delay" + AppaTransition.MethodsMenuSuffix + nameof(AppaDelay)
    )]
    public class AppaDelay : AppaMethodWithState
    {
        #region Fields and Autoproperties

        public State Data;

        #endregion

        public static AppaTransitionState Register(float duration)
        {
            var state = AppaTransition.Spawn(State.Pool);

            return AppaTransition.Register(state, duration);
        }

        public override void Register()
        {
            PreviousState = Register(Data.Duration);
        }

        #region Nested type: State

        [System.Serializable]
        public class State : AppaTransitionState
        {
            #region Static Fields and Autoproperties

            public static Stack<State> Pool = new Stack<State>();

            #endregion

            public override void Begin()
            {
                // No state to begin from
            }

            public override void Despawn()
            {
                Pool.Push(this);
            }

            public override void Update(float progress)
            {
                // No state to update
            }
        }

        #endregion
    }
}
