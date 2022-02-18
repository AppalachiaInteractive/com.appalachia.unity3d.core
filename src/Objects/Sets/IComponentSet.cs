using UnityEngine;

namespace Appalachia.Core.Objects.Sets
{
    public interface IComponentSet
    {
        public GameObject GameObject { get; }
        string ComponentSetName { get; }

        void DestroySet();
        void DisableSet();
        void EnableSet();
    }

    public interface IComponentSet<TSet, TSetData> : IComponentSet
        where TSet : IComponentSet<TSet, TSetData>, new()
        where TSetData : IComponentSetData<TSet, TSetData>
    {
        void GetOrAddComponents(TSetData data, GameObject parent, string name);
    }
}
