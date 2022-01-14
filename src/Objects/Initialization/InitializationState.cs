using System;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Logging;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Initialization
{
    public class InitializationState
    {
        public InitializationState(Func<AppaTask> initializationAction)
        {
            _initializationAction = initializationAction;
        }

        #region Fields and Autoproperties

        private readonly Func<AppaTask> _initializationAction;

        private bool _hasInitializationFinished;
        private bool _hasInitializationStarted;
        private bool _hasInvokedInitializationCompleted;

        #endregion

        public bool hasInitializationFinished
        {
            get => _hasInitializationFinished;
            internal set => _hasInitializationFinished = value;
        }

        public bool hasInitializationStarted
        {
            get => _hasInitializationStarted;
            internal set => _hasInitializationStarted = value;
        }

        public bool hasInvokedInitializationCompleted
        {
            get => _hasInvokedInitializationCompleted;
            internal set => _hasInvokedInitializationCompleted = value;
        }

        public async AppaTask Initialize(InitializationCompleteHandler handler, IInitializable target)
        {
            try
            {
                if (IsInitializationInProgress())
                {
                    return;
                }

                var task = PrepareInitializationTask(target);

                if (target is Component c)
                {
                    var cancellationToken = c.GetCancellationTokenOnDestroy();
                    task = task.AttachExternalCancellation(cancellationToken);
                }

                await task;

                OnFinishInitialization(handler, target);
            }
            catch (Exception ex)
            {
                AppaLog.Context.Initialize.Error(
                    ZString.Format(
                        "{0} for {1} on {2} threw an exception.",
                        nameof(Initialize).FormatMethodForLogging(),
                        target.GetType().FormatForLogging(),
                        target.Name.FormatNameForLogging()
                    ),
                    target,
                    ex
                );

                throw;
            }
        }

        public void InitializeSynchronous(InitializationCompleteHandler handler, IInitializable target)
        {
            using (_PRF_InitializeSynchronous.Auto())
            {
                try
                {
                    if (IsInitializationInProgress())
                    {
                        return;
                    }

                    var task = PrepareInitializationTask(target);
                    var coroutine = task.ToCoroutine();

                    while (coroutine.MoveNext())
                    {
                    }

                    OnFinishInitialization(handler, target);
                }
                catch (Exception ex)
                {
                    AppaLog.Context.Initialize.Error(
                        ZString.Format(
                            "{0} for {1} threw an exception.",
                            nameof(InitializeSynchronous).FormatMethodForLogging(),
                            target.Name.FormatNameForLogging()
                        ),
                        target,
                        ex
                    );

                    throw;
                }
            }
        }

        public bool IsInitializationInProgress()
        {
            using (_PRF_IsInitializationInProgress.Auto())
            {
                return _hasInitializationStarted && !_hasInitializationFinished;
            }
        }

        private void OnFinishInitialization(InitializationCompleteHandler handler, IInitializable target)
        {
            using (_PRF_OnFinishInitialization.Auto())
            {
                try
                {
                    if (!_hasInvokedInitializationCompleted)
                    {
                        _hasInvokedInitializationCompleted = true;

                        handler?.Invoke(target);
                    }
                }
                catch (Exception ex)
                {
                    AppaLog.Context.Initialize.Error(
                        ZString.Format(
                            "{0} for {1} threw an exception.",
                            nameof(OnFinishInitialization).FormatMethodForLogging(),
                            target.Name.FormatNameForLogging()
                        ),
                        target,
                        ex
                    );

                    throw;
                }
            }
        }

        private AppaTask PrepareInitializationTask(IInitializable target)
        {
            using (_PRF_PrepareInitializationTask.Auto())
            {
                try
                {
                    _hasInitializationStarted = true;
                    _hasInitializationFinished = false;

                    var task = _initializationAction();

                    return task;
                }
                catch (Exception ex)
                {
                    AppaLog.Context.Initialize.Error(
                        ZString.Format(
                            "{0} for {1} threw an exception.",
                            nameof(PrepareInitializationTask).FormatMethodForLogging(),
                            target.Name.FormatNameForLogging()
                        ),
                        target,
                        ex
                    );

                    throw;
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(InitializationState) + ".";

        private static readonly ProfilerMarker _PRF_IsInitializationInProgress =
            new ProfilerMarker(_PRF_PFX + nameof(IsInitializationInProgress));

        private static readonly ProfilerMarker _PRF_PrepareInitializationTask =
            new ProfilerMarker(_PRF_PFX + nameof(PrepareInitializationTask));

        private static readonly ProfilerMarker _PRF_InitializeSynchronous =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeSynchronous));

        private static readonly ProfilerMarker _PRF_OnFinishInitialization =
            new ProfilerMarker(_PRF_PFX + nameof(OnFinishInitialization));

        #endregion
    }
}
