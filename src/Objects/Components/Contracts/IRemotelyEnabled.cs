using System;

namespace Appalachia.Core.Objects.Components.Contracts
{
    public interface IRemotelyEnabled
    {
        bool ShouldEnable { get; }

        Func<bool> ShouldEnableFunction { get; set; }

        void BindEnabledStateTo(IRemotelyEnabledController controller);
    }
}
