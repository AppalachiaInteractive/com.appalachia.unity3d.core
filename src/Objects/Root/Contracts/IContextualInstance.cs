using Appalachia.CI.Constants;

namespace Appalachia.Core.Objects.Root.Contracts
{
    public interface IContextualInstance
    {
        AppaContext Context { get; }
    }
}
