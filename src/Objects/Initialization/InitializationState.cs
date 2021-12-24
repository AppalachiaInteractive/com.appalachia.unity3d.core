using System;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Logging;
using Appalachia.Utility.Strings;
using Unity.Profiling;

namespace Appalachia.Core.Objects.Initialization
{
    public class InitializationState
    {
        public InitializationState(Func<AppaTask> initializationAction)
        {
            _initializationAction = initializationAction;
        }

        #region Fields and Autoproperties

        public bool hasInitializationFinished;
        public bool hasInitializationStarted;
        public bool hasInvokedInitializationCompleted;
        private readonly Func<AppaTask> _initializationAction;

        #endregion

        public async AppaTask Initialize(InitializationCompleteHandler handler, IInitializable target)
        {
            try
            {
                if (IsInitializationInProgress())
                {
                    return;
                }

                var task = PrepareInitializationTask(target);
                await task;

                OnFinishInitialization(handler, target);
            }
            catch (Exception ex)
            {
                AppaLog.Context.Initialize.Error(
                    ZString.Format(
                        "{0} in [{1}] for [{2}]: {3}",
                        ex.GetType().FormatForLogging(),
                        nameof(Initialize).FormatMethodForLogging(),
                        target.Name.FormatNameForLogging(),
                        ex.Message
                    ),
                    target,
                    ex
                );

                throw;
            }
        }

        public void InitializeSynchronous(InitializationCompleteHandler handler, IInitializable target)
        {
            using (_PRF_InitializeImmediate.Auto())
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
                            "{0} in [{1}] for [{2}]: {3}",
                            ex.GetType().FormatForLogging(),
                            nameof(InitializeSynchronous).FormatMethodForLogging(),
                            target.Name.FormatNameForLogging(),
                            ex.Message
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
                return hasInitializationStarted && !hasInitializationFinished;
            }
        }

        private void OnFinishInitialization(InitializationCompleteHandler handler, IInitializable target)
        {
            using (_PRF_InvokeInitializationCompleted.Auto())
            {
                try
                {
                    if (!hasInvokedInitializationCompleted)
                    {
                        hasInvokedInitializationCompleted = true;

                        handler?.Invoke(target);
                    }

                    hasInitializationFinished = true;
                }
                catch (Exception ex)
                {
                    AppaLog.Context.Initialize.Error(
                        ZString.Format(
                            "{0} in [{1}] for [{2}]: {3}",
                            ex.GetType().FormatForLogging(),
                            nameof(OnFinishInitialization).FormatMethodForLogging(),
                            target.Name.FormatNameForLogging(),
                            ex.Message
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
                    hasInitializationStarted = true;
                    hasInitializationFinished = false;

                    var task = _initializationAction();

                    return task;
                }
                catch (Exception ex)
                {
                    AppaLog.Context.Initialize.Error(
                        ZString.Format(
                            "{0} in [{1}] for [{2}]: {3}",
                            ex.GetType().FormatForLogging(),
                            nameof(PrepareInitializationTask).FormatMethodForLogging(),
                            target.Name.FormatNameForLogging(),
                            ex.Message
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

        private static readonly ProfilerMarker _PRF_InitializeImmediate =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeSynchronous));

        private static readonly ProfilerMarker _PRF_InvokeInitializationCompleted =
            new ProfilerMarker(_PRF_PFX + nameof(OnFinishInitialization));

        #endregion
    }
}
