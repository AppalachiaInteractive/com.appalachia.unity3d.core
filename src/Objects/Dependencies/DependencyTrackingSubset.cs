using System;
using System.Collections.Generic;
using Unity.Profiling;

namespace Appalachia.Core.Objects.Dependencies
{
    public class DependencyTrackingSubset
    {
        public delegate void DependencyRegisteredHandler(
            AppalachiaRepositoryDependencyTracker dependent,
            AppalachiaRepositoryDependencyTracker dependency);

        public event DependencyRegisteredHandler DependencyRegistered;

        public DependencyTrackingSubset(AppalachiaRepositoryDependencyTracker tracking)
        {
            _tracking = tracking;
            _dependencies = new List<AppalachiaRepositoryDependencyTracker>();
        }

        #region Fields and Autoproperties

        private readonly AppalachiaRepositoryDependencyTracker _tracking;
        private readonly List<AppalachiaRepositoryDependencyTracker> _dependencies;

        [NonSerialized] private bool _dependenciesAreReady;

        #endregion

        public bool IsReady => DependenciesAreReady;

        public IReadOnlyList<AppalachiaRepositoryDependencyTracker> Dependencies => _dependencies;

        private bool DependenciesAreReady
        {
            get
            {
                using (_PRF_DependenciesAreReady.Auto())
                {
                    if (_dependenciesAreReady)
                    {
                        return true;
                    }

                    foreach (var dependency in _dependencies)
                    {
                        if (!dependency.IsReady)
                        {
                            return false;
                        }
                    }

                    _dependenciesAreReady = true;
                    return true;
                }
            }
        }

        internal void RegisterDependency(AppalachiaRepositoryDependencyTracker dependency)
        {
            using (_PRF_RegisterDependency.Auto())
            {
                _dependencies.Add(dependency);
                DependencyRegistered?.Invoke(_tracking, dependency);
            }
        }

        internal void ResetFully()
        {
            using (_PRF_ResetFully.Auto())
            {
                _dependenciesAreReady = false;

                foreach (var dependency in _dependencies)
                {
                    dependency.ResetFully();
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(DependencyTrackingSubset) + ".";

        private static readonly ProfilerMarker _PRF_ResetFully =
            new ProfilerMarker(_PRF_PFX + nameof(ResetFully));

        private static readonly ProfilerMarker _PRF_RegisterDependency =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterDependency));

        private static readonly ProfilerMarker _PRF_DependenciesAreReady =
            new ProfilerMarker(_PRF_PFX + nameof(DependenciesAreReady));

        #endregion
    }
}
