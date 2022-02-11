using System.Collections.Generic;
using UnityEngine;

namespace Appalachia.Core.Transitions.Methods.Material
{
    /// <summary>This component allows you to transition the specified <b>Material</b>'s <b>color</b> to the target value.</summary>
    [AddComponentMenu(
        AppaTransition.MethodsMenuPrefix +
        "Material/Material color" +
        AppaTransition.MethodsMenuSuffix +
        nameof(AppaMaterialColor)
    )]
    public class AppaMaterialColor : AppaMethodWithStateAndTarget
    {
        #region Fields and Autoproperties

        public State Data;

        #endregion

        public static AppaTransitionState Register(
            UnityEngine.Material target,
            string property,
            Color color,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            var state = AppaTransition.SpawnWithTarget(State.Pool, target);

            state.Property = property;
            state.Color = color;
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
                Data.Color,
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

            [PropertyTooltip("The color we will transition to.")]
            public Color Color = Color.white;

            [PropertyTooltip("The name of the color property in the shader.")]
            public string Property = "_Color";

            [System.NonSerialized] private Color oldColor;

            #endregion

            public override int CanFill => (Target != null) && (Target.GetColor(Property) != Color) ? 1 : 0;

            public override void BeginWithTarget()
            {
                oldColor = Target.GetColor(Property);
            }

            public override void Despawn()
            {
                Pool.Push(this);
            }

            public override void FillWithTarget()
            {
                Color = Target.GetColor(Property);
            }

            public override void UpdateWithTarget(float progress)
            {
                Target.SetColor(Property, Color.LerpUnclamped(oldColor, Color, Smooth(Ease, progress)));
            }
        }

        #endregion
    }
}
