using System.Runtime.CompilerServices;
using Unity.Profiling;

namespace Appalachia.Core.Events.Extensions
{
    public static class AppaEventExtensions
    {
        /// <summary>
        ///     Invokes the event.
        /// </summary>
        public static void RaiseEvent(
            this AppaEvent.Data eventHandler,
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

                eventHandler.Subscribers.InvokeSafe(
                    subscriber => subscriber.Invoke(),
                    callerFilePath,
                    callerMemberName,
                    callerLineNumber
                );
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ComponentArgsExtensions) + ".";

        private static readonly ProfilerMarker _PRF_RaiseEvent =
            new ProfilerMarker(_PRF_PFX + nameof(RaiseEvent));

        #endregion
    }
}
