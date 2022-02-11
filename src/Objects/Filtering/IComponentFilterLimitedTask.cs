using UnityEngine;

namespace Appalachia.Core.Objects.Filtering
{
    public interface IComponentFilterLimitedTask<T>
        where T : Component
    {
        T[] RunFilter();
    }
}
