using Appalachia.Utility.Standards;

namespace Appalachia.Core.Events.Contracts
{
    public interface IUniqueSubscriber
    {
        public ObjectId ObjectId { get; }
    }
}
