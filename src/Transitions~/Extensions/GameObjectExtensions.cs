using Appalachia.Core.Transitions.Methods;
using Appalachia.Core.Transitions.Methods.GameObject;
using UnityEngine;

namespace Appalachia.Core.Transitions.Extensions
{
    public static class GameObjectExtensions
    {
        /// <summary>This will pause the animation for the specified amount of seconds.</summary>
        public static GameObject DelayTransition(this GameObject target, float duration)
        {
            AppaDelay.Register(duration);
            return target;
        }

        public static GameObject EventTransition(
            this GameObject target,
            System.Action action,
            float duration = 0.0f)
        {
            AppaEvent.Register(action, duration);
            return target;
        }

        /// <summary>This will give you the previously registered transition state.</summary>

        // ReSharper disable once UnusedParameter.Global
        public static AppaTransitionState GetTransition(this GameObject target)
        {
            return AppaTransition.PreviousState;
        }

        /// <summary>This will insert all transitions inside the specified GameObject, as if they were added manually.</summary>
        public static GameObject InsertTransition(this GameObject target, GameObject root)
        {
            AppaTransition.InsertTransitions(root);
            return target;
        }

        /// <summary>This will insert all transitions inside the specified GameObject, as if they were added manually.</summary>
        public static GameObject InsertTransition(this GameObject target, Transform root)
        {
            AppaTransition.InsertTransitions(root);
            return target;
        }

        /// <summary>
        ///     This allows you to connect the previous and next transitions, and insert a delay. This means the next transition will only begin when the
        ///     previous one finishes.
        /// </summary>
        public static GameObject JoinDelayTransition(this GameObject target, float delay)
        {
            AppaTransition.CurrentQueue = AppaTransition.PreviousState;
            AppaDelay.Register(delay);
            AppaTransition.CurrentQueue = AppaTransition.PreviousState;
            return target;
        }

        /// <summary>This will insert all transitions inside the specified GameObject, as if they were added manually.</summary>
        public static GameObject JoinInsertTransition(
            this GameObject target,
            GameObject root,
            float speed = 1.0f)
        {
            AppaTransition.CurrentQueue = AppaTransition.PreviousState;
            AppaTransition.InsertTransitions(root, speed);
            AppaTransition.CurrentQueue = AppaTransition.PreviousState;
            return target;
        }

        /// <summary>This will insert all transitions inside the specified GameObject, as if they were added manually.</summary>
        public static GameObject JoinInsertTransition(
            this GameObject target,
            Transform root,
            float speed = 1.0f)
        {
            AppaTransition.CurrentQueue = AppaTransition.PreviousState;
            AppaTransition.InsertTransitions(root, speed);
            AppaTransition.CurrentQueue = AppaTransition.PreviousState;
            return target;
        }

        /// <summary>This allows you to connect the previous and next transitions. This means the next transition will only begin when the previous one finishes.</summary>
        public static GameObject JoinTransition(this GameObject target)
        {
            AppaTransition.CurrentQueue = AppaTransition.PreviousState;
            return target;
        }

        public static GameObject PlaySoundTransition(
            this GameObject target,
            AudioClip clip,
            float duration = 0.0f,
            float volume = 1.0f)
        {
            AppaPlaySound.Register(clip, duration, volume);
            return target;
        }

        /// <summary>This allows you to specify which transition must finish before the next transition can begin.</summary>
        public static GameObject QueueTransition(this GameObject target, AppaTransitionState beginAfter)
        {
            AppaTransition.CurrentQueue = beginAfter;
            return target;
        }

        public static GameObject SetActiveTransition(this GameObject target, bool active, float duration)
        {
            AppaGameObjectSetActive.Register(target, active, duration);
            return target;
        }

        public static GameObject timeScaleTransition(
            this GameObject target,
            float timeScale,
            float duration,
            AppaEase ease = AppaEase.Smooth)
        {
            AppaTimeScale.Register(timeScale, duration, ease);
            return target;
        }
    }
}
