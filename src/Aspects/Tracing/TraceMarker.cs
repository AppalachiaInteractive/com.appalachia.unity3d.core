#region

using System;
using System.Diagnostics;
using Unity.Profiling;
using Debug = UnityEngine.Debug;

#endregion

namespace Appalachia.Core.Aspects.Tracing
{
    public struct TraceMarker
    {
        public TraceMarker(string message)
        {
            _traceMessage = message;
        }

        public static readonly TraceMarker empty = default;

        private readonly string _traceMessage;

        private static string[] _indents = new string[100];

        private static readonly ProfilerMarker _PRF_TraceMarker_Trace = new("TraceMarker.Trace");

        private static readonly ProfilerMarker _PRF_TraceMarker_Trace_CheckAwake =
            new("TraceMarker.Trace.CheckAwake");

        private static readonly ProfilerMarker _PRF_TraceMarker_Trace_StackTrace =
            new("TraceMarker.Trace.StackTrace");

        private static readonly ProfilerMarker _PRF_TraceMarker_Trace_Format =
            new("TraceMarker.Trace.Format");

        private static readonly ProfilerMarker _PRF_TraceMarker_EnsureInitialized =
            new("TraceMarker.EnsureInitialized");

        public void Trace(TraceType type, bool ignore = false)
        {
            using (_PRF_TraceMarker_Trace.Auto())
            {
                if (ignore || TraceMarkerSet.InternalDisable)
                {
                    return;
                }

                using (_PRF_TraceMarker_Trace_CheckAwake.Auto())
                {
                    if (!TraceMarkerSet.Enabled)
                    {
                        return;
                    }
                }

                EnsureInitialized();

                int frameCount;
                using (_PRF_TraceMarker_Trace_StackTrace.Auto())
                {
                    frameCount = new StackTrace().FrameCount - (type == TraceType.EXIT ? 2 : 3);

                    if (frameCount < 0)
                    {
                        frameCount = 0;
                    }
                }

                using (_PRF_TraceMarker_Trace_Format.Auto())
                {
                    var formatPrefix = _indents[frameCount];

                    Debug.Log($"{formatPrefix}{_traceMessage}: {type}");
                }
            }
        }

        private void EnsureInitialized()
        {
            using (_PRF_TraceMarker_EnsureInitialized.Auto())
            {
                if (_indents == null)
                {
                    _indents = new string[100];
                }

                if (_indents[0] == null)
                {
                    _indents[0] = string.Empty;

                    for (var i = 1; i < 100; i++)
                    {
                        _indents[i] = new string('.', i * 2) + ' ';
                    }
                }
            }
        }

        public AutoScope Auto(bool ignored = false)
        {
            return new(this, ignored);
        }

        public struct AutoScope : IDisposable
        {
            private static readonly ProfilerMarker _PRF_AutoScope_AutoScope =
                new("AutoScope.AutoScope");

            private static readonly ProfilerMarker
                _PRF_AutoScope_Dispose = new("AutoScope.Dispose");

            internal TraceMarker _marker;
            internal bool _ignore;

            internal AutoScope(TraceMarker marker, bool ignore)
            {
                using (_PRF_AutoScope_AutoScope.Auto())
                {
                    _ignore = ignore;
                    _marker = marker;

                    _marker.Trace(TraceType.ENTRY, _ignore);
                }
            }

            public void Dispose()
            {
                using (_PRF_AutoScope_Dispose.Auto())
                {
                    _marker.Trace(TraceType.EXIT, _ignore);
                }
            }
        }
    }
}
