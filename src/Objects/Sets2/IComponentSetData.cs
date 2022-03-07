namespace Appalachia.Core.Objects.Sets2
{
    public interface IComponentSetData<in TComponentSet, TComponentSetData>
        where TComponentSet : IComponentSet<TComponentSet, TComponentSetData>, new()
        where TComponentSetData : IComponentSetData<TComponentSet, TComponentSetData>
    {
        void ApplyToComponentSet(TComponentSet componentSet);
    }
}
