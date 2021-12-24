using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Algorithms.Graphs;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.DataStructures.Graphs;
using Appalachia.Utility.Logging;
using Unity.Profiling;

namespace Appalachia.Core.Objects.Dependencies
{
    [CallStaticConstructorInEditor]
    public static class AppalachiaRepositoryDependencyManager
    {
        static AppalachiaRepositoryDependencyManager()
        {
            using (_PRF_AppalachiaDependencyManager.Auto())
            {
                Initialize();
            }
        }

        #region Static Fields and Autoproperties

        [NonSerialized] private static bool _registrationClosed;

        [NonSerialized] private static Dictionary<Type, AppalachiaRepositoryDependencyTracker> _trackerLookup;

        [NonSerialized]
        private static DirectedSparseGraph<AppalachiaRepositoryDependencyTracker> _trackerGraph;

        [NonSerialized] private static List<AppalachiaRepositoryDependencyTracker> _trackers;

        #endregion

        public static bool IsCyclical => CheckIfCyclical();

        private static AppaLogContext LogContext => AppaLog.Context.Dependencies;

        public static void AddTracker(AppalachiaRepositoryDependencyTracker tracker)
        {
            using (_PRF_AddTracker.Auto())
            {
                if (_registrationClosed)
                {
                    throw new DependencyRegistrationClosedException(tracker.Tracking);
                }

                LogContext.Info($"Now tracking {tracker.Tracking}.");

                Initialize();

                _trackers.Add(tracker);

                if (!_trackerLookup.ContainsKey(tracker.Tracking))
                {
                    _trackerLookup.Add(tracker.Tracking, tracker);
                }

                tracker.objectDependencies.DependencyRegistered += OnObjectDependencyRegistered;
                tracker.behaviourDependencies.DependencyRegistered += OnBehaviourDependencyRegistered;
            }
        }

        public static void CloseRegistration(
            [CallerFilePath] string callerFilePath = null,
            [CallerMemberName] string callerMemberName = null,
            [CallerLineNumber] int callerLineNumber = 0)
        {
            using (_PRF_CloseRegistration.Auto())
            {
                LogContext.Warn(
                    $"Dependency registration is now closed!  Closed by {callerFilePath.FormatNameForLogging()}, {callerMemberName.FormatMethodForLogging()}:{callerLineNumber.FormatForLogging()}."
                );
                _registrationClosed = true;
            }
        }

        public static void LogGraph()
        {
            using (_PRF_LogGraph.Auto())
            {
                var msg = _trackerGraph.ToReadable();
                LogContext.Trace(msg);
            }
        }

        public static void RecalculateGraph()
        {
            using (_PRF_CalculateGraph.Auto())
            {
                Initialize();

                _trackerGraph.Clear();
                _trackerGraph.AddVertex(AppalachiaRepository.DependencyTracker);

                foreach (var tracker in _trackers)
                {
                    foreach (var dependency in tracker.repositoryDependencies.Dependencies)
                    {
                        AddEdge(tracker, dependency);
                    }

                    foreach (var dependency in tracker.objectDependencies.Dependencies)
                    {
                        AddEdge(tracker, dependency);
                    }

                    foreach (var dependency in tracker.behaviourDependencies.Dependencies)
                    {
                        AddEdge(tracker, dependency);
                    }
                }

                CheckIfCyclical();
            }
        }

        public static async AppaTask ResolveRepositoryDependencies()
        {
            using (_PRF_ResolveRepositoryDependencies.Auto())
            {
                RecalculateGraph();

                var repositoryDependencies = TopologicalSorter.Sort(_trackerGraph).ToArray();

                var repository = await AppalachiaRepository.AwakeRepository();

                for (var index = repositoryDependencies.Length - 2; index >= 0; index--)
                {
                    var repositoryDependency = repositoryDependencies[index];

                    var tracked = await repository.Find(repositoryDependency.Tracking);

                    LogContext.Debug($"Successfully resolved {repositoryDependency.Tracking}", tracked);
                }
            }
        }

        private static void AddEdge(
            AppalachiaRepositoryDependencyTracker dependent,
            AppalachiaRepositoryDependencyTracker dependency)
        {
            using (_PRF_AddEdge.Auto())
            {
                var trackedType = dependent.Tracking;

                LogContext.Debug(
                    $"Tracked type {trackedType.FormatForLogging()} has registered a new {dependency.behaviourDependencies.FormatForLogging()} dependency on {dependency.FormatForLogging()}."
                );

                _trackerGraph.AddVertex(dependent);
                _trackerGraph.AddVertex(dependency);

                _trackerGraph.AddEdge(dependent, dependency);
            }
        }

        private static bool CheckIfCyclical()
        {
            using (_PRF_CheckIfCyclical.Auto())
            {
                if (CyclesDetector.IsCyclic(_trackerGraph))
                {
                    LogContext.Critical("Cyclical dependencies detected!");
                    return true;
                }

                return false;
            }
        }

        private static void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                _trackers ??= new List<AppalachiaRepositoryDependencyTracker>();
                _trackerLookup ??= new Dictionary<Type, AppalachiaRepositoryDependencyTracker>();

                if (_trackerGraph == null)
                {
                    _trackerGraph = new DirectedSparseGraph<AppalachiaRepositoryDependencyTracker>();
                    _trackerGraph.AddVertex(AppalachiaRepository.DependencyTracker);
                }
            }
        }

        private static void OnBehaviourDependencyRegistered(
            AppalachiaRepositoryDependencyTracker dependent,
            AppalachiaRepositoryDependencyTracker dependency)
        {
            using (_PRF_OnBehaviourDependencyRegistered.Auto())
            {
                AddEdge(dependent, dependency);
            }
        }

        private static void OnObjectDependencyRegistered(
            AppalachiaRepositoryDependencyTracker dependent,
            AppalachiaRepositoryDependencyTracker dependency)
        {
            using (_PRF_OnObjectDependencyRegistered.Auto())
            {
                AddEdge(dependent, dependency);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AppalachiaRepositoryDependencyManager) + ".";

        private static readonly ProfilerMarker _PRF_ResolveRepositoryDependencies =
            new ProfilerMarker(_PRF_PFX + nameof(ResolveRepositoryDependencies));

        private static readonly ProfilerMarker _PRF_CheckIfCyclical =
            new ProfilerMarker(_PRF_PFX + nameof(CheckIfCyclical));

        private static readonly ProfilerMarker
            _PRF_LogGraph = new ProfilerMarker(_PRF_PFX + nameof(LogGraph));

        private static readonly ProfilerMarker _PRF_AddEdge = new ProfilerMarker(_PRF_PFX + nameof(AddEdge));

        private static readonly ProfilerMarker _PRF_CloseRegistration =
            new ProfilerMarker(_PRF_PFX + nameof(CloseRegistration));

        private static readonly ProfilerMarker _PRF_AppalachiaDependencyManager =
            new ProfilerMarker(_PRF_PFX + nameof(AppalachiaRepositoryDependencyManager));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_AddTracker =
            new ProfilerMarker(_PRF_PFX + nameof(AddTracker));

        private static readonly ProfilerMarker _PRF_OnBehaviourDependencyRegistered =
            new ProfilerMarker(_PRF_PFX + nameof(OnBehaviourDependencyRegistered));

        private static readonly ProfilerMarker _PRF_OnObjectDependencyRegistered =
            new ProfilerMarker(_PRF_PFX + nameof(OnObjectDependencyRegistered));

        private static readonly ProfilerMarker _PRF_CalculateGraph =
            new ProfilerMarker(_PRF_PFX + nameof(RecalculateGraph));

        #endregion
    }
}
