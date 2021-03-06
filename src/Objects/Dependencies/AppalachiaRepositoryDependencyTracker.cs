using System;
using System.Collections.Generic;
using System.Reflection;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Reflection.Extensions;
using Appalachia.Utility.Strings;
using Unity.Profiling;

namespace Appalachia.Core.Objects.Dependencies
{
    public sealed class
        AppalachiaRepositoryDependencyTracker : IComparable<AppalachiaRepositoryDependencyTracker>,
                                                IComparable
    {
        public delegate void DependenciesReadyHandler();

        public event DependenciesReadyHandler DependenciesReady;

        #region Constants and Static Readonly

        private const BindingFlags DEPENDENCY_TRACKER_FLAGS =
            AppalachiaRootConstants.DEPENDENCY_TRACKER_FLAGS;

        private const string DEPENDENCY_TRACKER_NAME = AppalachiaRootConstants.DEPENDENCY_TRACKER_NAME;

        #endregion

        internal AppalachiaRepositoryDependencyTracker(Type t)
        {
            Owner = t;
            if (t == typeof(AppalachiaRepository))
            {
                ownerType = DependencyType.Repository;
            }
            else if (t.InheritsFrom(typeof(AppalachiaObject)))
            {
                ownerType = DependencyType.Object;
            }
            else // if (t.InheritsFrom(typeof(AppalachiaBehaviour)))
            {
                ownerType = DependencyType.Behaviour;
            }

            _repositoryDependencies = new DependencyTrackingSubset(this);
            _objectDependencies = new DependencyTrackingSubset(this);
            _behaviourDependencies = new DependencyTrackingSubset(this);

            _subsets = new[] { _repositoryDependencies, _objectDependencies, _behaviourDependencies, };

            AppalachiaRepositoryDependencyManager.AddTracker(this);
        }

        #region Fields and Autoproperties

        public readonly DependencyType ownerType;

        public ISingleton instance;

        public Type Owner { get; }

        private readonly DependencyTrackingSubset[] _subsets;

        [NonSerialized] private bool _dependenciesAreReady;

        private bool _isReady;
        private DependencyTrackingSubset _behaviourDependencies;
        private DependencyTrackingSubset _objectDependencies;
        private DependencyTrackingSubset _repositoryDependencies;

        #endregion

        public bool IsReady => _isReady && DependenciesAreReady;

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

        public IEnumerable<AppalachiaRepositoryDependencyTracker> AllDependencies
        {
            get
            {
                foreach (var dependency in repositoryDependencies.Dependencies)
                {
                    yield return dependency;
                }

                foreach (var dependency in objectDependencies.Dependencies)
                {
                    yield return dependency;
                }

                foreach (var dependency in behaviourDependencies.Dependencies)
                {
                    yield return dependency;
                }
            }
        }

        internal bool DependenciesAreReady
        {
            get
            {
                using (_PRF_DependenciesAreReady.Auto())
                {
                    if (_dependenciesAreReady)
                    {
                        return true;
                    }

                    for (var index = 0; index < subsets.Length; index++)
                    {
                        var d = subsets[index];

                        if (!d.IsReady)
                        {
                            return false;
                        }
                    }

                    _dependenciesAreReady = true;
                    return true;
                }
            }
        }

        private DependencyTrackingSubset[] subsets => _subsets;

        private string typeName => Owner.FullName;

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

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{ownerType}: {Owner.Name}";
        }

        public void InvokeDependenciesReady()
        {
            using (_PRF_InvokeDependenciesReady.Auto())
            {
                DependenciesReady?.Invoke();
            }
        }

        public void MarkReady()
        {
            using (_PRF_MarkReady.Auto())
            {
                _isReady = true;
            }
        }

        public void RegisterDependency(
            AppalachiaRepositoryDependencyTracker dependentOn,
            AppalachiaRepository.InstanceAvailableHandler handler)
        {
            using (_PRF_RegisterDependency.Auto())
            {
                _isReady = false;

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
                _isReady = false;

                SingletonAppalachiaBehaviour<TDependency>.InstanceAvailable += handler;

                behaviourDependencies.RegisterDependency(dependentOn);
            }
        }

        public void RegisterDependency(AppalachiaRepositoryDependencyTracker dependentOn)
        {
            using (_PRF_RegisterDependency.Auto())
            {
                _isReady = false;

                behaviourDependencies.RegisterDependency(dependentOn);
            }
        }

        public void RegisterDependency<TDependency>(
            SingletonAppalachiaObject<TDependency>.InstanceAvailableHandler handler)
            where TDependency : SingletonAppalachiaObject<TDependency>,
            IRepositoryDependencyTracker<TDependency>
        {
            using (_PRF_RegisterDependency.Auto())
            {
                _isReady = false;

                var property = typeof(AppalachiaObject<TDependency>).GetField_CACHE(
                    DEPENDENCY_TRACKER_NAME,
                    DEPENDENCY_TRACKER_FLAGS
                );

                var value = property.GetValue(null);
                var cast = value as AppalachiaRepositoryDependencyTracker;

                RegisterDependency(cast, handler);
            }
        }

        public void RegisterDependency<TDependency>(
            SingletonAppalachiaBehaviour<TDependency>.InstanceAvailableHandler handler)
            where TDependency : SingletonAppalachiaBehaviour<TDependency>,
            IRepositoryDependencyTracker<TDependency>
        {
            using (_PRF_RegisterDependency.Auto())
            {
                _isReady = false;

                var property = typeof(AppalachiaBehaviour<TDependency>).GetField_CACHE(
                    DEPENDENCY_TRACKER_NAME,
                    DEPENDENCY_TRACKER_FLAGS
                );

                var value = property.GetValue(null);
                var cast = value as AppalachiaRepositoryDependencyTracker;

                RegisterDependency(cast, handler);
            }
        }

        public void RegisterDependency(Type type)
        {
            using (_PRF_RegisterDependency.Auto())
            {
                _isReady = false;

                var behaviourUnrealized = typeof(AppalachiaBehaviour<>);
                var objectUnrealized = typeof(AppalachiaObject<>);

                Type realizedGeneric;

                var errorMessage = ZString.Format(
                    "You may not depend on the type {0}",
                    type.FormatForLogging()
                );
                
                if (type.InheritsFrom(behaviourUnrealized))
                {
                    realizedGeneric = behaviourUnrealized.MakeGenericType(type);
                }
                else if (type.InheritsFrom(objectUnrealized))
                {
                    realizedGeneric = objectUnrealized.MakeGenericType(type);
                }
                else
                {
                    throw new NotSupportedException(errorMessage);
                }

                var property = realizedGeneric.GetField_CACHE(DEPENDENCY_TRACKER_NAME, DEPENDENCY_TRACKER_FLAGS);

                if (property == null)
                {
                    throw new NotSupportedException(errorMessage);
                }

                var value = property.GetValue(null);
                var cast = value as AppalachiaRepositoryDependencyTracker;

                RegisterDependency(cast);
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

        internal void ResetFully()
        {
            using (_PRF_ResetFully.Auto())
            {
                _dependenciesAreReady = false;
                _isReady = false;

                for (var index = 0; index < subsets.Length; index++)
                {
                    var d = subsets[index];

                    d.ResetFully();
                }
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

        private static readonly ProfilerMarker _PRF_ResetFully =
            new ProfilerMarker(_PRF_PFX + nameof(ResetFully));

        private static readonly ProfilerMarker _PRF_MarkReady =
            new ProfilerMarker(_PRF_PFX + nameof(MarkReady));

        private static readonly ProfilerMarker _PRF_InvokeDependenciesReady =
            new ProfilerMarker(_PRF_PFX + nameof(InvokeDependenciesReady));

        private static readonly ProfilerMarker _PRF_DependenciesAreReady =
            new ProfilerMarker(_PRF_PFX + nameof(DependenciesAreReady));

        private static readonly ProfilerMarker _PRF_RegisterDependency =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterDependency));

        #endregion
    }
}
