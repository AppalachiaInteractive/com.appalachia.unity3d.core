
namespace Appalachia.Core.ObjectPooling
{
    public static class ObjectPoolInitializer
    {
        public static ObjectPool<T> Create<T>()
            where T : class, new()
        {
            return ObjectPoolProvider.Create<T>();
        }
    }
}
