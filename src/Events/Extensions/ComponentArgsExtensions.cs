using System.Runtime.CompilerServices;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Events.Extensions
{
    public static class ComponentArgsExtensions
    {
        /// <summary>
        ///     Invokes the event, using the provided arguments to generate the necessary delegate handler.
        /// </summary>
        /// <param name="eventHandler">The event to invoke.</param>
        /// <param name="component">The component invoking the event.</param>
        /// <param name="callerFilePath">Do not provide a value for this argument.  It will be populated by the compiler.</param>
        /// <param name="callerMemberName">Do not provide a value for this argument.  It will be populated by the compiler.</param>
        /// <param name="callerLineNumber">Do not provide a value for this argument.  It will be populated by the compiler.</param>
        /// <typeparam name="T">The type of component.</typeparam>
        public static void RaiseEvent<T>(
            this ComponentEvent<T>.Data eventHandler,
            T component,
            [CallerFilePath] string callerFilePath = null,
            [CallerMemberName] string callerMemberName = null,
            [CallerLineNumber] int callerLineNumber = 0)
            where T : Component
        {
            using (_PRF_RaiseEvent.Auto())
            {
                if (eventHandler.Subscribers == null)
                {
                    return;
                }

                var args = ToArgs(component);
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
        ///     Provides a disposable delegate wrapper for the component.
        /// </summary>
        /// <param name="component">The component instance.</param>
        /// <typeparam name="T">The type of component.</typeparam>
        /// <returns>The wrapper.  Remember to dispose!</returns>
        public static ComponentEvent<T>.Args ToArgs<T>(this T component)
            where T : Component
        {
            using (_PRF_ToArgs.Auto())
            {
                var instance = ComponentEvent<T>.Args.Get();
                instance.component = component;

                return instance;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ComponentArgsExtensions) + ".";

        private static readonly ProfilerMarker _PRF_RaiseEvent =
            new ProfilerMarker(_PRF_PFX + nameof(RaiseEvent));

        private static readonly ProfilerMarker _PRF_ToArgs = new ProfilerMarker(_PRF_PFX + nameof(ToArgs));

        #endregion
    }
}
