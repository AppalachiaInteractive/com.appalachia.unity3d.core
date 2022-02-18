using System;
using System.Collections.Generic;
using System.Linq;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Algorithms.Graphs;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.DataStructures.Graphs;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Logging;
using Appalachia.Utility.Reflection.Extensions;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Objects.Dependencies
{
    [CallStaticConstructorInEditor]
    public static class AppalachiaRepositoryDependencyManager
    {
        public delegate void DependenciesResolvedHandler();

        public static event DependenciesResolvedHandler DependenciesResolved;

        private enum Status
        {
            Unresolved = 0,
            Resolving = 1,
            Resolved = 2,
        }

        static AppalachiaRepositoryDependencyManager()
        {
            using (_PRF_AppalachiaDependencyManager.Auto())
            {
                InitializeCollections();
            }
        }

        #region Static Fields and Autoproperties

        [NonSerialized] private static Dictionary<Type, AppalachiaRepositoryDependencyTracker> _trackerLookup;

        [NonSerialized] private static Dictionary<Type, Status> _resolutionStatus;

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
                _resolutionStatus.AddOrUpdate(tracker.Owner, Status.Unresolved);

                try
                {
                    _trackers.Add(tracker);

                    if (!_trackerLookup.ContainsKey(tracker.Owner))
                    {
                        _trackerLookup.Add(tracker.Owner, tracker);
                    }

                    tracker.objectDependencies.DependencyRegistered += OnObjectDependencyRegistered;
                    tracker.behaviourDependencies.DependencyRegistered += OnBehaviourDependencyRegistered;

                    /*LogContext.Trace(ZString.Format("Now tracking {0}.", tracker.Owner.FormatForLogging()));*/
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

        public static async AppaTask ValidateDependencies(Type owner)
        {
            var scene = SceneManager.GetActiveScene();

            await AppaTask.WaitUntil(() => scene.isLoaded);

            if (!_trackerLookup.ContainsKey(owner))
            {
                return;
            }

            var tracker = _trackerLookup[owner];

            if (_resolutionStatus.ContainsKey(tracker.Owner))
            {
                if (_resolutionStatus[tracker.Owner] == Status.Resolved)
                {
                    return;
                }

                if (_resolutionStatus[tracker.Owner] == Status.Resolving)
                {
                    await AppaTask.WaitUntil(() => _resolutionStatus[tracker.Owner] == Status.Resolved);
                }
            }

            await ResolveDependencies(tracker);
        }

        private static void AddEdge(
            AppalachiaRepositoryDependencyTracker dependent,
            AppalachiaRepositoryDependencyTracker dependency)
        {
            using (_PRF_AddEdge.Auto())
            {
                var trackedType = dependent.Owner;

                /*LogContext.Trace(
                    ZString.Format(
                        "Tracked type {0} has registered a new {1} dependency on {2}.",
                        trackedType.FormatForLogging(),
                        dependency.ownerType.FormatEnumForLogging(),
                        dependency.Owner.FormatForLogging()
                    )
                );*/

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

        private static Object FindExistingBehaviourInstance(
            Object instance,
            AppalachiaRepositoryDependencyTracker dependency)
        {
            using (_PRF_FindExistingBehaviourInstance.Auto())
            {
                for (var i = 0; i < SceneManager.sceneCount; i++)
                {
                    var scene = SceneManager.GetSceneAt(i);
                    var roots = scene.GetRootGameObjects();

                    for (var rootObjectIndex = 0; rootObjectIndex < roots.Length; rootObjectIndex++)
                    {
                        var root = roots[rootObjectIndex];

                        instance = root.GetComponent(dependency.Owner);

                        if (instance != null)
                        {
                            break;
                        }

                        instance = root.GetComponentInChildren(dependency.Owner);

                        if (instance != null)
                        {
                            break;
                        }
                    }

                    if (instance != null)
                    {
                        break;
                    }
                }

                if (instance == null)
                {
                    instance = Resources.FindObjectsOfTypeAll(dependency.Owner)
                                        .Cast<MonoBehaviour>()
                                        .FirstOrDefault(o => (o != null) && o.isActiveAndEnabled);
                }

                if (instance == null)
                {
                    var go = new GameObject(dependency.Owner.Name);
                    instance = go.AddComponent(dependency.Owner);
                }

                return instance;
            }
        }

        private static void InitializeCollections()
        {
            using (_PRF_Initialize.Auto())
            {
                _trackers = new List<AppalachiaRepositoryDependencyTracker>();
                _trackerLookup = new Dictionary<Type, AppalachiaRepositoryDependencyTracker>();
                _resolutionStatus = new Dictionary<Type, Status>();

                if (_trackerGraph == null)
                {
                    _trackerGraph = new DirectedSparseGraph<AppalachiaRepositoryDependencyTracker>();
                    _trackerGraph.AddVertex(AppalachiaRepository.DependencyTracker);
                }
            }
        }

        private static void LogGraph()
        {
            using (_PRF_LogGraph.Auto())
            {
                var msg = _trackerGraph.ToReadable();
                LogContext.Debug(msg);
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

        private static void RecalculateGraph()
        {
            using (_PRF_CalculateGraph.Auto())
            {
                //_trackerGraph.Clear();

                if (!_trackerGraph.HasVertex(AppalachiaRepository.DependencyTracker))
                {
                    _trackerGraph.AddVertex(AppalachiaRepository.DependencyTracker);
                }

                foreach (var tracker in _trackers)
                {
                    foreach (var dependency in tracker.repositoryDependencies.Dependencies)
                    {
                        if (!_trackerGraph.HasEdge(tracker, dependency))
                        {
                            AddEdge(tracker, dependency);
                        }
                    }

                    foreach (var dependency in tracker.objectDependencies.Dependencies)
                    {
                        if (!_trackerGraph.HasEdge(tracker, dependency))
                        {
                            AddEdge(tracker, dependency);
                        }
                    }

                    foreach (var dependency in tracker.behaviourDependencies.Dependencies)
                    {
                        if (!_trackerGraph.HasEdge(tracker, dependency))
                        {
                            AddEdge(tracker, dependency);
                        }
                    }
                }

                CheckIfCyclical();
            }
        }

        private static async AppaTask ResolveDependencies(
            AppalachiaRepository repository,
            AppalachiaRepositoryDependencyTracker tracker)
        {
            if (tracker.IsReady)
            {
                return;
            }

            for (var dependencyIndex = 0;
                 dependencyIndex < tracker.objectDependencies.Dependencies.Count;
                 dependencyIndex++)
            {
                var dependency = tracker.objectDependencies.Dependencies[dependencyIndex];

                await ResolveDependencies(repository, dependency);
            }

            for (var dependencyIndex = 0;
                 dependencyIndex < tracker.behaviourDependencies.Dependencies.Count;
                 dependencyIndex++)
            {
                var dependency = tracker.behaviourDependencies.Dependencies[dependencyIndex];

                await ResolveDependencies(repository, dependency);
            }

            if (tracker.IsReady)
            {
                return;
            }

            var singletonBehaviour = tracker.Owner.InheritsFrom(typeof(SingletonAppalachiaBehaviour<>));
            var singletonObject = tracker.Owner.InheritsFrom(typeof(SingletonAppalachiaObject<>));

            var singleton = singletonBehaviour || singletonObject;

            if (!singleton)
            {
                /*LogContext.Trace(
                    ZString.Format(
                        "Successfully prepared static dependencies for {0} {1}.",
                        tracker.ownerType,
                        tracker.Owner
                    )
                );*/

                tracker.MarkReady();
                return;
            }

            if (singletonBehaviour)
            {
                var instance = Object.FindObjectOfType(tracker.Owner, true);

                if (instance == null)
                {
                    instance = FindExistingBehaviourInstance(instance, tracker);
                }

                tracker.instance = instance as ISingleton;
            }
            else if (singletonObject)
            {
                tracker.instance = await repository.Find(tracker.Owner);
            }

            if (tracker.instance == null)
            {
                LogContext.Error(
                    ZString.Format("Could not prepare Singleton {0} {1}", tracker.ownerType, tracker.Owner)
                );

                return;
            }

            /*LogContext.Trace(
                ZString.Format(
                    "Successfully resolved {0} {1} instance.  Now preparing.",
                    tracker.ownerType,
                    tracker.Owner
                ),
                tracker.instance
            );*/

            if (tracker.instance is ISingletonBehaviour sb)
            {
                tracker.instance = sb;

                sb.EnsureInstanceIsPrepared(sb);
            }

            tracker.MarkReady();

            tracker.instance.SetSingletonInstance(tracker.instance);

            tracker.InvokeDependenciesReady();

            /*LogContext.Trace(
                ZString.Format("Successfully prepared {0} {1} instance.", tracker.ownerType, tracker.Owner),
                tracker.instance
            );*/
        }

        private static async AppaTask ResolveDependencies(AppalachiaRepositoryDependencyTracker tracker)
        {
            _resolutionStatus.AddOrUpdate(tracker.Owner, Status.Resolving);

            RecalculateGraph();

            var repository = await AppalachiaRepository.AwakeRepository();

            await ResolveDependencies(repository, tracker);

            _resolutionStatus[tracker.Owner] = Status.Resolved;

            DependenciesResolved?.Invoke();
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AppalachiaRepositoryDependencyManager) + ".";

        private static readonly ProfilerMarker _PRF_FindExistingBehaviourInstance =
            new ProfilerMarker(_PRF_PFX + nameof(FindExistingBehaviourInstance));

        private static readonly ProfilerMarker _PRF_CheckIfCyclical =
            new ProfilerMarker(_PRF_PFX + nameof(CheckIfCyclical));

        private static readonly ProfilerMarker
            _PRF_LogGraph = new ProfilerMarker(_PRF_PFX + nameof(LogGraph));

        private static readonly ProfilerMarker _PRF_AddEdge = new ProfilerMarker(_PRF_PFX + nameof(AddEdge));

        private static readonly ProfilerMarker _PRF_AppalachiaDependencyManager =
            new ProfilerMarker(_PRF_PFX + nameof(AppalachiaRepositoryDependencyManager));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeCollections));

        private static readonly ProfilerMarker _PRF_AddTracker =
            new ProfilerMarker(_PRF_PFX + nameof(AddTracker));

        private static readonly ProfilerMarker _PRF_OnBehaviourDependencyRegistered =
            new ProfilerMarker(_PRF_PFX + nameof(OnBehaviourDependencyRegistered));

        private static readonly ProfilerMarker _PRF_OnObjectDependencyRegistered =
            new ProfilerMarker(_PRF_PFX + nameof(OnObjectDependencyRegistered));

        private static readonly ProfilerMarker _PRF_CalculateGraph =
            new ProfilerMarker(_PRF_PFX + nameof(RecalculateGraph));

        #endregion

        /*private static readonly ProfilerMarker _PRF_CloseRegistration =
            new ProfilerMarker(_PRF_PFX + nameof(CloseRegistration));*/
    }
}
