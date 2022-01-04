using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Dependencies;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Strings;
using Unity.Profiling;

// ReSharper disable StaticMemberInGenericType

namespace Appalachia.Core.Objects.Root
{
    [CallStaticConstructorInEditor]
    public partial class AppalachiaRepository : IRepositoryDependencyTracker<AppalachiaRepository>
    {
        static AppalachiaRepository()
        {
            _dependencyTracker = new AppalachiaRepositoryDependencyTracker(typeof(AppalachiaRepository));
        }

        #region Static Fields and Autoproperties

        private static AppalachiaRepositoryDependencyTracker _dependencyTracker;

        #endregion

        public static AppalachiaRepositoryDependencyTracker DependencyTracker => _dependencyTracker;

        public static bool DependenciesAreReady => _dependencyTracker.DependenciesAreReady;

        #region IRepositoryDependencyTracker<AppalachiaRepository> Members

        bool IRepositoryDependencyTracker<AppalachiaRepository>.DependenciesAreReady => DependenciesAreReady;

        AppalachiaRepositoryDependencyTracker IRepositoryDependencyTracker<AppalachiaRepository>.
            DependencyTracker =>
            DependencyTracker;

        #endregion
    }

    public abstract partial class AppalachiaObject
    {
    }

    [CallStaticConstructorInEditor]
    public abstract partial class AppalachiaObject<T> : IRepositoryDependencyTracker<T>
    {
        static AppalachiaObject()
        {
            _dependencyTracker = new AppalachiaRepositoryDependencyTracker(typeof(T));
            _dependencyTracker.RegisterDependency(
                AppalachiaRepository.DependencyTracker,
                i => _appalachiaRepository = i
            );
        }

        #region Static Fields and Autoproperties

        protected internal static AppalachiaRepositoryDependencyTracker _dependencyTracker;

        private static AppalachiaRepository _appalachiaRepository;

        #endregion

        public static AppalachiaRepositoryDependencyTracker DependencyTracker => _dependencyTracker;

        public static bool DependenciesAreReady => _dependencyTracker.DependenciesAreReady;

        protected static AppalachiaRepository AppalachiaRepository => _appalachiaRepository;

        protected static void RegisterDependency<TDependency>(
            SingletonAppalachiaObject<TDependency>.InstanceAvailableHandler handler)
            where TDependency : SingletonAppalachiaObject<TDependency>,
            IRepositoryDependencyTracker<TDependency>
        {
            using (_PRF_RegisterDependency.Auto())
            {
                _dependencyTracker.RegisterDependency(handler);
            }
        }

        #region IRepositoryDependencyTracker<T> Members

        bool IRepositoryDependencyTracker<T>.DependenciesAreReady => DependenciesAreReady;

        AppalachiaRepositoryDependencyTracker IRepositoryDependencyTracker<T>.DependencyTracker =>
            DependencyTracker;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_RegisterDependency =
            new(_PRF_PFX + nameof(RegisterDependency));

        #endregion
    }

    [CallStaticConstructorInEditor]
    public partial class SingletonAppalachiaObject<T>
    {
        static SingletonAppalachiaObject()
        {
            _dependencyTracker.RegisterDependency(
                AppalachiaRepository.DependencyTracker,
                ValidateTypeIsInRepository
            );
        }

        private static void ValidateTypeIsInRepository(AppalachiaRepository repositoryInstance)
        {
            using (_PRF_ValidateTypeIsInRepository.Auto())
            {
                if (!repositoryInstance.IsTypeInRepository<T>())
                {
                    SingletonContext.Error(
                        ZString.Format(
                            "Singleton {0} is not present in the {1}",
                            typeof(T).FormatForLogging(),
                            typeof(AppalachiaRepository).FormatForLogging()
                        )
                    );
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_ValidateTypeIsInRepository =
            new(_PRF_PFX + nameof(ValidateTypeIsInRepository));

        #endregion
    }

    public partial class AppalachiaBehaviour
    {
    }

    [CallStaticConstructorInEditor]
    public abstract partial class AppalachiaBehaviour<T> : IRepositoryDependencyTracker<T>
    {
        static AppalachiaBehaviour()
        {
            _dependencyTracker = new AppalachiaRepositoryDependencyTracker(typeof(T));
            _dependencyTracker.RegisterDependency(
                AppalachiaRepository.DependencyTracker,
                i => _appalachiaRepository = i
            );
        }

        #region Static Fields and Autoproperties

        protected internal static AppalachiaRepositoryDependencyTracker _dependencyTracker;

        private static AppalachiaRepository _appalachiaRepository;

        #endregion

        public static AppalachiaRepositoryDependencyTracker DependencyTracker => _dependencyTracker;

        public static bool DependenciesAreReady => _dependencyTracker.DependenciesAreReady;

        protected static AppalachiaRepository AppalachiaRepository => _appalachiaRepository;

        protected static void RegisterDependency<TDependency>(
            SingletonAppalachiaBehaviour<TDependency>.InstanceAvailableHandler handler)
            where TDependency : SingletonAppalachiaBehaviour<TDependency>,
            IRepositoryDependencyTracker<TDependency>
        {
            using (_PRF_RegisterDependency.Auto())
            {
                _dependencyTracker.RegisterDependency(handler);
            }
        }

        protected static void RegisterDependency<TDependency>(
            SingletonAppalachiaObject<TDependency>.InstanceAvailableHandler handler)
            where TDependency : SingletonAppalachiaObject<TDependency>,
            IRepositoryDependencyTracker<TDependency>
        {
            using (_PRF_RegisterDependency.Auto())
            {
                _dependencyTracker.RegisterDependency(handler);
            }
        }

        #region IRepositoryDependencyTracker<T> Members

        bool IRepositoryDependencyTracker<T>.DependenciesAreReady => DependenciesAreReady;

        AppalachiaRepositoryDependencyTracker IRepositoryDependencyTracker<T>.DependencyTracker =>
            DependencyTracker;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_RegisterDependency =
            new(_PRF_PFX + nameof(RegisterDependency));

        #endregion
    }

    [CallStaticConstructorInEditor]
    public partial class SingletonAppalachiaBehaviour<T>
    {
        static SingletonAppalachiaBehaviour()
        {
        }
    }

    public partial class AppalachiaSimpleBase
    {
    }

    public partial class AppalachiaBase
    {
    }

    [CallStaticConstructorInEditor]
    public partial class AppalachiaBase<T> : IRepositoryDependencyTracker<T>
    {
        static AppalachiaBase()
        {
            _dependencyTracker = new AppalachiaRepositoryDependencyTracker(typeof(T));
            _dependencyTracker.RegisterDependency(
                AppalachiaRepository.DependencyTracker,
                i => _appalachiaRepository = i
            );
        }

        #region Static Fields and Autoproperties

        protected internal static AppalachiaRepositoryDependencyTracker _dependencyTracker;

        private static AppalachiaRepository _appalachiaRepository;

        #endregion

        public static AppalachiaRepositoryDependencyTracker DependencyTracker => _dependencyTracker;

        public static bool DependenciesAreReady => _dependencyTracker.DependenciesAreReady;

        protected static AppalachiaRepository AppalachiaRepository => _appalachiaRepository;

        protected static void RegisterDependency<TDependency>(
            SingletonAppalachiaObject<TDependency>.InstanceAvailableHandler handler)
            where TDependency : SingletonAppalachiaObject<TDependency>,
            IRepositoryDependencyTracker<TDependency>
        {
            using (_PRF_RegisterDependency.Auto())
            {
                _dependencyTracker.RegisterDependency(handler);
            }
        }

        #region IRepositoryDependencyTracker<T> Members

        bool IRepositoryDependencyTracker<T>.DependenciesAreReady => DependenciesAreReady;

        AppalachiaRepositoryDependencyTracker IRepositoryDependencyTracker<T>.DependencyTracker =>
            DependencyTracker;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_RegisterDependency =
            new(_PRF_PFX + nameof(RegisterDependency));

        #endregion
    }

    public partial class AppalachiaSimplePlayable
    {
    }

    public partial class AppalachiaPlayable
    {
    }

    public partial class AppalachiaPlayable<T> : IRepositoryDependencyTracker<T>
    {
        static AppalachiaPlayable()
        {
            _dependencyTracker = new AppalachiaRepositoryDependencyTracker(typeof(T));
            _dependencyTracker.RegisterDependency(
                AppalachiaRepository.DependencyTracker,
                i => _appalachiaRepository = i
            );
        }

        #region Static Fields and Autoproperties

        protected internal static AppalachiaRepositoryDependencyTracker _dependencyTracker;

        private static AppalachiaRepository _appalachiaRepository;

        #endregion

        public static AppalachiaRepositoryDependencyTracker DependencyTracker => _dependencyTracker;

        public static bool DependenciesAreReady => _dependencyTracker.DependenciesAreReady;

        protected static AppalachiaRepository AppalachiaRepository => _appalachiaRepository;

        protected static void RegisterDependency<TDependency>(
            SingletonAppalachiaObject<TDependency>.InstanceAvailableHandler handler)
            where TDependency : SingletonAppalachiaObject<TDependency>,
            IRepositoryDependencyTracker<TDependency>
        {
            using (_PRF_RegisterDependency.Auto())
            {
                _dependencyTracker.RegisterDependency(handler);
            }
        }

        #region IRepositoryDependencyTracker<T> Members

        bool IRepositoryDependencyTracker<T>.DependenciesAreReady => DependenciesAreReady;

        AppalachiaRepositoryDependencyTracker IRepositoryDependencyTracker<T>.DependencyTracker =>
            DependencyTracker;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_RegisterDependency =
            new(_PRF_PFX + nameof(RegisterDependency));

        #endregion
    }
}
