using UnityEngine;

namespace Appalachia.Core.Objects.Sets2
{
    public interface IComponentSet
    {
        public GameObject GameObject { get; }
        string ComponentSetNamePrefix { get; }
    }

    public interface IComponentSet<TComponentSet, TComponentSetData> : IComponentSet
        where TComponentSet : IComponentSet<TComponentSet, TComponentSetData>, new()
        where TComponentSetData : IComponentSetData<TComponentSet, TComponentSetData>
    {
        void GetOrAddComponents(GameObject parent, string name);
    }
}
