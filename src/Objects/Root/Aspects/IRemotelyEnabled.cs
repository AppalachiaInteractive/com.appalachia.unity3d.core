using System;

namespace Appalachia.Core.Objects.Root
{
    public interface IRemotelyEnabled
    {
        bool ShouldEnable { get; }

        Func<bool> ShouldEnableFunction { get; set; }

        void BindEnabledStateTo(IRemotelyEnabledController controller);
    }
}
