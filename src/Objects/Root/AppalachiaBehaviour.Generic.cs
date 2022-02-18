using Unity.Profiling;

namespace Appalachia.Core.Objects.Root
{
    public abstract partial class AppalachiaBehaviour<T> : AppalachiaBehaviour
        where T : AppalachiaBehaviour<T>
    {
        #region Profiling

        protected static readonly string _PRF_PFX = typeof(T).Name + ".";

        protected static readonly ProfilerMarker _PRF_AwakeActual =
            new ProfilerMarker(_PRF_PFX + nameof(AwakeActual));

        protected static readonly ProfilerMarker _PRF_FixedUpdate =
            new ProfilerMarker(_PRF_PFX + "FixedUpdate");

        protected static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        protected static readonly ProfilerMarker
            _PRF_LateUpdate = new ProfilerMarker(_PRF_PFX + "LateUpdate");

        protected static readonly ProfilerMarker _PRF_OnDestroyActual =
            new ProfilerMarker(_PRF_PFX + nameof(OnDestroyActual));

        protected static readonly ProfilerMarker _PRF_OnDisableActual =
            new ProfilerMarker(_PRF_PFX + nameof(OnDisableActual));

        protected static readonly ProfilerMarker _PRF_OnDrawGizmos =
            new ProfilerMarker(_PRF_PFX + "OnDrawGizmos");

        protected static readonly ProfilerMarker _PRF_OnDrawGizmosSelected =
            new ProfilerMarker(_PRF_PFX + "OnDrawGizmosSelected");

        protected static readonly ProfilerMarker _PRF_OnEnableActual =
            new ProfilerMarker(_PRF_PFX + nameof(OnEnableActual));

        protected static readonly ProfilerMarker _PRF_ResetActual =
            new ProfilerMarker(_PRF_PFX + nameof(ResetActual));

        protected static readonly ProfilerMarker _PRF_StartActual =
            new ProfilerMarker(_PRF_PFX + nameof(StartActual));

        protected static readonly ProfilerMarker _PRF_Update = new ProfilerMarker(_PRF_PFX + "Update");

        protected static readonly ProfilerMarker _PRF_WhenDestroyed =
            new ProfilerMarker(_PRF_PFX + nameof(WhenDestroyed));

        protected static readonly ProfilerMarker _PRF_WhenDisabled =
            new ProfilerMarker(_PRF_PFX + nameof(WhenDisabled));

        protected static readonly ProfilerMarker _PRF_WhenEnabled =
            new ProfilerMarker(_PRF_PFX + nameof(WhenEnabled));

        #endregion
    }
}
