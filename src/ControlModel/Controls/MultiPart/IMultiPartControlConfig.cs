using Appalachia.Core.Collections;
using Appalachia.Core.ControlModel.ComponentGroups;
using Appalachia.Core.ControlModel.Controls.Contracts;

namespace Appalachia.Core.ControlModel.Controls.MultiPart
{
    public interface IMultiPartControlConfig<TGroup, TGroupList, TGroupConfig,
                                             out TGroupConfigList> : IAppaControlConfig
        where TGroup : AppaComponentGroup<TGroup, TGroupConfig>, new()
        where TGroupList : AppaList<TGroup>, new()
        where TGroupConfig : AppaComponentGroupConfig<TGroup, TGroupConfig>, new()
        where TGroupConfigList : AppaList<TGroupConfig>
    {
        TGroupConfigList ConfigList { get; }
    }
}
