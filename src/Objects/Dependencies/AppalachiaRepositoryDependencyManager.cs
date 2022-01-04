using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Algorithms.Graphs;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.DataStructures.Graphs;
using Appalachia.Utility.Logging;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

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

        [NonSerialized] private static bool _dependenciesResolved;

        [NonSerialized] private static bool _dependenciesResolving;

        [NonSerialized] private static Dictionary<Type, AppalachiaRepositoryDependencyTracker> _trackerLookup;

        [NonSerialized]
        private static DirectedSparseGraph<AppalachiaRepositoryDependencyTracker> _trackerGraph;

        [NonSerialized] private static List<AppalachiaRepositoryDependencyTracker> _trackers;

        #endregion

        public static bool DependenciesResolved => _dependenciesResolved;

        public static bool DependenciesResolving => _dependenciesResolving;

        public static bool IsCyclical => CheckIfCyclical();
        private static AppaLogContext LogContext => AppaLog.Context.Dependencies;

        public static void AddTracker(AppalachiaRepositoryDependencyTracker tracker)
        {
            using (_PRF_AddTracker.Auto())
            {
                _dependenciesResolved = false;

                try
                {
                    Initialize();

                    _trackers.Add(tracker);

                    if (!_trackerLookup.ContainsKey(tracker.Owner))
                    {
                        _trackerLookup.Add(tracker.Owner, tracker);
                    }

                    tracker.objectDependencies.DependencyRegistered += OnObjectDependencyRegistered;
                    tracker.behaviourDependencies.DependencyRegistered += OnBehaviourDependencyRegistered;

                    LogContext.Debug(ZString.Format("Now tracking {0}.", tracker.Owner.FormatForLogging()));
                }
                catch (Exception ex)
                {
                    LogContext.Error(
                        ZString.Format("Failed to track {0}.", tracker.Owner.FormatForLogging()),
                        null,
                        ex
                    );
                }
            }
        }

        public static void LogGraph()
        {
            using (_PRF_LogGraph.Auto())
            {
                var msg = _trackerGraph.ToReadable();
                LogContext.Debug(msg);
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

        [SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
        public static async AppaTask ValidateDependencies(

            /*[CallerFilePath] string callerFilePath = null,
            [CallerMemberName] string callerMemberName = null,
            [CallerLineNumber] int callerLineNumber = 0*/)
        {
            if (_dependenciesResolved)
            {
                return;
            }

            if (_dependenciesResolving)
            {
                await AppaTask.WaitUntil(() => DependenciesResolved);
            }
            else
            {
                await ResolveDependencies();
            }
        }

        private static void AddEdge(
            AppalachiaRepositoryDependencyTracker dependent,
            AppalachiaRepositoryDependencyTracker dependency)
        {
            using (_PRF_AddEdge.Auto())
            {
                var trackedType = dependent.Owner;

                LogContext.Debug(
                    ZString.Format(
                        "Tracked type {0} has registered a new {1} dependency on {2}.",
                        trackedType.FormatForLogging(),
                        dependency.ownerType.FormatForLogging(),
                        dependency.Owner.FormatForLogging()
                    )
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

        /*private static void CloseRegistration(
            [CallerFilePath] string callerFilePath = null,
            [CallerMemberName] string callerMemberName = null,
            [CallerLineNumber] int callerLineNumber = 0)
        {
            using (_PRF_CloseRegistration.Auto())
            {
                LogContext.Warn(
                    ZString.Format(
                        "Dependency registration is now closed!  Closed by {0}, {1}:{2}.",
                        callerFilePath.FormatNameForLogging(),
                        callerMemberName.FormatMethodForLogging(),
                        callerLineNumber.FormatForLogging()
                    )
                );
                _registrationClosed = true;
            }
        }*/

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

        private static async Task InitializeBehaviourDependencies(
            AppalachiaRepositoryDependencyTracker[] dependencyTrackers)
        {
            for (var index = dependencyTrackers.Length - 2; index >= 0; index--)
            {
                var tracker = dependencyTrackers[index];
                var dependencies = tracker.behaviourDependencies.Dependencies;
                var dependencyCount = dependencies.Count;

                for (var dependencyIndex = 0; dependencyIndex < dependencyCount; dependencyIndex++)
                {
                    var dep = dependencies[dependencyIndex];

                    var instance = Object.FindObjectOfType(dep.Owner);

                    if (instance == null)
                    {
                        var go = new GameObject(dep.Owner.Name);
                        instance = go.AddComponent(dep.Owner);
                    }

                    dep.instance = instance as ISingleton;

                    if (instance is ISingletonBehaviour sb)
                    {
                        sb.EnsureInstanceIsPrepared(sb);
                    }

                    if (instance != null)
                    {
                        LogContext.Debug(
                            ZString.Format(
                                "Successfully prepared {0} {1} instance.",
                                dep.ownerType,
                                dep.Owner
                            ),
                            instance
                        );
                    }
                    else
                    {
                        LogContext.Error(
                            ZString.Format("Could not prepare {0} {1}", dep.ownerType, dep.Owner),
                            dep.instance
                        );
                    }
                }
            }

            for (var trackerIndex = dependencyTrackers.Length - 2; trackerIndex >= 0; trackerIndex--)
            {
                var tracker = dependencyTrackers[trackerIndex];
                var dependencies = tracker.behaviourDependencies.Dependencies;
                var dependencyCount = dependencies.Count;

                for (var dependencyIndex = 0; dependencyIndex < dependencyCount; dependencyIndex++)
                {
                    var dep = dependencies[dependencyIndex];

                    if (dep.instance is IInitializable i)
                    {
                        await i.ExecuteInitialization();
                    }

                    if (dep.instance != null)
                    {
                        LogContext.Debug(
                            ZString.Format("Successfully resolved {0} {1}", dep.ownerType, dep.Owner),
                            dep.instance
                        );
                    }
                    else
                    {
                        LogContext.Error(
                            ZString.Format("Could not resolve {0} {1}", dep.ownerType, dep.Owner),
                            dep.instance
                        );
                    }
                }
            }
        }

        private static async Task InitializeObjectDependencies(
            AppalachiaRepositoryDependencyTracker[] dependencyTrackers,
            AppalachiaRepository repository)
        {
            for (var index = dependencyTrackers.Length - 2; index >= 0; index--)
            {
                var tracker = dependencyTrackers[index];
                var dependencies = tracker.objectDependencies.Dependencies;
                var dependencyCount = dependencies.Count;

                for (var dependencyIndex = 0; dependencyIndex < dependencyCount; dependencyIndex++)
                {
                    var dep = dependencies[dependencyIndex];

                    var instance = await repository.Find(dep.Owner);

                    if (instance != null)
                    {
                        LogContext.Debug(
                            ZString.Format("Successfully resolved {0} {1}", dep.ownerType, dep.Owner),
                            instance
                        );
                    }
                    else
                    {
                        LogContext.Error(
                            ZString.Format("Could not resolve {0} {1}", dep.ownerType, dep.Owner),
                            repository
                        );
                    }
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

        private static async AppaTask ResolveDependencies()
        {
            using (_PRF_ResolveDependencies.Auto())
            {
                _dependenciesResolved = false;
                _dependenciesResolving = true;

                RecalculateGraph();

                var dependencyTrackers = TopologicalSorter.Sort(_trackerGraph).ToArray();

                var repository = await AppalachiaRepository.AwakeRepository();

                await InitializeObjectDependencies(dependencyTrackers, repository);

                await InitializeBehaviourDependencies(dependencyTrackers);

                _dependenciesResolving = false;
                _dependenciesResolved = true;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AppalachiaRepositoryDependencyManager) + ".";

        private static readonly ProfilerMarker _PRF_ResolveDependencies =
            new ProfilerMarker(_PRF_PFX + nameof(ResolveDependencies));

        private static readonly ProfilerMarker _PRF_CheckIfCyclical =
            new ProfilerMarker(_PRF_PFX + nameof(CheckIfCyclical));

        private static readonly ProfilerMarker
            _PRF_LogGraph = new ProfilerMarker(_PRF_PFX + nameof(LogGraph));

        private static readonly ProfilerMarker _PRF_AddEdge = new ProfilerMarker(_PRF_PFX + nameof(AddEdge));

        private static readonly ProfilerMarker _PRF_AppalachiaDependencyManager =
            new ProfilerMarker(_PRF_PFX + nameof(AppalachiaRepositoryDependencyManager));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_AddTracker =
            new ProfilerMarker(_PRF_PFX + nameof(AddTracker));

        /*private static readonly ProfilerMarker _PRF_CloseRegistration =
            new ProfilerMarker(_PRF_PFX + nameof(CloseRegistration));*/

        private static readonly ProfilerMarker _PRF_OnBehaviourDependencyRegistered =
            new ProfilerMarker(_PRF_PFX + nameof(OnBehaviourDependencyRegistered));

        private static readonly ProfilerMarker _PRF_OnObjectDependencyRegistered =
            new ProfilerMarker(_PRF_PFX + nameof(OnObjectDependencyRegistered));

        private static readonly ProfilerMarker _PRF_CalculateGraph =
            new ProfilerMarker(_PRF_PFX + nameof(RecalculateGraph));

        #endregion
    }
}
