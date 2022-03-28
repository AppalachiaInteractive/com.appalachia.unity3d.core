using Appalachia.Core.Objects.Dependencies;

namespace Appalachia.Core.Objects.Root.Contracts
{
    public interface IRepositoryDependencyTracker<T>
        where T : IRepositoryDependencyTracker<T>
    {
        AppalachiaRepositoryDependencyTracker DependencyTracker { get; }
        bool DependenciesAreReady { get; }
    }
}
