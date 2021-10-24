using UnityEngine;

namespace Appalachia.Core.Filtering
{
    public interface IComponentFilterSortedTask<T>
        where T : Component
    {
        IComponentFilterLimitedTask<T> LimitResults(int count);
        T[] RunFilter();
    }
}
