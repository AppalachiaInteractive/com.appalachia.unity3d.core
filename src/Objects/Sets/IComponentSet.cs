using UnityEngine;

namespace Appalachia.Core.Objects.Sets
{
    public interface IComponentSet<TSet, TSetMetadata>
        where TSet : IComponentSet<TSet, TSetMetadata>
        where TSetMetadata : IComponentSetMetadata<TSet, TSetMetadata>
    {
        public GameObject GameObject { get; }
        void CreateComponents(GameObject parent, string name /*, string prefixOverride = null*/);
    }
}
