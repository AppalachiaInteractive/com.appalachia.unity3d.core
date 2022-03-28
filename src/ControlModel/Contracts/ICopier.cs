namespace Appalachia.Core.ControlModel.Contracts
{
    public interface ICopier<in T>
        where T : ICopier<T>
    {
        void CopyTo(T other);
    }
}
