namespace Appalachia.Core.Objects.Sets
{
    public interface IComponentSetData<in TSet, TSetData>
        where TSet : IComponentSet<TSet, TSetData>, new()
        where TSetData : IComponentSetData<TSet, TSetData>
    {
        void ApplyToComponentSet(TSet componentSet);
    }
}
