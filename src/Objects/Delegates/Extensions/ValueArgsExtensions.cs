using Unity.Profiling;

namespace Appalachia.Core.Objects.Delegates.Extensions
{
    public static class ValueArgsExtensions
    {
        /// <summary>
        ///     Invokes the event, using the provided arguments to generate the necessary delegate handler.
        /// </summary>
        /// <param name="eventHandler">The event to invoke.</param>
        /// <param name="value">The current value.</param>
        /// <typeparam name="T">The type of component.</typeparam>
        public static void RaiseEvent<T>(this ValueArgs<T>.Handler eventHandler, T value)
        {
            using (_PRF_RaiseEvent.Auto())
            {
                if (eventHandler == null)
                {
                    return;
                }

                using (var args = ToArgs(value))
                {
                    eventHandler.Invoke(args);
                }
            }
        }

        /// <summary>
        ///     Provides a disposable delegate wrapper for the value.
        /// </summary>
        /// <param name="value">The current value.</param>
        /// <typeparam name="T">The value type.</typeparam>
        /// <returns>The wrapper.  Remember to dispose!</returns>
        public static ValueArgs<T> ToArgs<T>(T value)
        {
            using (_PRF_ToArgs.Auto())
            {
                var instance = ValueArgs<T>.Get();
                instance.value = value;

                return instance;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ValueArgsExtensions) + ".";

        private static readonly ProfilerMarker _PRF_RaiseEvent =
            new ProfilerMarker(_PRF_PFX + nameof(RaiseEvent));

        private static readonly ProfilerMarker _PRF_ToArgs = new ProfilerMarker(_PRF_PFX + nameof(ToArgs));

        #endregion
    }
}
