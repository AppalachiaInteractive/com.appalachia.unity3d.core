using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Events.Contracts;

namespace Appalachia.Core.ControlModel.Components.Contracts
{
    public interface IAppaComponentConfig : IOwned, IUnique, IChangePublisher
    {
        bool SharesControlError { get; set; }
        string SharesControlWith { get; set; }
        void ResetConfig();
    }

    public interface IAppaComponentConfig<in TComponent> : IAppaComponentConfig
    {
        void Apply(TComponent comp);
    }

    public interface IAppaComponentConfig<in TComponent, TConfig> : IAppaComponentConfig<TComponent>
        where TConfig : IAppaComponentConfig<TComponent, TConfig>, new()
    {
    }
}
