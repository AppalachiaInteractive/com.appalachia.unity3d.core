namespace Appalachia.Core.Objects.Root.Contracts
{
    public interface ISingletonBehaviour : ISingleton
    {
        void EnsureInstanceIsPrepared(ISingletonBehaviour callingInstance);
    }
}
