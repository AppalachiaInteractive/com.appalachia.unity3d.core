using Appalachia.Core.Objects.Components.Contracts;
using Appalachia.Core.Objects.Components.Core;

namespace Appalachia.Core.Objects.Components.Sets
{
    public interface IComponentSetData : IComponentData, IRemotelyEnabled
    {
    }

    public interface IComponentSetData<in TComponentSet> : IComponentSetData, IComponentData<TComponentSet>
    {
    }

    public interface IComponentSetData<in TComponentSet, TComponentSetData> : IComponentSetData<TComponentSet>,
                                                                              IComponentData<TComponentSet,
                                                                                  TComponentSetData>
        where TComponentSet : IComponentSet<TComponentSet, TComponentSetData>, new()
        where TComponentSetData : IComponentSetData<TComponentSet, TComponentSetData>, new()
    {
    }
}
