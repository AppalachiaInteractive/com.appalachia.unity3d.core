using System.Collections.Generic;
using UnityEngine;

namespace Appalachia.Core.Transitions.Methods.Transform
{
    /// <summary>This component allows you to transition the specified Transform.Translate to the target value.</summary>
    [AddComponentMenu(
        AppaTransition.MethodsMenuPrefix +
        "Transform/Transform.Translate" +
        AppaTransition.MethodsMenuSuffix +
        nameof(AppaTransformTranslate)
    )]
    public class AppaTransformTranslate : AppaMethodWithStateAndTarget
    {
        #region Fields and Autoproperties

        public State Data;

        #endregion

        public static AppaTransitionState Register(
            UnityEngine.Transform target,
            Vector3 translation,
            UnityEngine.Transform relativeTo,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            var state = AppaTransition.SpawnWithTarget(State.Pool, target);

            state.Translation = translation;
            state.Space = Space.Self;
            state.RelativeTo = relativeTo;
            state.Ease = ease;

            return AppaTransition.Register(state, duration);
        }

        public static AppaTransitionState Register(
            UnityEngine.Transform target,
            Vector3 translation,
            Space space,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            var state = AppaTransition.SpawnWithTarget(State.Pool, target);

            state.Translation = translation;
            state.Space = space;
            state.RelativeTo = null;
            state.Ease = ease;

            return AppaTransition.Register(state, duration);
        }

        public override System.Type GetTargetType()
        {
            return typeof(UnityEngine.Transform);
        }

        public override void Register()
        {
            if (Data.RelativeTo != null)
            {
                PreviousState = Register(
                    GetAliasedTarget(Data.Target),
                    Data.Translation,
                    Data.RelativeTo,
                    Data.Duration,
                    Data.Ease
                );
            }
            else
            {
                PreviousState = Register(
                    GetAliasedTarget(Data.Target),
                    Data.Translation,
                    Data.Space,
                    Data.Duration,
                    Data.Ease
                );
            }
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

            [PropertyTooltip("The space we will transition in.")]
            public Space Space = Space.Self;

            [PropertyTooltip("The space we will transition in.")]
            public UnityEngine.Transform RelativeTo;

            [PropertyTooltip("The amount we will translate.")]
            public Vector3 Translation;

            [System.NonSerialized] private Vector3 oldTranslation;

            #endregion

            public override ConflictType Conflict => ConflictType.None;

            public override void BeginWithTarget()
            {
                oldTranslation = Vector3.zero;
            }

            public override void Despawn()
            {
                Pool.Push(this);
            }

            public override void UpdateWithTarget(float progress)
            {
                var newTranslation = Translation * Smooth(Ease, progress);

                if (RelativeTo != null)
                {
                    Target.Translate(newTranslation - oldTranslation, RelativeTo);
                }
                else
                {
                    Target.Translate(newTranslation - oldTranslation, Space);
                }

                oldTranslation = newTranslation;
            }
        }

        #endregion
    }
}
