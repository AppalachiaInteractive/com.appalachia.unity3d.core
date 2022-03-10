using Appalachia.Core.Objects.Root.Contracts;
using UnityEngine;

namespace Appalachia.Core.Objects.Components.Core
{
    public interface IComponentData : IOwned, IUnique
    {
        bool SharesControlError { get; set; }
        string SharesControlWith { get; set; }
        void InitializeFields(Object owner);
        void ResetData();
    }

    public interface IComponentData<in TComponent> : IComponentData
    {
        void Apply(TComponent comp);
    }

    public interface IComponentData<in TComponent, TComponentData> : IComponentData<TComponent>
        where TComponentData : IComponentData<TComponent, TComponentData>, new()
    {
    }
}
