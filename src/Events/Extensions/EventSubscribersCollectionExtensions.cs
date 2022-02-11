using System;
using Appalachia.Core.Events.Collections;
using Unity.Profiling;

namespace Appalachia.Core.Events.Extensions
{
    internal static class EventSubscribersCollectionExtensions
    {
        internal static int SubscriberCountSafe<T>(this EventSubscribersCollection<T> collection)
            where T : MulticastDelegate
        {
            using (_PRF_SubscriberCountSafe.Auto())
            {
                if (collection == null)
                {
                    return 0;
                }

                return collection.SubscriberCount;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(EventSubscribersCollectionExtensions) + ".";

        private static readonly ProfilerMarker _PRF_SubscriberCountSafe =
            new ProfilerMarker(_PRF_PFX + nameof(SubscriberCountSafe));

        #endregion
    }
}
