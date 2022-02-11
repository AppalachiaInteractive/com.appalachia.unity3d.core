namespace Appalachia.Core.Objects.Root.Contracts
{
    public interface IOwned
    {
        UnityEngine.Object Owner { get; }

        void SetOwner(UnityEngine.Object owner);
    }
}
