namespace Appalachia.Core.Objects.Root.Contracts
{
    public interface ISingleton<T>
    {
        bool HasInstance { get; }

        object InstanceWriteLock { get; }

        T instance { get; }

        void InvokeInstanceAvailable(T original, T current);

        void SetInstance(T i);
    }

    public interface ISingleton
    {
        bool IsReady { get; }

        ISingleton instance { get; }

        object InstanceWriteLock { get; }

        void InvokeInstanceAvailable(ISingleton original, ISingleton current);

        void SetInstance(ISingleton i);
    }
}
