#region

using System;
using Appalachia.Core.Preferences;
using Unity.Profiling;

#endregion

namespace Appalachia.Core.Aspects.Tracing
{
    public class TraceMarkerSet : DisposableAspectSet<TraceMarker>
    {
        public const bool _TRACE_BEFORE_PREFS_READY = false;
        private static PREF<bool> _enabled;

        public static bool InternalDisable;

        private static readonly ProfilerMarker _PRF_TraceMarkerSet_Create =
            new("TraceMarkerSet.Create");

        public static bool Enabled
        {
            get
            {
                if (_enabled == null)
                {
                    var orig = InternalDisable;
                    InternalDisable = true;
                    _enabled = PREFS.REG(TRACE._TRACE_LOG_GROUPING, TRACE._TRACE_LOG_LABEL, false);

                    InternalDisable = orig;
                }

                if (_enabled.IsAwake)
                {
                    return _enabled.v;
                }

                return _TRACE_BEFORE_PREFS_READY;
            }
        }

        protected override TraceMarker Create(
            string typePrefix,
            string memberName,
            string additive,
            int sourceLineNumber)
        {
            using (_PRF_TraceMarkerSet_Create.Auto())
            {
                var markerName =
                    $"[{typePrefix}] [{memberName}]{(additive == null ? string.Empty : $" ({additive})")} [{sourceLineNumber}]";

                var marker = new TraceMarker(markerName);

                return marker;
            }
        }

        public override IDisposable Initiate(TraceMarker instance)
        {
            return instance.Auto();
        }
    }
}
