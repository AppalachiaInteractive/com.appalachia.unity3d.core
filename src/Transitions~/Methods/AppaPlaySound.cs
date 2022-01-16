using System.Collections.Generic;
using UnityEngine;

namespace Appalachia.Core.Transitions.Methods
{
    /// <summary>This component allows you to play a sound after the specified duration.</summary>
    [AddComponentMenu(
        AppaTransition.MethodsMenuPrefix +
        "Play Sound" +
        AppaTransition.MethodsMenuSuffix +
        nameof(AppaPlaySound)
    )]
    public class AppaPlaySound : AppaMethodWithStateAndTarget
    {
        #region Fields and Autoproperties

        public State Data;

        #endregion

        public static AppaTransitionState Register(AudioClip target, float duration, float volume = 1.0f)
        {
            var state = AppaTransition.SpawnWithTarget(State.Pool, target);

            state.Volume = volume;

            return AppaTransition.Register(state, duration);
        }

        public override System.Type GetTargetType()
        {
            return typeof(AudioClip);
        }

        public override void Register()
        {
            PreviousState = Register(GetAliasedTarget(Data.Target), Data.Duration, Data.Volume);
        }

        #region Nested type: State

        [System.Serializable]
        public class State : AppaTransitionStateWithTarget<AudioClip>
        {
            #region Static Fields and Autoproperties

            public static Stack<State> Pool = new Stack<State>();

            #endregion

            #region Fields and Autoproperties

            [Range(0.0f, 1.0f)] public float Volume = 1.0f;

            #endregion

            public override int CanFill => 0;

            public override void Despawn()
            {
                Pool.Push(this);
            }

            public override void UpdateWithTarget(float progress)
            {
                if (progress == 1.0f)
                {
#if UNITY_EDITOR
                    if (Application.isPlaying == false)
                    {
                        return;
                    }
#endif
                    var gameObject = new UnityEngine.GameObject(Target.name);
                    var audioSource = gameObject.AddComponent<UnityEngine.AudioSource>();

                    audioSource.clip = Target;
                    audioSource.volume = Volume;

                    audioSource.Play();

                    Destroy(gameObject, Target.length);
                }
            }
        }

        #endregion
    }
}
