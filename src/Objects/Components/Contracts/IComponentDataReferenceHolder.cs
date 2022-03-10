using Appalachia.Core.Objects.Components.Core;
using Appalachia.Utility.Standards;

namespace Appalachia.Core.Objects.Components.Contracts
{
    public interface IComponentDataReferenceHolder
    {
        public ObjectID ComponentDataID { get; set; }
    }

    public interface IComponentDataReferenceHolder<out T>
        where T : IComponentData
    {
        public T ComponentData { get; }
    }
}
