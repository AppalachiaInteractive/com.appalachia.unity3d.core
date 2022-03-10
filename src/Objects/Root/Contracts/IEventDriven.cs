namespace Appalachia.Core.Objects.Root.Contracts
{
    public interface IBasicEventDriven
    {
        bool HasBeenDisabled { get; }
        bool HasBeenEnabled { get; }
        bool HasBeenInitialized { get; }
    }

    public interface IEventDriven : IBasicEventDriven
    {
        bool HasBeenOrIsBeingDisabled { get; }
        bool HasBeenOrIsBeingEnabled { get; }
        bool HasBeenOrIsBeingInitialized { get; }
        bool IsBeingDisabled { get; }
        bool IsBeingEnabled { get; }
        bool IsBeingInitialized { get; }
        int AwakeDuration { get; }
        int AwakeFrame { get; }
        int DisabledDuration { get; }
        int EnabledDuration { get; }
        int OnDisableFrame { get; }
        int OnEnableFrame { get; }
    }
}
