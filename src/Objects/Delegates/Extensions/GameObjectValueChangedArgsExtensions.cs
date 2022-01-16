using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Delegates.Extensions
{
    public static class GameObjectValueChangedArgsExtensions
    {
        /// <summary>
        ///     Invokes the event, using the provided arguments to generate the necessary delegate handler.
        /// </summary>
        /// <param name="eventHandler">The event to invoke.</param>
        /// <param name="gameObject">The <see cref="GameObject" /> invoking the event.</param>
        /// <param name="previousValue">The previous value.</param>
        /// <param name="value">The current value.</param>
        /// <typeparam name="T">The value type.</typeparam>
        public static void RaiseEvent<T>(
            this GameObjectValueChangedArgs<T>.Handler eventHandler,
            GameObject gameObject,
            T previousValue,
            T value)
        {
            using (_PRF_RaiseEvent.Auto())
            {
                if (eventHandler == null)
                {
                    return;
                }

                using (var args = ToArgs(gameObject, previousValue, value))
                {
                    eventHandler.Invoke(args);
                }
            }
        }

        /// <summary>
        ///     Provides a disposable delegate wrapper for the <see cref="GameObject" />.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject" /> instance.</param>
        /// <param name="previousValue">The previous value.</param>
        /// <param name="value">The current value.</param>
        /// <typeparam name="T">The value type.</typeparam>
        /// <returns>The wrapper.  Remember to dispose!</returns>
        public static GameObjectValueChangedArgs<T> ToArgs<T>(
            this GameObject gameObject,
            T previousValue,
            T value)
        {
            using (_PRF_ToArgs.Auto())
            {
                var instance = GameObjectValueChangedArgs<T>.Get();
                instance.gameObject = gameObject;
                instance.previousValue = previousValue;
                instance.value = value;

                return instance;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(GameObjectValueChangedArgsExtensions) + ".";

        private static readonly ProfilerMarker _PRF_RaiseEvent =
            new ProfilerMarker(_PRF_PFX + nameof(RaiseEvent));

        private static readonly ProfilerMarker _PRF_ToArgs = new ProfilerMarker(_PRF_PFX + nameof(ToArgs));

        #endregion
    }
}
