namespace Appalachia.Core.Objects.Root
{
    public interface IRemotelyEnabledController
    {
        public bool ShouldEnable { get; }
        public void BindValueEnabledState();
    }
}
