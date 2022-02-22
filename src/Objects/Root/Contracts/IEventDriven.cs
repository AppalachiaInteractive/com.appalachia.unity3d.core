namespace Appalachia.Core.Objects.Root.Contracts
{
    public interface IEventDriven
    {
        bool HasBeenOrIsBeingDisabled { get; }
        bool HasBeenOrIsBeingEnabled { get; }
        bool HasBeenOrIsBeingInitialized { get; }
        bool IsBeingDisabled { get; }
        bool IsBeingEnabled { get; }
        bool IsBeingInitialized { get; }
        bool HasBeenDisabled { get; }
        bool HasBeenEnabled { get; }
        bool HasBeenInitialized { get; }
        int AwakeDuration { get; }
        int AwakeFrame { get; }
        int DisabledDuration { get; }
        int EnabledDuration { get; }
        int OnDisableFrame { get; }
        int OnEnableFrame { get; }
    }
}
