using System.Collections.Generic;
using Unity.Profiling;

namespace Appalachia.Core.Objects.Dependencies
{
    public class DependencyTrackingSubset
    {
        public delegate void DependencyRegisteredHandler(
            AppalachiaRepositoryDependencyTracker dependent,
            AppalachiaRepositoryDependencyTracker dependency);

        public DependencyTrackingSubset(AppalachiaRepositoryDependencyTracker tracking)
        {
            _tracking = tracking;
            _dependencies = new List<AppalachiaRepositoryDependencyTracker>();
        }

        #region Fields and Autoproperties

        private readonly AppalachiaRepositoryDependencyTracker _tracking;
        private readonly List<AppalachiaRepositoryDependencyTracker> _dependencies;

        private bool _dependenciesAreReady;

        #endregion

        public bool DependenciesAreReady
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
                        if (!dependency.DependenciesAreReady)
                        {
                            return false;
                        }
                    }

                    _dependenciesAreReady = true;

                    return true;
                }
            }
        }

        public IReadOnlyList<AppalachiaRepositoryDependencyTracker> Dependencies => _dependencies;

        public event DependencyRegisteredHandler DependencyRegistered;

        internal void RegisterDependency(AppalachiaRepositoryDependencyTracker dependency)
        {
            using (_PRF_RegisterDependency.Auto())
            {
                _dependenciesAreReady = false;

                _dependencies.Add(dependency);
                DependencyRegistered?.Invoke(_tracking, dependency);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(DependencyTrackingSubset) + ".";

        private static readonly ProfilerMarker _PRF_RegisterDependency =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterDependency));

        private static readonly ProfilerMarker _PRF_DependenciesAreReady =
            new ProfilerMarker(_PRF_PFX + nameof(DependenciesAreReady));

        #endregion
    }
}
