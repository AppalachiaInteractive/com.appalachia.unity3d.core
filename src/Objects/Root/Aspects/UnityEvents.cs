using System;
using Unity.Profiling;

// ReSharper disable NotAccessedField.Local
// ReSharper disable StaticMemberInGenericType
#pragma warning disable CS0414

namespace Appalachia.Core.Objects.Root
{
    [Flags]
    internal enum UnityEventFlags
    {
        None = 0,
        Awake = 1 << 0,
        OnEnable = 1 << 1,
        Start = 1 << 2,
        Reset = 1 << 3,
        OnDisable = 1 << 4,
        OnDestroy = 1 << 5,
    }

    [Flags]
    internal enum AppalachiaEventFlags
    {
        None = 0,
        Initialized = 1 << 0,
        Enabled = 1 << 1,
    }

    public partial class AppalachiaRepository
    {
    }

    public partial class AppalachiaObject<T>
    {
    }

    public partial class SingletonAppalachiaObject<T>
    {
    }

    public partial class AppalachiaBehaviour<T>
    {
        /// <inheritdoc />
        protected override bool ShouldSkipUpdate => base.ShouldSkipUpdate || !DependenciesAreReady;

        #region Profiling

        protected static readonly string _PRF_PFX8 = typeof(T).Name + ".";

        protected static readonly ProfilerMarker _PRF_NotifyWhenEnabled =
            new ProfilerMarker(_PRF_PFX8 + nameof(RaiseNotificationWhenEnabled));

        #endregion
    }
    
    public partial class SingletonAppalachiaBehaviour<T>
    {
    }

    public partial class AppalachiaSimpleBase
    {
    }

    public partial class AppalachiaBase<T>
    {
    }

    public partial class AppalachiaSimplePlayable
    {
    }

    public partial class AppalachiaPlayable
    {
    }

    public partial class AppalachiaPlayable<T>
    {
    }
}
#pragma warning restore CS0414
