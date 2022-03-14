using System.Collections.Generic;
using Appalachia.Core.ControlModel.Components.Contracts;
using Appalachia.Core.Objects.Root;

namespace Appalachia.Core.ControlModel.ComponentGroups.Contracts
{
    public interface IAppaComponentGroupConfig : IAppaComponentConfig, IRemotelyEnabled
    {
        IReadOnlyList<IAppaComponentConfig> ComponentConfigs { get; }
    }

    public interface IAppaComponentGroupConfig<in TGroup> : IAppaComponentGroupConfig, IAppaComponentConfig<TGroup>
    {
    }

    public interface IAppaComponentGroupConfig<in TGroup, TConfig> : IAppaComponentGroupConfig<TGroup>,
                                                                     IAppaComponentConfig<TGroup, TConfig>
        where TGroup : IAppaComponentGroup
        where TConfig : IAppaComponentGroupConfig<TGroup, TConfig>, new()
    {
    }
}
