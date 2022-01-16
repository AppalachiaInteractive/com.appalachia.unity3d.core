using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Delegates.Extensions
{
    public static class ComponentArgsExtensions
    {
        /// <summary>
        ///     Invokes the event, using the provided arguments to generate the necessary delegate handler.
        /// </summary>
        /// <param name="eventHandler">The event to invoke.</param>
        /// <param name="component">The component invoking the event.</param>
        /// <typeparam name="T">The type of component.</typeparam>
        public static void RaiseEvent<T>(this ComponentArgs<T>.Handler eventHandler, T component)
            where T : Component
        {
            using (_PRF_RaiseEvent.Auto())
            {
                if (eventHandler == null)
                {
                    return;
                }

                using (var args = ToArgs(component))
                {
                    eventHandler.Invoke(args);
                }
            }
        }

        /// <summary>
        ///     Provides a disposable delegate wrapper for the component.
        /// </summary>
        /// <param name="component">The component instance.</param>
        /// <typeparam name="T">The type of component.</typeparam>
        /// <returns>The wrapper.  Remember to dispose!</returns>
        public static ComponentArgs<T> ToArgs<T>(this T component)
            where T : Component
        {
            using (_PRF_ToArgs.Auto())
            {
                var instance = ComponentArgs<T>.Get();
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
