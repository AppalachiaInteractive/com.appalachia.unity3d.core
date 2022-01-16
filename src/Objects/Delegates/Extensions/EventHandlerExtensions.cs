using Unity.Profiling;

namespace Appalachia.Core.Objects.Delegates.Extensions
{
    public static class EventHandlerExtensions
    {
        /// <summary>
        ///     Invokes the event, using the provided arguments to generate the necessary delegate handler.
        /// </summary>
        /// <param name="eventHandler">The event to invoke.</param>
        public static void RaiseEvent(this EventHandler eventHandler)
        {
            using (_PRF_RaiseEvent.Auto())
            {
                if (eventHandler == null)
                {
                    return;
                }

                eventHandler.Invoke();
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ComponentArgsExtensions) + ".";

        private static readonly ProfilerMarker _PRF_RaiseEvent =
            new ProfilerMarker(_PRF_PFX + nameof(RaiseEvent));

        #endregion
    }
}
