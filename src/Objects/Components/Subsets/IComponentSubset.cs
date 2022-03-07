using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Events;
using Appalachia.Utility.Standards;
using UnityEngine;

namespace Appalachia.Core.Objects.Components.Subsets
{
    public interface IComponentSubset
    {
        /// <summary>
        ///     Offers a chance to respond to the completion of initialization.
        /// </summary>
        event InitializationCompleteHandler InitializationComplete;

        bool FullyInitialized { get; }
        bool HasBeenDisabled { get; }
        bool HasBeenEnabled { get; }
        bool HasBeenInitialized { get; }
        bool HasInitializationStarted { get; }
        bool HasInvokedInitializationCompleted { get; }
        Object Owner { get; }
        ObjectId ObjectId { get; }
        string Name { get; }
        void DestroySafely(bool includeGameObject = true);
        void MarkAsModified();
        void OnAfterDeserialize();

        void OnBeforeSerialize();
        void RecordUndo(string operation, string modifiedBy);
        void SetOwner(Object owner);
        void SubscribeToChanges(AppaEvent.Handler handler);
        void UnsubscribeFromChanges(AppaEvent.Handler handler);
    }
}
