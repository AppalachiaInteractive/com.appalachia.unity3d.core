using System;
using Appalachia.Core.Objects.Root;
using Unity.Profiling;

namespace Appalachia.Core.ControlModel.Components
{
    public abstract partial class AppaComponentConfig<TComponent, TConfig> : IRemotelyEnabled
    {
        #region Fields and Autoproperties

        [NonSerialized] private Func<bool> _shouldEnableFunction;

        #endregion

        #region IRemotelyEnabled Members

        public Func<bool> ShouldEnableFunction
        {
            get => _shouldEnableFunction;
            set => _shouldEnableFunction = value;
        }

        public bool ShouldEnable => _shouldEnableFunction?.Invoke() ?? true;

        public void BindEnabledStateTo(IRemotelyEnabledController controller)
        {
            using (_PRF_BindEnabledStateTo.Auto())
            {
                ShouldEnableFunction = () => controller.ShouldEnable;
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_BindEnabledStateTo =
            new ProfilerMarker(_PRF_PFX + nameof(BindEnabledStateTo));

        #endregion
    }
}
