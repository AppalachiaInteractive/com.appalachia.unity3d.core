using Appalachia.CI.Constants;

namespace Appalachia.Core.Objects.Root.Contracts
{
    public interface IContextualStatic
    {
        AppaContext StaticContext { get; }
    }
}
