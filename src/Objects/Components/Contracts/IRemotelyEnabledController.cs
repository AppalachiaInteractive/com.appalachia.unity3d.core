namespace Appalachia.Core.Objects.Components.Contracts
{
    public interface IRemotelyEnabledController
    {
        public bool ShouldEnable { get; }
        public void BindValueEnabledState();
    }
}
