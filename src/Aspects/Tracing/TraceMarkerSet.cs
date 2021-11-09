#region

using System;
using System.Diagnostics;
using Appalachia.Core.Preferences;
using Unity.Profiling;

#endregion

namespace Appalachia.Core.Aspects.Tracing
{
    [DebuggerStepThrough]
    public class TraceMarkerSet : DisposableAspectSet<TraceMarker>
    {
        #region Profiling And Tracing Markers

        private static readonly ProfilerMarker _PRF_TraceMarkerSet_Create = new("TraceMarkerSet.Create");

        #endregion

        #region Constants and Static Readonly

        public const bool _TRACE_BEFORE_PREFS_READY = false;

        #endregion

        #region Preferences

        private static PREF<bool> _enabled;

        #endregion

        public static bool InternalDisable;

        public static bool Enabled
        {
            get
            {
                if (_enabled == null)
                {
                    var orig = InternalDisable;
                    InternalDisable = true;
                    _enabled = PREFS.REG(PKG.Prefs.Group, TRACE._TRACE_LOG_LABEL, false);

                    InternalDisable = orig;
                }

                if (_enabled.IsAwake)
                {
                    return _enabled.v;
                }

                return _TRACE_BEFORE_PREFS_READY;
            }
        }

        public override IDisposable Initiate(TraceMarker instance)
        {
            return instance.Auto();
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
    }
}
