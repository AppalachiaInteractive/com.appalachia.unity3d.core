using System;
using System.Collections.Generic;
using Appalachia.Core.Objects.Root;
using Unity.Profiling;

namespace Appalachia.Core.Objects.Dependencies
{
    public sealed class
        AppalachiaRepositoryDependencyTracker : IComparable<AppalachiaRepositoryDependencyTracker>,
                                                IComparable
    {
        public delegate void DependenciesReadyHandler();

        internal AppalachiaRepositoryDependencyTracker(Type t, DependencyType dt)
        {
            Tracking = t;
            dependencyType = dt;

            _repositoryDependencies = new DependencyTrackingSubset(this);
            _objectDependencies = new DependencyTrackingSubset(this);
            _behaviourDependencies = new DependencyTrackingSubset(this);

            _subsets = new[] { _repositoryDependencies, _objectDependencies, _behaviourDependencies, };

            AppalachiaRepositoryDependencyManager.AddTracker(this);
        }

        #region Fields and Autoproperties

        public readonly DependencyType dependencyType;

        public Type Tracking { get; }

        private readonly DependencyTrackingSubset[] _subsets;
        private DependencyTrackingSubset _behaviourDependencies;
        private DependencyTrackingSubset _objectDependencies;
        private DependencyTrackingSubset _repositoryDependencies;

        #endregion

        public bool DependenciesAreReady
        {
            get
            {
                using (_PRF_DependenciesAreReady.Auto())
                {
                    for (var index = 0; index < subsets.Length; index++)
                    {
                        var d = subsets[index];
                        if (!d.DependenciesAreReady)
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }
        }

        public DependencyTrackingSubset behaviourDependencies
        {
            get
            {
                _behaviourDependencies ??= new DependencyTrackingSubset(this);

                return _behaviourDependencies;
            }
        }

        public DependencyTrackingSubset objectDependencies
        {
            get
            {
                _objectDependencies ??= new DependencyTrackingSubset(this);

                return _objectDependencies;
            }
        }

        public DependencyTrackingSubset repositoryDependencies
        {
            get
            {
                _repositoryDependencies ??= new DependencyTrackingSubset(this);

                return _repositoryDependencies;
            }
        }

        private DependencyTrackingSubset[] subsets => _subsets;

        private string typeName => Tracking.FullName;

        public event DependenciesReadyHandler DependenciesReady;

        public static bool operator >(
            AppalachiaRepositoryDependencyTracker left,
            AppalachiaRepositoryDependencyTracker right)
        {
            return Comparer<AppalachiaRepositoryDependencyTracker>.Default.Compare(left, right) > 0;
        }

        public static bool operator >=(
            AppalachiaRepositoryDependencyTracker left,
            AppalachiaRepositoryDependencyTracker right)
        {
            return Comparer<AppalachiaRepositoryDependencyTracker>.Default.Compare(left, right) >= 0;
        }

        public static bool operator <(
            AppalachiaRepositoryDependencyTracker left,
            AppalachiaRepositoryDependencyTracker right)
        {
            return Comparer<AppalachiaRepositoryDependencyTracker>.Default.Compare(left, right) < 0;
        }

        public static bool operator <=(
            AppalachiaRepositoryDependencyTracker left,
            AppalachiaRepositoryDependencyTracker right)
        {
            return Comparer<AppalachiaRepositoryDependencyTracker>.Default.Compare(left, right) <= 0;
        }

        public void InvokeDependenciesReady()
        {
            using (_PRF_InvokeDependenciesReady.Auto())
            {
                DependenciesReady?.Invoke();
            }
        }

        public void RegisterDependency(
            AppalachiaRepositoryDependencyTracker dependentOn,
            AppalachiaRepository.InstanceAvailableHandler handler)
        {
            using (_PRF_RegisterDependency.Auto())
            {
                AppalachiaRepository.InstanceAvailable += handler;

                repositoryDependencies.RegisterDependency(dependentOn);
            }
        }

        public void RegisterDependency<TDependency>(
            AppalachiaRepositoryDependencyTracker dependentOn,
            SingletonAppalachiaBehaviour<TDependency>.InstanceAvailableHandler handler)
            where TDependency : SingletonAppalachiaBehaviour<TDependency>
        {
            using (_PRF_RegisterDependency.Auto())
            {
                SingletonAppalachiaBehaviour<TDependency>.InstanceAvailable += handler;

                behaviourDependencies.RegisterDependency(dependentOn);
            }
        }

        public void RegisterDependency<TDependency>(
            AppalachiaRepositoryDependencyTracker dependentOn,
            SingletonAppalachiaObject<TDependency>.InstanceAvailableHandler handler)
            where TDependency : SingletonAppalachiaObject<TDependency>
        {
            using (_PRF_RegisterDependency.Auto())
            {
                SingletonAppalachiaObject<TDependency>.InstanceAvailable += handler;

                objectDependencies.RegisterDependency(dependentOn);
            }
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return 1;
            }

            if (ReferenceEquals(this, obj))
            {
                return 0;
            }

            return obj is AppalachiaRepositoryDependencyTracker other
                ? CompareTo(other)
                : throw new ArgumentException(
                    $"Object must be of type {nameof(AppalachiaRepositoryDependencyTracker)}"
                );
        }

        #endregion

        #region IComparable<AppalachiaRepositoryDependencyTracker> Members

        public int CompareTo(AppalachiaRepositoryDependencyTracker other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }

            return string.Compare(typeName, other.typeName, StringComparison.Ordinal);
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(AppalachiaRepositoryDependencyManager) + ".";

        private static readonly ProfilerMarker _PRF_InvokeDependenciesReady =
            new ProfilerMarker(_PRF_PFX + nameof(InvokeDependenciesReady));

        private static readonly ProfilerMarker _PRF_DependenciesAreReady =
            new ProfilerMarker(_PRF_PFX + nameof(DependenciesAreReady));

        private static readonly ProfilerMarker _PRF_RegisterDependency =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterDependency));

        #endregion
    }
}
