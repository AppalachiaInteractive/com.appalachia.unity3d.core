using System.Runtime.CompilerServices;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Events.Extensions
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
        /// <param name="callerFilePath">Do not provide a value for this argument.  It will be populated by the compiler.</param>
        /// <param name="callerMemberName">Do not provide a value for this argument.  It will be populated by the compiler.</param>
        /// <param name="callerLineNumber">Do not provide a value for this argument.  It will be populated by the compiler.</param>
        public static void RaiseEvent<T>(
            this GameObjectValueChangedEvent<T>.Data eventHandler,
            GameObject gameObject,
            T previousValue,
            T value,
            [CallerFilePath] string callerFilePath = null,
            [CallerMemberName] string callerMemberName = null,
            [CallerLineNumber] int callerLineNumber = 0)
        {
            using (_PRF_RaiseEvent.Auto())
            {
                if (eventHandler.Subscribers == null)
                {
                    return;
                }

                var args = ToArgs(gameObject, previousValue, value);
                eventHandler.Subscribers.InvokeSafe(
                    subscriber => subscriber.Invoke(args),
                    callerFilePath,
                    callerMemberName,
                    callerLineNumber,
                    args
                );
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
        public static GameObjectValueChangedEvent<T>.Args ToArgs<T>(
            this GameObject gameObject,
            T previousValue,
            T value)
        {
            using (_PRF_ToArgs.Auto())
            {
                var instance = GameObjectValueChangedEvent<T>.Args.Get();
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
