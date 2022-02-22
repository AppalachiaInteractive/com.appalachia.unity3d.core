namespace Appalachia.Core.Objects.Root.Contracts
{
    public interface IEnableNotifier
    {
        /// <summary>
        ///     Does this require a subscriber to its <see cref="RaiseNotificationWhenEnabled" /> publishing?
        ///     If so, the notifications will be stored until a subscriber becomes available.
        /// </summary>
        bool GuaranteedEventRouting { get; }
    }
}
