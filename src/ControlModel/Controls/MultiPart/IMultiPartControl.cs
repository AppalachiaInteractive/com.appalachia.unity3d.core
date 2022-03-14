using Appalachia.Core.Collections;
using Appalachia.Core.ControlModel.ComponentGroups;
using Appalachia.Core.ControlModel.Controls.Contracts;

namespace Appalachia.Core.ControlModel.Controls.MultiPart
{
    public interface IMultiPartControl<TGroup, out TGroupList, TGroupConfig,
                                                 out TGroupConfigList> : IAppaControl
        where TGroup : AppaComponentGroup<TGroup, TGroupConfig>, new()
        where TGroupList : AppaList<TGroup>, new()
        where TGroupConfig : AppaComponentGroupConfig<TGroup, TGroupConfig>, new()
        where TGroupConfigList : AppaList<TGroupConfig>
    {
        TGroupList GroupList { get; }
    }
}
