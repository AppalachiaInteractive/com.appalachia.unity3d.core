using UnityEngine;

namespace Appalachia.Filtering
{
    public interface IComponentFilterSortedTask<T>
        where T : Component
    {
        T[] RunFilter();
        IComponentFilterLimitedTask<T> LimitResults(int count);
    }
}