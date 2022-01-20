using UnityEngine;

namespace Appalachia.Core.Objects.Sets
{
    public interface IComponentSet
    {
        string ComponentSetName { get; }
        
        public GameObject GameObject { get; }
    }

    public interface IComponentSet<TSet, TSetMetadata> : IComponentSet
        where TSet : IComponentSet<TSet, TSetMetadata>, new()
        where TSetMetadata : IComponentSetMetadata<TSet, TSetMetadata>
    {
        void GetOrAddComponents(
            GameObject parent,
            string name,
            TSetMetadata data /*, string prefixOverride = null*/);
    }
}
