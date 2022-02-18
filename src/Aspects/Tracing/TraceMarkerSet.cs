#region

using System;
using System.Diagnostics;
using Appalachia.Core.Preferences;
using Appalachia.Utility.Strings;
using Unity.Profiling;

#endregion

namespace Appalachia.Core.Aspects.Tracing
{
    [DebuggerStepThrough]
    public class TraceMarkerSet : DisposableAspectSet<TraceMarker>
    {
        #region Constants and Static Readonly

        public const bool _TRACE_BEFORE_PREFS_READY = false;

        #endregion

        #region Preferences

        private static PREF<bool> _enabled;

        #endregion

        #region Static Fields and Autoproperties

        public static bool InternalDisable;

        #endregion

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

        /// <inheritdoc />
        public override IDisposable Initiate(TraceMarker instance)
        {
            return instance.Auto();
        }

        /// <inheritdoc />
        protected override TraceMarker Create(
            string typePrefix,
            string memberName,
            string additive,
            int sourceLineNumber)
        {
            using (_PRF_TraceMarkerSet_Create.Auto())
            {
                var markerName = ZString.Format(
                    "[{0}] [{1}]{2} [{3}]",
                    typePrefix,
                    memberName,
                    additive == null ? string.Empty : ZString.Format(" ({0})", additive),
                    sourceLineNumber
                );

                var marker = new TraceMarker(markerName);

                return marker;
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_TraceMarkerSet_Create = new("TraceMarkerSet.Create");

        #endregion
    }
}
