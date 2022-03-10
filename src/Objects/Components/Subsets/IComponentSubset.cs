using Appalachia.Core.Objects.Components.Contracts;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Events.Contracts;
using UnityEngine;

namespace Appalachia.Core.Objects.Components.Subsets
{
    public interface IComponentSubset : IInitializable,
                                        IOwned,
                                        IUnique,
                                        IUnitySerializable,
                                        IBasicEventDriven,
                                        ISerializationCallbackReceiver,
                                        IChangePublisher,
                                        IUndoable
    {
        void DestroySafely(bool includeGameObject = true);
    }
}
