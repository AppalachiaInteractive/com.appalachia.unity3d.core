namespace Appalachia.Core.Objects.Root.Contracts
{
    public interface IOwned
    {
        bool SharesOwnership { get; set; }
        bool HasOwner { get; }
        UnityEngine.Object Owner { get; }

        void SetOwner(UnityEngine.Object owner);
    }
}
