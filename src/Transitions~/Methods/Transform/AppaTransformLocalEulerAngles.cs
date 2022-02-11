using System.Collections.Generic;
using UnityEngine;

namespace Appalachia.Core.Transitions.Methods.Transform
{
    /// <summary>This component allows you to transition the specified Transform.localEulerAngles to the target value.</summary>
    [AddComponentMenu(
        AppaTransition.MethodsMenuPrefix +
        "Transform/Transform.localEulerAngles" +
        AppaTransition.MethodsMenuSuffix +
        nameof(AppaTransformLocalEulerAngles)
    )]
    public class AppaTransformLocalEulerAngles : AppaMethodWithStateAndTarget
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

            [PropertyTooltip("The ease method that will be used for the transition.")]
            public AppaEase Ease = AppaEase.Smooth;

            [PropertyTooltip("The rotation we will transition to.")]
            public Vector3 Rotation;

            [System.NonSerialized] private Vector3 oldRotation;

            #endregion

            public override int CanFill => (Target != null) && (Target.localEulerAngles != Rotation) ? 1 : 0;

            public override void BeginWithTarget()
            {
                oldRotation = Target.localEulerAngles;
            }

            public override void Despawn()
            {
                Pool.Push(this);
            }

            public override void FillWithTarget()
            {
                Rotation = Target.localEulerAngles;
            }

            public override void UpdateWithTarget(float progress)
            {
                var rotation = Vector3.LerpUnclamped(oldRotation, Rotation, Smooth(Ease, progress));

                Target.localRotation = Quaternion.Euler(rotation);
            }
        }

        #endregion
    }
}
