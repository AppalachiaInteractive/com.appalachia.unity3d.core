namespace Appalachia.Core.Objects.Root.Contracts
{
    public interface IOwned
    {
        public bool HasOwner { get; }
        public UnityEngine.Object Owner { get; }

        public void SetOwner(UnityEngine.Object owner);
    }
}
