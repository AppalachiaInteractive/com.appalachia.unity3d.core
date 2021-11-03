using System;
using System.Collections;
using System.Diagnostics;
using Appalachia.Utility.Extensions;
using Unity.Profiling;

namespace Appalachia.Core.Context.Elements
{
    public class AppaCoroutineRunner
    {
        #region Profiling And Tracing Markers

        private const string _PRF_PFX = nameof(AppaCoroutineRunner) + ".";

        private static readonly ProfilerMarker _PRF_ExecuteCoroutineEnumerator =
            new(_PRF_PFX + nameof(ExecuteCoroutineEnumerator));

        private static readonly ProfilerMarker _PRF_ExecuteCoroutine =
            new(_PRF_PFX + nameof(ExecuteCoroutine));

        private static readonly ProfilerMarker _PRF_BeginExecution = new(_PRF_PFX + nameof(BeginExecution));

        private static readonly ProfilerMarker _PRF_EndExecution = new(_PRF_PFX + nameof(EndExecution));

        #endregion

        private bool _cancelOnError = true;

        private bool _forceCancelImmediately;

        private bool _isExecutingCoroutine;

        private DateTime _lastExecutionTime;

        private double _executionTime;

        private int _stepSize = 10;

        private readonly Stopwatch _stopwatch = new();

        public bool IsExecutingCoroutine => _isExecutingCoroutine;

        public DateTime LastExecutionTime => _lastExecutionTime;

        public double ExecutionTime => _executionTime;

        public bool CancelOnError
        {
            get => _cancelOnError;
            set => _cancelOnError = value;
        }

        public bool ForceCancelImmediately
        {
            get => _forceCancelImmediately;
            set => _forceCancelImmediately = value;
        }

        public int StepSize
        {
            get => _stepSize;
            set => _stepSize = value;
        }

        public void ExecuteCoroutine(Func<IEnumerator> coroutine, Action onComplete = null)
        {
            using (_PRF_ExecuteCoroutine.Auto())
            {
                var enumerator = ExecuteCoroutineEnumerator(coroutine, onComplete);

#if UNITY_EDITOR
                Unity.EditorCoroutines.Editor.EditorCoroutineUtility.StartCoroutine(enumerator, this);
#endif
            }
        }

        protected IEnumerator ExecuteCoroutineEnumerator(Func<IEnumerator> coroutine, Action onComplete)
        {
            using (_PRF_ExecuteCoroutineEnumerator.Auto())
            {
                try
                {
                    BeginExecution();

                    var count = -1;

                    var routineResults = coroutine();
                    using (_PRF_ExecuteCoroutineEnumerator.Suspend())
                    {
                        while (true)
                        {
                            if (_forceCancelImmediately)
                            {
                                break;
                            }

                            count += 1;
                            object current = null;

                            try
                            {
                                if (!routineResults.MoveNext())
                                {
                                    break;
                                }

                                current = routineResults.Current;
                            }
                            catch (Exception ex)
                            {
                                UnityEngine.Debug.LogException(ex);

                                if (_cancelOnError)
                                {
                                    break;
                                }
                            }

                            if ((count % _stepSize) == 0)
                            {
                                yield return current;
                            }
                        }
                    }
                }
                finally
                {
                    EndExecution(onComplete);
                }
            }
        }

        protected void BeginExecution()
        {
            using (_PRF_BeginExecution.Auto())
            {
                _isExecutingCoroutine = true;
                _stopwatch.Restart();
            }
        }

        protected void EndExecution(Action onComplete)
        {
            using (_PRF_EndExecution.Auto())
            {
                _stopwatch.Stop();
                _lastExecutionTime = DateTime.Now;
                _executionTime = _stopwatch.Elapsed.TotalSeconds;
                _isExecutingCoroutine = false;

                onComplete?.Invoke();
            }
        }
    }
}
