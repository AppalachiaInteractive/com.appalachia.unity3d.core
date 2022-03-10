using Appalachia.Core.Objects.Components.Contracts;
using Appalachia.Core.Objects.Components.Core;

namespace Appalachia.Core.Objects.Components.Subsets
{
    public interface IComponentSubsetData : IComponentData, IRemotelyEnabled
    {
    }

    public interface IComponentSubsetData<in TComponentSubset> : IComponentSubsetData, IComponentData<TComponentSubset>
    {
    }

    public interface IComponentSubsetData<in TComponentSubset, TComponentSubsetData> :
        IComponentSubsetData<TComponentSubset>,
        IComponentData<TComponentSubset, TComponentSubsetData>
        where TComponentSubset : IComponentSubset
        where TComponentSubsetData : IComponentSubsetData<TComponentSubset, TComponentSubsetData>, new()
    {
    }
}
