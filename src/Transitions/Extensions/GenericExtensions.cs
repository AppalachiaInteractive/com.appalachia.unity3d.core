using Appalachia.Core.Transitions.Methods;
using UnityEngine;

namespace Appalachia.Core.Transitions.Extensions
{
    public static class GenericExtensions
    {
        /// <summary>This will pause the animation for the specified amount of seconds.</summary>
        public static T DelayTransition<T>(this T target, float duration)
            where T : Component
        {
            AppaDelay.Register(duration);
            return target;
        }

        public static T EventTransition<T>(this T target, System.Action action, float duration = 0.0f)
            where T : Component
        {
            AppaEvent.Register(action, duration);
            return target;
        }

        /// <summary>This will give you the previously registered transition state.</summary>

        // ReSharper disable once UnusedParameter.Global
        public static AppaTransitionState GetTransition<T>(this T target)
            where T : Component
        {
            return AppaTransition.PreviousState;
        }

        /// <summary>This will insert all transitions inside the specified GameObject, as if they were added manually.</summary>
        public static T InsertTransition<T>(this T target, GameObject root)
            where T : Component
        {
            AppaTransition.InsertTransitions(root);
            return target;
        }

        /// <summary>This will insert all transitions inside the specified GameObject, as if they were added manually.</summary>
        public static T InsertTransition<T>(this T target, Transform root)
            where T : Component
        {
            AppaTransition.InsertTransitions(root);
            return target;
        }

        /// <summary>
        ///     This allows you to connect the previous and next transitions, and insert a delay. This means the next transition will only begin when the
        ///     previous one finishes.
        /// </summary>
        public static T JoinDelayTransition<T>(this T target, float delay)
            where T : Component
        {
            AppaTransition.CurrentQueue = AppaTransition.PreviousState;
            AppaDelay.Register(delay);
            AppaTransition.CurrentQueue = AppaTransition.PreviousState;
            return target;
        }

        /// <summary>This will insert all transitions inside the specified GameObject, as if they were added manually.</summary>
        public static T JoinInsertTransition<T>(this T target, GameObject root, float speed = 1.0f)
            where T : Component
        {
            AppaTransition.CurrentQueue = AppaTransition.PreviousState;
            AppaTransition.InsertTransitions(root, speed);
            AppaTransition.CurrentQueue = AppaTransition.PreviousState;
            return target;
        }

        /// <summary>This will insert all transitions inside the specified GameObject, as if they were added manually.</summary>
        public static T JoinInsertTransition<T>(this T target, Transform root, float speed = 1.0f)
            where T : Component
        {
            AppaTransition.CurrentQueue = AppaTransition.PreviousState;
            AppaTransition.InsertTransitions(root, speed);
            AppaTransition.CurrentQueue = AppaTransition.PreviousState;
            return target;
        }

        /// <summary>This allows you to connect the previous and next transitions. This means the next transition will only begin when the previous one finishes.</summary>
        public static T JoinTransition<T>(this T target)
            where T : Component
        {
            AppaTransition.CurrentQueue = AppaTransition.PreviousState;
            return target;
        }

        public static T PlaySoundTransition<T>(
            this T target,
            AudioClip clip,
            float duration = 0.0f,
            float volume = 1.0f)
            where T : Component
        {
            AppaPlaySound.Register(clip, duration, volume);
            return target;
        }

        /// <summary>This allows you to specify which transition must finish before the next transition can begin.</summary>
        public static T QueueTransition<T>(this T target, AppaTransitionState beginAfter)
            where T : Component
        {
            AppaTransition.CurrentQueue = beginAfter;
            return target;
        }

        public static T timeScaleTransition<T>(
            this T target,
            float timeScale,
            float duration,
            AppaEase ease = AppaEase.Smooth)
            where T : Component
        {
            AppaTimeScale.Register(timeScale, duration, ease);
            return target;
        }
    }
}
