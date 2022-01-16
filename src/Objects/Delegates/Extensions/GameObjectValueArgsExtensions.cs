using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Delegates.Extensions
{
    public static class GameObjectValueArgsExtensions
    {
        /// <summary>
        ///     Invokes the event, using the provided arguments to generate the necessary delegate handler.
        /// </summary>
        /// <param name="eventHandler">The event to invoke.</param>
        /// <param name="gameObject">The <see cref="GameObject" /> invoking the event.</param>
        /// <param name="value">The current value.</param>
        /// <typeparam name="TV">The value type.</typeparam>
        public static void RaiseEvent<TV>(
            this GameObjectValueArgs<TV>.Handler eventHandler,
            GameObject gameObject,
            TV value)
        {
            using (_PRF_RaiseEvent.Auto())
            {
                if (eventHandler == null)
                {
                    return;
                }

                using (var args = ToArgs(gameObject, value))
                {
                    eventHandler.Invoke(args);
                }
            }
        }

        /// <summary>
        ///     Provides a disposable delegate wrapper for the component.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject" /> instance.</param>
        /// <param name="value">The current value.</param>
        /// <typeparam name="TV">The value type.</typeparam>
        /// <returns>The wrapper.  Remember to dispose!</returns>
        public static GameObjectValueArgs<TV> ToArgs<TV>(this GameObject gameObject, TV value)
        {
            using (_PRF_ToArgs.Auto())
            {
                var instance = GameObjectValueArgs<TV>.Get();
                instance.gameObject = gameObject;
                instance.value = value;

                return instance;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(GameObjectValueArgsExtensions) + ".";

        private static readonly ProfilerMarker _PRF_RaiseEvent =
            new ProfilerMarker(_PRF_PFX + nameof(RaiseEvent));

        private static readonly ProfilerMarker _PRF_ToArgs = new ProfilerMarker(_PRF_PFX + nameof(ToArgs));

        #endregion
    }
}
