using Appalachia.Core.ControlModel.Components.Contracts;
using Appalachia.Core.Objects.Root;

namespace Appalachia.Core.ControlModel.Controls.Contracts
{
    public interface IAppaControlConfig : IAppaComponentConfig, IRemotelyEnabled
    {
    }

    public interface IAppaControlConfig<in TControl> : IAppaControlConfig, IAppaComponentConfig<TControl>
    {
    }

    public interface IAppaControlConfig<in TControl, TConfig> : IAppaControlConfig<TControl>,
                                                                IAppaComponentConfig<TControl, TConfig>
        where TControl : IAppaControl<TControl, TConfig>
        where TConfig : IAppaControlConfig<TControl, TConfig>, new()
    {
    }
}
