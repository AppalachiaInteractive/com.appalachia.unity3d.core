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

    public interface IComponentSet<TSet, TSetMetadata> : IComponentSet
        where TSet : IComponentSet<TSet, TSetMetadata>, new()
        where TSetMetadata : IComponentSetMetadata<TSet, TSetMetadata>
    {
        void GetOrAddComponents(
            GameObject parent,
            string name,
            TSetMetadata data);
    }
}
