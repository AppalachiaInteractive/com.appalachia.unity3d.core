using Unity.Profiling;

namespace Appalachia.Core.Events.Extensions
{
    public static class AppaEventExtensions
    {
        public static void RaiseEvent(this AppaEvent.Data eventHandler)
        {
            using (_PRF_RaiseEvent.Auto())
            {
                if (eventHandler.Subscribers == null)
                {
                    return;
                }

                eventHandler.Subscribers.InvokeSafe(subscriber => subscriber.Invoke());
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ComponentArgsExtensions) + ".";

        private static readonly ProfilerMarker _PRF_RaiseEvent =
            new ProfilerMarker(_PRF_PFX + nameof(RaiseEvent));

        #endregion
    }
}
