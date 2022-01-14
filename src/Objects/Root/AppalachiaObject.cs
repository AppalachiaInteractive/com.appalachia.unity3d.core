#region

using Unity.Profiling;
using UnityEngine;

// ReSharper disable StaticMemberInGenericType

#endregion

namespace Appalachia.Core.Objects.Root
{
    public abstract partial class AppalachiaObject : ScriptableObject
    {
        #region Profiling

        private const string _PRF_PFX = nameof(AppalachiaObject) + ".";

        #endregion
    }

    public abstract partial class AppalachiaObject<T> : AppalachiaObject
        where T : AppalachiaObject<T>
    {
        #region Constants and Static Readonly

        protected static readonly string _PRF_PFX = typeof(T).Name + ".";

        protected static readonly ProfilerMarker _PRF_AwakeActual =
            new ProfilerMarker(_PRF_PFX + nameof(AwakeActual));

        protected static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        protected static readonly ProfilerMarker _PRF_OnDestroyActual =
            new ProfilerMarker(_PRF_PFX + nameof(OnDestroyActual));

        protected static readonly ProfilerMarker _PRF_OnDisableActual =
            new ProfilerMarker(_PRF_PFX + nameof(OnDisableActual));

        protected static readonly ProfilerMarker _PRF_OnEnableActual =
            new ProfilerMarker(_PRF_PFX + nameof(OnEnableActual));

        protected static readonly ProfilerMarker _PRF_ResetActual =
            new ProfilerMarker(_PRF_PFX + nameof(ResetActual));

        protected static readonly ProfilerMarker _PRF_WhenDestroyed =
            new ProfilerMarker(_PRF_PFX + nameof(WhenDestroyed));

        protected static readonly ProfilerMarker _PRF_WhenDisabled =
            new ProfilerMarker(_PRF_PFX + nameof(WhenDisabled));

        protected static readonly ProfilerMarker _PRF_WhenEnabled =
            new ProfilerMarker(_PRF_PFX + nameof(WhenEnabled));

        #endregion

        #region Profiling

        #endregion
    }
}
