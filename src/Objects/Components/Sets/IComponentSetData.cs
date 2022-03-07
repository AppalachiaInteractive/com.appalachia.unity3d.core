namespace Appalachia.Core.Objects.Components.Sets
{
    public interface IComponentSetData<in TComponentSet, TComponentSetData>
        where TComponentSet : IComponentSet<TComponentSet, TComponentSetData>, new()
        where TComponentSetData : IComponentSetData<TComponentSet, TComponentSetData>
    {
        void OnApply(TComponentSet componentSet);
    }
}
