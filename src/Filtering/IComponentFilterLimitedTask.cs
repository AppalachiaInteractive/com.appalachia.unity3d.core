using UnityEngine;

namespace Appalachia.Filtering
{
    public interface IComponentFilterLimitedTask<T>
        where T : Component
    {
        T[] RunFilter();
    }
}