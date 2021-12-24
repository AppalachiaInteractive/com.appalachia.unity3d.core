using Appalachia.Core.Objects.Dependencies;

namespace Appalachia.Core.Objects.Root.Contracts
{
    public interface IRepositoryDependencyTracker<T>
        where T : IRepositoryDependencyTracker<T>
    {
        public bool DependenciesAreReady { get; }
        public AppalachiaRepositoryDependencyTracker DependencyTracker { get; }
    }
}
