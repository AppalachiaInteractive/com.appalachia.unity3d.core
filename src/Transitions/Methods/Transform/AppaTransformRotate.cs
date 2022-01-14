using System.Collections.Generic;
using UnityEngine;

namespace Appalachia.Core.Transitions.Methods.Transform
{
    /// <summary>This component allows you to rotate the specified Transform by the target value.</summary>
    [AddComponentMenu(
        AppaTransition.MethodsMenuPrefix +
        "Transform/Transform.Rotate" +
        AppaTransition.MethodsMenuSuffix +
        nameof(AppaTransformRotate)
    )]
    public class AppaTransformRotate : AppaMethodWithStateAndTarget
    {
        #region Fields and Autoproperties

        public State Data;

        #endregion

        public static AppaTransitionState Register(
            UnityEngine.Transform target,
            Vector3 eulerAngles,
            Space space,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            var state = AppaTransition.SpawnWithTarget(State.Pool, target);

            state.EulerAngles = eulerAngles;
            state.Space = space;
            state.Ease = ease;

            return AppaTransition.Register(state, duration);
        }

        public override System.Type GetTargetType()
        {
            return typeof(UnityEngine.Transform);
        }

        public override void Register()
        {
            PreviousState = Register(
                GetAliasedTarget(Data.Target),
                Data.EulerAngles,
                Data.Space,
                Data.Duration,
                Data.Ease
            );
        }

        #region Nested type: State

        [System.Serializable]
        public class State : AppaTransitionStateWithTarget<UnityEngine.Transform>
        {
            #region Static Fields and Autoproperties

            public static Stack<State> Pool = new Stack<State>();

            #endregion

            #region Fields and Autoproperties

            [Tooltip("The ease method that will be used for the transition.")]
            public AppaEase Ease = AppaEase.Smooth;

            [Tooltip("The space we will transition in.")]
            public Space Space = Space.Self;

            [Tooltip("The amount we will rotate.")]
            public Vector3 EulerAngles;

            [System.NonSerialized] private Vector3 previousEulerAngles;

            #endregion

            public override ConflictType Conflict => ConflictType.None;

            public override void BeginWithTarget()
            {
                previousEulerAngles = Vector3.zero;
            }

            public override void Despawn()
            {
                Pool.Push(this);
            }

            public override void UpdateWithTarget(float progress)
            {
                var eulerAngles = EulerAngles * Smooth(Ease, progress);

                Target.Rotate(eulerAngles - previousEulerAngles, Space);

                previousEulerAngles = eulerAngles;
            }
        }

        #endregion
    }
}
