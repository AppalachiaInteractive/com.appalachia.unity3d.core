namespace Appalachia.Core.Objects.Sets
{
    public interface IComponentSetMetadata<in TSet, TSetMetadata>
        where TSet : IComponentSet<TSet, TSetMetadata>
        where TSetMetadata : IComponentSetMetadata<TSet, TSetMetadata>
    {
        void ConfigureComponents(TSet componentSet);
    }
}
