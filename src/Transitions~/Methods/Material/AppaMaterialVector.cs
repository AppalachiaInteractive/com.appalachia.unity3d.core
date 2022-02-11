using System.Collections.Generic;
using UnityEngine;

namespace Appalachia.Core.Transitions.Methods.Material
{
    /// <summary>This component allows you to transition the specified <b>Material</b>'s <b>vector</b> to the target value.</summary>
    [AddComponentMenu(
        AppaTransition.MethodsMenuPrefix +
        "Material/Material vector" +
        AppaTransition.MethodsMenuSuffix +
        nameof(AppaMaterialVector)
    )]
    public class AppaMaterialVector : AppaMethodWithStateAndTarget
    {
        #region Fields and Autoproperties

        public State Data;

        #endregion

        public static AppaTransitionState Register(
            UnityEngine.Material target,
            string property,
            Vector4 value,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            var state = AppaTransition.SpawnWithTarget(State.Pool, target);

            state.Property = property;
            state.Value = value;
            state.Ease = ease;

            return AppaTransition.Register(state, duration);
        }

        public override System.Type GetTargetType()
        {
            return typeof(UnityEngine.Material);
        }

        public override void Register()
        {
            PreviousState = Register(
                GetAliasedTarget(Data.Target),
                Data.Property,
                Data.Value,
                Data.Duration,
                Data.Ease
            );
        }

        #region Nested type: State

        [System.Serializable]
        public class State : AppaTransitionStateWithTarget<UnityEngine.Material>
        {
            #region Static Fields and Autoproperties

            public static Stack<State> Pool = new Stack<State>();

            #endregion

            #region Fields and Autoproperties

            [PropertyTooltip("The ease method that will be used for the transition.")]
            public AppaEase Ease = AppaEase.Smooth;

            [PropertyTooltip("The name of the vector property in the shader.")]
            public string Property;

            [PropertyTooltip("The value we will transition to.")]
            public Vector4 Value;

            [System.NonSerialized] private Vector4 oldValue;

            #endregion

            public override int CanFill => (Target != null) && (Target.GetVector(Property) != Value) ? 1 : 0;

            public override void BeginWithTarget()
            {
                oldValue = Target.GetVector(Property);
            }

            public override void Despawn()
            {
                Pool.Push(this);
            }

            public override void FillWithTarget()
            {
                Value = Target.GetVector(Property);
            }

            public override void UpdateWithTarget(float progress)
            {
                Target.SetVector(Property, Vector4.LerpUnclamped(oldValue, Value, Smooth(Ease, progress)));
            }
        }

        #endregion
    }
}
