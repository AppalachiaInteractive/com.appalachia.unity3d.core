using UnityEngine;

namespace Appalachia.Core.Objects.Root.Contracts
{
    public interface IBehaviour : IInitializable, IEventDriven
    {
        GameObject GameObject { get; }
        int InstanceID { get; }
        Transform Transform { get; }
    }
}
