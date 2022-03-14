using System.Collections.Generic;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Events.Contracts;
using UnityEngine;

namespace Appalachia.Core.ControlModel.ComponentGroups.Contracts
{
    public interface IAppaComponentGroup : IBehaviour, IChangePublisher
    {
        IReadOnlyList<Component> Components { get; }
        string NamePostfix { get; }
        void ApplyConfig();
        void DestroySafely(bool includeGameObject);
        void Refresh();
    }

    public interface IAppaComponentGroup<in TGroup> : IAppaComponentGroup
    {
    }

    public interface IAppaComponentGroup<in TGroup, TConfig> : IAppaComponentGroup<TGroup>
        where TGroup : IAppaComponentGroup
        where TConfig : IAppaComponentGroupConfig<TGroup, TConfig>, new()
    {
        public TConfig Config { get; set; }
    }
}
