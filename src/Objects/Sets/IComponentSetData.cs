namespace Appalachia.Core.Objects.Sets
{
    public interface IComponentSetData<in TComponentSet, TComponentSetData>
        where TComponentSet : IComponentSet<TComponentSet, TComponentSetData>, new()
        where TComponentSetData : IComponentSetData<TComponentSet, TComponentSetData>
    {
        void ApplyToComponentSet(TComponentSet componentSet);
    }
}
