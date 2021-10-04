using UnityEngine;

namespace Appalachia.Core.Filtering
{
    public interface IComponentFilterLimitedTask<T>
        where T : Component
    {
        T[] RunFilter();
    }
}
