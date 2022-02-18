namespace Appalachia.Core.Objects.Root.Contracts
{
    public interface IEventDriven
    {
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
