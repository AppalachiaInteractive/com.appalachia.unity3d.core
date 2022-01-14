using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Appalachia.Core.Transitions.Methods
{
    /// <summary>This component allows you to invoke a custom action after the specified duration.</summary>
    [AddComponentMenu(
        AppaTransition.MethodsMenuPrefix + "Event" + AppaTransition.MethodsMenuSuffix + nameof(AppaEvent)
    )]
    public class AppaEvent : AppaMethodWithState
    {
        #region Fields and Autoproperties

        public State Data;

        #endregion

        public static AppaTransitionState Register(System.Action action, float duration)
        {
            var state = AppaTransition.Spawn(State.Pool);

            state.Action = action;
            state.Event = null;

            return AppaTransition.Register(state, duration);
        }

        public static AppaTransitionState Register(UnityEvent action, float duration)
        {
            var state = AppaTransition.Spawn(State.Pool);

            state.Action = null;
            state.Event = action;

            return AppaTransition.Register(state, duration);
        }

        public override void Register()
        {
            PreviousState = Register(Data.Event, Data.Duration);
        }

        #region Nested type: State

        [System.Serializable]
        public class State : AppaTransitionState
        {
            #region Static Fields and Autoproperties

            public static Stack<State> Pool = new Stack<State>();

            #endregion

            #region Fields and Autoproperties

            [System.NonSerialized] public System.Action Action;

            [Tooltip("The event that will be invoked.")]
            public UnityEvent Event;

            #endregion

            public override ConflictType Conflict => ConflictType.None;

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
                if (progress == 1.0f)
                {
                    if (Event != null)
                    {
                        Event.Invoke();
                    }

                    if (Action != null)
                    {
                        Action.Invoke();
                    }
                }
            }
        }

        #endregion
    }
}
