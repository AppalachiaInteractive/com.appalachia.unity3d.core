using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Events.Contracts;

namespace Appalachia.Core.ControlModel.Controls.Contracts
{
    public interface IAppaControl : IBehaviour, IChangePublisher
    {
        string NamePostfix { get; }
        void ApplyConfig();
        void DestroySafely(bool includeGameObject);
        void Refresh();
    }

    public interface IAppaControl<TControl> : IAppaControl
    {
    }

    public interface IAppaControl<TControl, TConfig> : IAppaControl<TControl>
        where TControl : IAppaControl<TControl, TConfig>
        where TConfig : IAppaControlConfig<TControl, TConfig>, new()
    {
        public TConfig Config { get; set; }
    }
}
