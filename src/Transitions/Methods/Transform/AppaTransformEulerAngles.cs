using System.Collections.Generic;
using UnityEngine;

namespace Appalachia.Core.Transitions.Methods.Transform
{
    /// <summary>This component allows you to transition the specified <b>Transform.eulerAngles</b> to the target value.</summary>
    [AddComponentMenu(
        AppaTransition.MethodsMenuPrefix +
        "Transform/Transform.eulerAngles" +
        AppaTransition.MethodsMenuSuffix +
        nameof(AppaTransformEulerAngles)
    )]
    public class AppaTransformEulerAngles : AppaMethodWithStateAndTarget
    {
        #region Fields and Autoproperties

        public State Data;

        #endregion

        public static AppaTransitionState Register(
            UnityEngine.Transform target,
            Vector3 rotation,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            var state = AppaTransition.SpawnWithTarget(State.Pool, target);

            state.Rotation = rotation;
            state.Ease = ease;

            return AppaTransition.Register(state, duration);
        }

        public override System.Type GetTargetType()
        {
            return typeof(UnityEngine.Transform);
        }

        public override void Register()
        {
            PreviousState = Register(GetAliasedTarget(Data.Target), Data.Rotation, Data.Duration, Data.Ease);
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

            [Tooltip("The rotation we will transition to.")]
            public Vector3 Rotation;

            [System.NonSerialized] private Vector3 oldRotation;

            #endregion

            public override int CanFill => (Target != null) && (Target.eulerAngles != Rotation) ? 1 : 0;

            public override void BeginWithTarget()
            {
                oldRotation = Target.eulerAngles;
            }

            public override void Despawn()
            {
                Pool.Push(this);
            }

            public override void FillWithTarget()
            {
                Rotation = Target.eulerAngles;
            }

            public override void UpdateWithTarget(float progress)
            {
                var rotation = Vector3.LerpUnclamped(oldRotation, Rotation, Smooth(Ease, progress));

                Target.rotation = Quaternion.Euler(rotation);
            }
        }

        #endregion
    }
}
