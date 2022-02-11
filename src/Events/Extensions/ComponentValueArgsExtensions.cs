using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Events.Extensions
{
    public static class ComponentValueArgsExtensions
    {
        /// <summary>
        ///     Invokes the event, using the provided arguments to generate the necessary delegate handler.
        /// </summary>
        /// <param name="eventHandler">The event to invoke.</param>
        /// <param name="component">The component invoking the event.</param>
        /// <param name="value">The current value.</param>
        /// <typeparam name="T">The type of component.</typeparam>
        /// <typeparam name="TV">The value type.</typeparam>
        public static void RaiseEvent<T, TV>(
            this ComponentValueEvent<T, TV>.Data eventHandler,
            T component,
            TV value)
            where T : Component
        {
            using (_PRF_RaiseEvent.Auto())
            {
                if (eventHandler.Subscribers == null)
                {
                    return;
                }

                var args = ToArgs(component, value);
                eventHandler.Subscribers.InvokeSafe(subscriber => subscriber.Invoke(args), args);
            }
        }

        /// <summary>
        ///     Provides a disposable delegate wrapper for the component.
        /// </summary>
        /// <param name="component">The component instance.</param>
        /// <param name="value">The current value.</param>
        /// <typeparam name="T">The type of component.</typeparam>
        /// <typeparam name="TV">The value type.</typeparam>
        /// <returns>The wrapper.  Remember to dispose!</returns>
        public static ComponentValueEvent<T, TV>.Args ToArgs<T, TV>(this T component, TV value)
            where T : Component
        {
            using (_PRF_ToArgs.Auto())
            {
                var instance = ComponentValueEvent<T, TV>.Args.Get();
                instance.component = component;
                instance.value = value;

                return instance;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ComponentValueArgsExtensions) + ".";

        private static readonly ProfilerMarker _PRF_RaiseEvent =
            new ProfilerMarker(_PRF_PFX + nameof(RaiseEvent));

        private static readonly ProfilerMarker _PRF_ToArgs = new ProfilerMarker(_PRF_PFX + nameof(ToArgs));

        #endregion
    }
}
