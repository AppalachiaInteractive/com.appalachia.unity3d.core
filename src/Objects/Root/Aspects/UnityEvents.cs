using System;
using System.Collections.Generic;
using System.Linq;
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Objects.Dependencies;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Core.Objects.Routing;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Enums;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Strings;
using Appalachia.Utility.Timing;
using Unity.Profiling;
using UnityEngine;

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

    public partial class AppalachiaObject : IEventDriven
    {
        #region Static Fields and Autoproperties

        private static string _deleted;

        #endregion

        #region Fields and Autoproperties

        [NonSerialized] private int _awakeFrame;
        [NonSerialized] private int _resetFrame;
        [NonSerialized] private int _onEnableFrame;
        [NonSerialized] private int _onDisableFrame;

        [NonSerialized] private AppalachiaEventFlags _eventFlags;
        [NonSerialized] private UnityEventFlags _unityEventFlags;

        [NonSerialized] private bool _unityAwake;
        [NonSerialized] private bool _unityOnEnable;
        [NonSerialized] private bool _unityReset;
        [NonSerialized] private bool _unityOnDisable;
        [NonSerialized] private bool _unityOnDestroy;

        [NonSerialized] private bool _hasBeenInitialized;
        [NonSerialized] private bool _hasBeenEnabled;
        [NonSerialized] private bool _hasBeenDisabled;

        #endregion

        protected static string DELETED
        {
            get
            {
                if (_deleted == null)
                {
                    _deleted = "[DELETED]".Bold().FormatNameForLogging();
                }

                return _deleted;
            }
        }

        protected virtual bool LogEventFunctions => false;

        #region Event Functions

        protected void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                if (LogEventFunctions)
                {
                    Context.Log.Info(nameof(Awake), this);
                }

                RemapUnityEvents(UnityEventFlags.Awake).Forget(LogRemapException);
            }
        }

        protected virtual void Reset()
        {
            using (_PRF_Reset.Auto())
            {
                if (LogEventFunctions)
                {
                    Context.Log.Info(nameof(Reset), this);
                }

                RemapUnityEvents(UnityEventFlags.Reset).Forget(LogRemapException);
            }
        }

        protected void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                _onEnableFrame = CoreClock.Instance.FrameCount;

                if (LogEventFunctions)
                {
                    Context.Log.Info(nameof(OnEnable), this);
                }

                RemapUnityEvents(UnityEventFlags.OnEnable).Forget(LogRemapException);
            }
        }

        protected void OnDisable()
        {
            using (_PRF_OnDisable.Auto())
            {
                _onDisableFrame = CoreClock.Instance.FrameCount;

                if (LogEventFunctions)
                {
                    Context.Log.Info(nameof(OnDisable), this);
                }

                RemapUnityEvents(UnityEventFlags.OnDisable).Forget(LogRemapException);
            }
        }

        protected void OnDestroy()
        {
            using (_PRF_OnDestroy.Auto())
            {
                if (LogEventFunctions)
                {
                    Context.Log.Info(nameof(OnDestroy), this);
                }

                RemapUnityEvents(UnityEventFlags.OnDestroy).Forget(LogRemapException);
            }
        }

        #endregion

        protected virtual void AwakeActual()
        {
        }

        protected virtual void OnDestroyActual()
        {
        }

        protected virtual void OnDisableActual()
        {
        }

        protected virtual void OnEnableActual()
        {
        }

        protected virtual void ResetActual()
        {
        }

        protected virtual async AppaTask WhenDestroyed()
        {
            await AppaTask.CompletedTask;
        }

        protected virtual async AppaTask WhenDisabled()
        {
            await AppaTask.CompletedTask;
        }

        protected virtual async AppaTask WhenEnabled()
        {
            await AppaTask.CompletedTask;
        }

        private void LogRemapException(Exception ex)
        {
            using (_PRF_LogRemapException.Auto())
            {
                var stackSplits = ex.StackTrace.Split('\n').Select(l => l.Trim()).ToArray();

                Context.Log.Error(stackSplits[0], this, ex);
            }
        }

        private async AppaTask RemapUnityEvents(UnityEventFlags currentUnityEventType)
        {
            if (this == null)
            {
                return;
            }

            try
            {
                await RemapUnityEventsInternal(currentUnityEventType);
            }
            catch (Exception ex)
            {
                Context.Log.Error(
                    ZString.Format(
                        "Error remapping {0} for {1} on {2}.",
                        currentUnityEventType.ToString().FormatMethodForLogging(),
                        GetType().FormatForLogging(),
                        this == null ? DELETED : name.FormatNameForLogging()
                    ),
                    this,
                    ex
                );

                throw;
            }
        }

        private async AppaTask RemapUnityEventsInternal(UnityEventFlags currentUnityEventType)
        {
            await AppalachiaRepositoryDependencyManager.ValidateDependencies(GetType());

            if (this == null)
            {
                return;
            }

            if (_unityEventFlags.HasFlag(UnityEventFlags.OnDestroy))
            {
                return;
            }

            _unityEventFlags |= currentUnityEventType;

            switch (currentUnityEventType)
            {
                case UnityEventFlags.Awake:

                    _awakeFrame = CoreClock.Instance.FrameCount;
                    _unityAwake = true;
                    try
                    {
                        AwakeActual();
                    }
                    catch (Exception ex)
                    {
                        Context.Log.Error(
                            ZString.Format(
                                "Error executing {0} for {1} on {2}.",
                                nameof(AwakeActual).FormatMethodForLogging(),
                                GetType().FormatForLogging(),
                                this == null ? DELETED : name.FormatNameForLogging()
                            ),
                            this,
                            ex
                        );

                        throw;
                    }

                    break;
                case UnityEventFlags.OnEnable:

                    _onEnableFrame = CoreClock.Instance.FrameCount;
                    _unityOnEnable = true;
                    try
                    {
                        OnEnableActual();
                    }
                    catch (Exception ex)
                    {
                        Context.Log.Error(
                            ZString.Format(
                                "Error executing {0} for {1} on {2}.",
                                nameof(OnEnableActual).FormatMethodForLogging(),
                                GetType().FormatForLogging(),
                                this == null ? DELETED : name.FormatNameForLogging()
                            ),
                            this,
                            ex
                        );

                        throw;
                    }

                    break;
                case UnityEventFlags.Reset:

                    _resetFrame = CoreClock.Instance.FrameCount;
                    _unityReset = true;
                    try
                    {
                        ResetActual();
                    }
                    catch (Exception ex)
                    {
                        Context.Log.Error(
                            ZString.Format(
                                "Error executing {0} for {1} on {2}.",
                                nameof(ResetActual).FormatMethodForLogging(),
                                GetType().FormatForLogging(),
                                this == null ? DELETED : name.FormatNameForLogging()
                            ),
                            this,
                            ex
                        );

                        throw;
                    }

                    break;
                case UnityEventFlags.OnDisable:

                    _onDisableFrame = CoreClock.Instance.FrameCount;
                    _unityOnDisable = true;
                    try
                    {
                        OnDisableActual();
                    }
                    catch (Exception ex)
                    {
                        Context.Log.Error(
                            ZString.Format(
                                "Error executing {0} for {1} on {2}.",
                                nameof(OnDisableActual).FormatMethodForLogging(),
                                GetType().FormatForLogging(),
                                this == null ? DELETED : name.FormatNameForLogging()
                            ),
                            this,
                            ex
                        );

                        throw;
                    }

                    break;
                case UnityEventFlags.OnDestroy:

                    _unityOnDestroy = true;
                    try
                    {
                        OnDestroyActual();
                    }
                    catch (Exception ex)
                    {
                        Context.Log.Error(
                            ZString.Format(
                                "Error executing {0} for {1} on {2}.",
                                nameof(OnDestroyActual).FormatMethodForLogging(),
                                GetType().FormatForLogging(),
                                this == null ? DELETED : name.FormatNameForLogging()
                            ),
                            this,
                            ex
                        );

                        throw;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_unityEventFlags), _unityEventFlags, null);
            }

            if (currentUnityEventType == UnityEventFlags.OnDestroy)
            {
                _eventFlags = AppalachiaEventFlags.None;
                try
                {
                    await WhenDestroyed();
                }
                catch (Exception ex)
                {
                    Context.Log.Error(
                        ZString.Format(
                            "Error executing {0} for {1} on {2}.",
                            nameof(WhenDestroyed).FormatMethodForLogging(),
                            GetType().FormatForLogging(),
                            this == null ? DELETED : name.FormatNameForLogging()
                        ),
                        this,
                        ex
                    );

                    throw;
                }

                _hasBeenInitialized = false;
                return;
            }

            if (currentUnityEventType == UnityEventFlags.OnDisable)
            {
                _eventFlags = _eventFlags.UnsetFlag(AppalachiaEventFlags.Enabled);
                try
                {
                    await WhenDisabled();
                }
                catch (Exception ex)
                {
                    Context.Log.Error(
                        ZString.Format(
                            "Error executing {0} for {1} on {2}.",
                            nameof(WhenDisabled).FormatMethodForLogging(),
                            GetType().FormatForLogging(),
                            this == null ? DELETED : name.FormatNameForLogging()
                        ),
                        this,
                        ex
                    );

                    throw;
                }

                _hasBeenEnabled = false;
                _hasBeenDisabled = true;

                return;
            }

            if (!_eventFlags.Has(AppalachiaEventFlags.Initialized))
            {
                try
                {
                    await ExecuteInitialization();
                }
                catch (Exception ex)
                {
                    Context.Log.Error(
                        ZString.Format(
                            "Error executing {0} for {1} on {2}.",
                            nameof(ExecuteInitialization).FormatMethodForLogging(),
                            GetType().FormatForLogging(),
                            this == null ? DELETED : name.FormatNameForLogging()
                        ),
                        this,
                        ex
                    );

                    throw;
                }

                _eventFlags |= AppalachiaEventFlags.Initialized;
                _hasBeenInitialized = true;
                await AppaTask.Yield();
            }

            if (currentUnityEventType == UnityEventFlags.OnEnable)
            {
                _eventFlags = _eventFlags.SetFlag(AppalachiaEventFlags.Enabled);
                try
                {
                    await WhenEnabled();
                }
                catch (Exception ex)
                {
                    Context.Log.Error(
                        ZString.Format(
                            "Error executing {0} for {1} on {2}.",
                            nameof(WhenEnabled).FormatMethodForLogging(),
                            GetType().FormatForLogging(),
                            this == null ? DELETED : name.FormatNameForLogging()
                        ),
                        this,
                        ex
                    );

                    throw;
                }

                _hasBeenEnabled = true;
                _hasBeenDisabled = false;
            }
        }

        #region IEventDriven Members

        public bool HasBeenDisabled => _hasBeenDisabled;
        public bool HasBeenEnabled => _hasBeenEnabled;

        public bool HasBeenInitialized => _hasBeenInitialized;

        public int AwakeDuration => CoreClock.Instance.FrameCount - _awakeFrame;
        public int AwakeFrame => _awakeFrame;
        public int DisabledDuration => CoreClock.Instance.FrameCount - _onDisableFrame;
        public int EnabledDuration => CoreClock.Instance.FrameCount - _onEnableFrame;
        public int OnDisableFrame => _onDisableFrame;
        public int OnEnableFrame => _onEnableFrame;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_LogRemapException =
            new ProfilerMarker(_PRF_PFX + nameof(LogRemapException));

        private static readonly ProfilerMarker _PRF_Reset = new ProfilerMarker(_PRF_PFX + nameof(Reset));
        private static readonly ProfilerMarker _PRF_Awake = new ProfilerMarker(_PRF_PFX + nameof(Awake));

        private static readonly ProfilerMarker
            _PRF_OnEnable = new ProfilerMarker(_PRF_PFX + nameof(OnEnable));

        private static readonly ProfilerMarker _PRF_OnDisable =
            new ProfilerMarker(_PRF_PFX + nameof(OnDisable));

        private static readonly ProfilerMarker _PRF_OnDestroy =
            new ProfilerMarker(_PRF_PFX + nameof(OnDestroy));

        #endregion
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

    /// <summary>
    ///     The base class from which every Appalachia <see cref="Behaviour" /> derives.
    /// </summary>
    /// <remarks>
    ///     The <see cref="AppalachiaBehaviour{T}" /> provides a feature-rich
    ///     base class to jump-start a "do-er" class.
    ///     <list type="bullet">
    ///         <item>It supports asynchronous initialization via the <see cref="Initialize" /> method.</item>
    ///         <item>It offers instance-based availability subscription via its <see cref="When" /> property.</item>
    ///         <item>Logging is available via its <see cref="Context" />.</item>
    ///         <item>
    ///             It offers the following events:
    ///             <list type="bullet">
    ///                 <item>
    ///                     <see cref="WhenEnabled" />
    ///                 </item>
    ///                 <item>
    ///                     <see cref="WhenDisabled" />
    ///                 </item>
    ///                 <item>
    ///                     <see cref="WhenDestroyed" />
    ///                 </item>
    ///             </list>
    ///         </item>
    ///     </list>
    /// </remarks>
    public partial class AppalachiaBehaviour : IEventDriven
    {
        #region Static Fields and Autoproperties

        private static string _deleted;

        #endregion

        #region Fields and Autoproperties

        [NonSerialized] private int _awakeFrame;
        [NonSerialized] private int _startFrame;
        [NonSerialized] private int _resetFrame;
        [NonSerialized] private int _onEnableFrame;
        [NonSerialized] private int _onDisableFrame;

        [NonSerialized] private AppalachiaEventFlags _eventFlags;
        [NonSerialized] private UnityEventFlags _unityEventFlags;

        [NonSerialized] private bool _unityAwake;
        [NonSerialized] private bool _unityOnEnable;
        [NonSerialized] private bool _unityStart;
        [NonSerialized] private bool _unityReset;
        [NonSerialized] private bool _unityOnDisable;
        [NonSerialized] private bool _unityOnDestroy;

        [NonSerialized] private bool _hasBeenInitialized;
        [NonSerialized] private bool _hasBeenEnabled;
        [NonSerialized] private bool _hasBeenDisabled;

        #endregion

        /// <summary>
        ///     A cached formatted string for logging the word "[DELETED]" to the console.
        /// </summary>
        protected static string DELETED
        {
            get
            {
                if (_deleted == null)
                {
                    _deleted = "[DELETED]".Bold().FormatNameForLogging();
                }

                return _deleted;
            }
        }

        /// <summary>
        ///     Does this <see cref="Behaviour" /> require a subscriber to its <see cref="NotifyWhenEnabled" /> publishing?
        ///     If so, the notifications will be stored until a subscriber becomes available.
        /// </summary>
        protected virtual bool GuaranteedEventRouting => false;

        /// <summary>
        ///     Should this <see cref="Behaviour" /> log whenever event functions are called?
        /// </summary>
        protected virtual bool LogEventFunctions => false;

        /// <summary>
        ///     Should this <see cref="Behaviour" /> call <see cref="Initialize" /> every time it is enabled?
        /// </summary>
        protected virtual bool ReInitializeOnEnable => false;

        /// <summary>
        ///     Should this <see cref="Behaviour" /> skip this frame's Update call?
        /// </summary>
        protected virtual bool ShouldSkipUpdate =>
            !FullyInitialized || !HasBeenEnabled || AppalachiaApplication.IsCompiling;

        /// <summary>
        ///     The number of frames since this <see cref="Behaviour" /> received its call to <see cref="Start" />.
        /// </summary>
        public int StartedDuration => enabled ? CoreClock.Instance.FrameCount - _startFrame : 0;

        /// <summary>
        ///     The frame on which this <see cref="Behaviour" /> received its call to <see cref="Start" />.
        /// </summary>
        public int StartFrame => _startFrame;

        #region Event Functions

        /// <summary>
        ///     Maps the Unity <see cref="Awake" /> method to the Appalachia <see cref="Initialize" /> method.
        /// </summary>
        protected void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                if ((this == null) || (gameObject == null))
                {
                    return;
                }

                if (LogEventFunctions)
                {
                    Context.Log.Info(nameof(Awake), this);
                }

                var cancellationToken = this.GetCancellationTokenOnDestroy();

                RemapUnityEvents(UnityEventFlags.Awake)
                   .AttachExternalCancellation(cancellationToken)
                   .Forget(LogRemapException);
            }
        }

        /// <summary>
        ///     Handles the Unity <see cref="OnDestroy" /> method but does not forward it.
        /// </summary>
        protected void Reset()
        {
            using (_PRF_Reset.Auto())
            {
                if ((this == null) || (gameObject == null))
                {
                    return;
                }

                if (LogEventFunctions)
                {
                    Context.Log.Info(nameof(Reset), this);
                }

                _renderingBounds = default;
                _cachedTransform = default;
                _cachedRectTransform = default;
                _hasCachedRectTransform = false;
                _hasCachedTransform = false;

                var cancellationToken = this.GetCancellationTokenOnDestroy();

                RemapUnityEvents(UnityEventFlags.Reset)
                   .AttachExternalCancellation(cancellationToken)
                   .Forget(LogRemapException);
            }
        }

        /// <summary>
        ///     Maps the Unity <see cref="Start" /> method to the Appalachia <see cref="WhenEnabled" /> method.
        /// </summary>
        protected void Start()
        {
            using (_PRF_Start.Auto())
            {
                if ((this == null) || (gameObject == null))
                {
                    return;
                }

                if (LogEventFunctions)
                {
                    Context.Log.Info(nameof(Start), this);
                }

                var cancellationToken = this.GetCancellationTokenOnDestroy();

                RemapUnityEvents(UnityEventFlags.Start)
                   .AttachExternalCancellation(cancellationToken)
                   .Forget(LogRemapException);
            }
        }

        /// <summary>
        ///     Maps the Unity <see cref="OnEnable" /> method to the Appalachia <see cref="WhenEnabled" /> method.
        /// </summary>
        protected void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                if ((this == null) || (gameObject == null))
                {
                    return;
                }

                if (LogEventFunctions)
                {
                    Context.Log.Info(nameof(OnEnable), this);
                }

                var cancellationToken = this.GetCancellationTokenOnDestroy();

                RemapUnityEvents(UnityEventFlags.OnEnable)
                   .AttachExternalCancellation(cancellationToken)
                   .Forget(LogRemapException);

#if UNITY_EDITOR
                if (!AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    var icon = GetGameObjectIcon();
                    var currentIcon = UnityEditor.EditorGUIUtility.GetIconForObject(gameObject);

                    if (currentIcon != icon)
                    {
                        UnityEditor.EditorGUIUtility.SetIconForObject(gameObject, icon);
                    }
                }
#endif
            }
        }

        /// <summary>
        ///     Maps the Unity <see cref="OnDisable" /> method to the Appalachia <see cref="WhenDisabled" /> method.
        /// </summary>
        protected void OnDisable()
        {
            using (_PRF_OnDisable.Auto())
            {
                if ((this == null) || (gameObject == null))
                {
                    return;
                }

                _onDisableFrame = CoreClock.Instance.FrameCount;

                if (LogEventFunctions)
                {
                    Context.Log.Info(nameof(OnDisable), this);
                }

                var cancellationToken = this.GetCancellationTokenOnDestroy();

                RemapUnityEvents(UnityEventFlags.OnDisable)
                   .AttachExternalCancellation(cancellationToken)
                   .Forget(LogRemapException);
            }
        }

        /// <summary>
        ///     Maps the Unity <see cref="OnDestroy" /> method to the Appalachia <see cref="WhenDestroyed" /> method.
        /// </summary>
        protected void OnDestroy()
        {
            using (_PRF_OnDestroy.Auto())
            {
                if ((this == null) || (gameObject == null))
                {
                    return;
                }

                if (LogEventFunctions)
                {
                    Context.Log.Info(nameof(OnDestroy), this);
                }

                var cancellationToken = this.GetCancellationTokenOnDestroy();

                RemapUnityEvents(UnityEventFlags.OnDestroy)
                   .AttachExternalCancellation(cancellationToken)
                   .Forget(LogRemapException);
            }
        }

        #endregion

        /// <summary>
        ///     Notify the <see cref="ObjectEnableEventRouter" /> that this <see cref="Behaviour" />
        ///     has been enabled, so that any subscriber can be notified in turn.
        /// </summary>
        protected abstract void NotifyWhenEnabled();

        /// <summary>
        ///     The <see cref="MonoBehaviour" /> Start method is invoked via Unity.
        /// </summary>
        /// <remarks>Should not be used unless absolutely necessary.</remarks>
        protected virtual void AwakeActual()
        {
        }

        /// <summary>
        ///     The <see cref="MonoBehaviour" /> OnDestroy method is invoked via Unity.
        /// </summary>
        /// <remarks>Should not be used unless absolutely necessary.</remarks>
        protected virtual void OnDestroyActual()
        {
        }

        /// <summary>
        ///     The <see cref="MonoBehaviour" /> OnDisable method is invoked via Unity.
        /// </summary>
        /// <remarks>Should not be used unless absolutely necessary.</remarks>
        protected virtual void OnDisableActual()
        {
        }

        /// <summary>
        ///     The <see cref="MonoBehaviour" /> OnEnable method is invoked via Unity.
        /// </summary>
        /// <remarks>Should not be used unless absolutely necessary.</remarks>
        protected virtual void OnEnableActual()
        {
        }

        /// <summary>
        ///     The <see cref="MonoBehaviour" /> Reset method is invoked via Unity.
        /// </summary>
        /// <remarks>Should not be used unless absolutely necessary.</remarks>
        protected virtual void ResetActual()
        {
        }

        /// <summary>
        ///     The <see cref="MonoBehaviour" /> Start method is invoked via Unity.
        /// </summary>
        /// <remarks>Should not be used unless absolutely necessary.</remarks>
        protected virtual void StartActual()
        {
        }

        /// <summary>
        ///     Runs:
        ///     <list type="bullet">
        ///         <item>When the <see cref="Behaviour" /> is destroyed.</item>
        ///         <item>When the <see cref="GameObject" />  the <see cref="Behaviour" />  belongs to is destroyed.</item>
        ///         /// <item>When the application is quitting.</item>
        ///     </list>
        /// </summary>
        protected virtual async AppaTask WhenDestroyed()
        {
            await AppaTask.CompletedTask;
        }

        /// <summary>
        ///     Runs:
        ///     <list type="bullet">
        ///         <item>Every time the <see cref="Behaviour" /> moves from enabled to disabled.</item>
        ///         <item>In the editor, just before the application is reloaded after a build..</item>
        ///     </list>
        ///     In the editor, "disabled" means that the checkbox
        ///     in the inspector window being false.  At runtime, it means that the
        ///     <see cref="Behaviour.enabled" /> value is false.
        /// </summary>
        protected virtual async AppaTask WhenDisabled()
        {
            await AppaTask.CompletedTask;
        }

        /// <summary>
        ///     Runs:
        ///     <list type="bullet">
        ///         <item>
        ///             Once after the <see cref="Behaviour" /> is initialized on application load.  This call is guaranteed to be after completion of the
        ///             <see cref="Initialize" /> call.
        ///         </item>
        ///         <item>Every time the <see cref="Behaviour" /> moves from disabled to enabled.</item>
        ///     </list>
        ///     In the editor, "enabled" means that the checkbox
        ///     in the inspector window being true.  At runtime, it means that the
        ///     <see cref="Behaviour.enabled" /> value is true.
        /// </summary>
        protected virtual async AppaTask WhenEnabled()
        {
            await AppaTask.CompletedTask;
        }

        private void LogRemapException(Exception ex)
        {
            using (_PRF_LogRemapException.Auto())
            {
                if (ex is OperationCanceledException)
                {
                    return;
                }

                var stackSplits = ex.StackTrace.Split('\n').Select(l => l.Trim()).ToArray();

                Context.Log.Error(stackSplits[0], this, ex);
            }
        }

        private async AppaTask RemapUnityEvents(UnityEventFlags currentUnityEventType)
        {
            if ((gameObject == null) || (this == null))
            {
                return;
            }

            try
            {
                await RemapUnityEventsInternal(currentUnityEventType);
            }
            catch (Exception ex)
            {
                Context.Log.Error(
                    ZString.Format(
                        "Error remapping {0} for {1} on {2}.",
                        currentUnityEventType.ToString().FormatMethodForLogging(),
                        GetType().FormatForLogging(),
                        this == null ? DELETED : name.FormatNameForLogging()
                    ),
                    this,
                    ex
                );

                throw;
            }
        }

        private async AppaTask RemapUnityEventsInternal(UnityEventFlags currentUnityEventType)
        {
            await AppalachiaRepositoryDependencyManager.ValidateDependencies(GetType());

            if ((this == null) || (gameObject == null))
            {
                return;
            }

            if (_unityEventFlags.HasFlag(UnityEventFlags.OnDestroy))
            {
                return;
            }

            _unityEventFlags |= currentUnityEventType;

            switch (currentUnityEventType)
            {
                case UnityEventFlags.Awake:

                    _awakeFrame = CoreClock.Instance.FrameCount;
                    _unityAwake = true;
                    try
                    {
                        AwakeActual();
                    }
                    catch (Exception ex)
                    {
                        Context.Log.Error(
                            ZString.Format(
                                "Error executing {0} for {1} on {2}.",
                                nameof(AwakeActual).FormatMethodForLogging(),
                                GetType().FormatForLogging(),
                                this == null ? DELETED : name.FormatNameForLogging()
                            ),
                            this,
                            ex
                        );

                        throw;
                    }

                    break;
                case UnityEventFlags.OnEnable:

                    _onEnableFrame = CoreClock.Instance.FrameCount;
                    _unityOnEnable = true;
                    try
                    {
                        OnEnableActual();
                    }
                    catch (Exception ex)
                    {
                        Context.Log.Error(
                            ZString.Format(
                                "Error executing {0} for {1} on {2}.",
                                nameof(OnEnableActual).FormatMethodForLogging(),
                                GetType().FormatForLogging(),
                                this == null ? DELETED : name.FormatNameForLogging()
                            ),
                            this,
                            ex
                        );

                        throw;
                    }

                    break;
                case UnityEventFlags.Start:

                    _startFrame = CoreClock.Instance.FrameCount;
                    _unityStart = true;
                    try
                    {
                        StartActual();
                    }
                    catch (Exception ex)
                    {
                        Context.Log.Error(
                            ZString.Format(
                                "Error executing {0} for {1} on {2}.",
                                nameof(StartActual).FormatMethodForLogging(),
                                GetType().FormatForLogging(),
                                this == null ? DELETED : name.FormatNameForLogging()
                            ),
                            this,
                            ex
                        );

                        throw;
                    }

                    break;
                case UnityEventFlags.Reset:

                    _resetFrame = CoreClock.Instance.FrameCount;
                    _unityReset = true;
                    try
                    {
                        ResetActual();
                    }
                    catch (Exception ex)
                    {
                        Context.Log.Error(
                            ZString.Format(
                                "Error executing {0} for {1} on {2}.",
                                nameof(ResetActual).FormatMethodForLogging(),
                                GetType().FormatForLogging(),
                                this == null ? DELETED : name.FormatNameForLogging()
                            ),
                            this,
                            ex
                        );

                        throw;
                    }

                    break;
                case UnityEventFlags.OnDisable:

                    _onDisableFrame = CoreClock.Instance.FrameCount;
                    _unityOnDisable = true;
                    try
                    {
                        OnDisableActual();
                    }
                    catch (Exception ex)
                    {
                        Context.Log.Error(
                            ZString.Format(
                                "Error executing {0} for {1} on {2}.",
                                nameof(OnDisableActual).FormatMethodForLogging(),
                                GetType().FormatForLogging(),
                                this == null ? DELETED : name.FormatNameForLogging()
                            ),
                            this,
                            ex
                        );

                        throw;
                    }

                    break;
                case UnityEventFlags.OnDestroy:

                    _unityOnDestroy = true;
                    try
                    {
                        OnDestroyActual();
                    }
                    catch (Exception ex)
                    {
                        Context.Log.Error(
                            ZString.Format(
                                "Error executing {0} for {1} on {2}.",
                                nameof(OnDestroyActual).FormatMethodForLogging(),
                                GetType().FormatForLogging(),
                                this == null ? DELETED : name.FormatNameForLogging()
                            ),
                            this,
                            ex
                        );

                        throw;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_unityEventFlags), _unityEventFlags, null);
            }

            if ((this == null) || (gameObject == null))
            {
                return;
            }

            if (currentUnityEventType == UnityEventFlags.OnDestroy)
            {
                _eventFlags = AppalachiaEventFlags.None;

                try
                {
                    await WhenDestroyed();
                }
                catch (Exception ex)
                {
                    Context.Log.Error(
                        ZString.Format(
                            "Error executing {0} for {1} on {2}.",
                            nameof(WhenDestroyed).FormatMethodForLogging(),
                            GetType().FormatForLogging(),
                            this == null ? DELETED : name.FormatNameForLogging()
                        ),
                        this,
                        ex
                    );

                    throw;
                }

                _hasBeenInitialized = false;

                return;
            }

            if (currentUnityEventType == UnityEventFlags.OnDisable)
            {
                _eventFlags = _eventFlags.UnsetFlag(AppalachiaEventFlags.Enabled);

                try
                {
                    await WhenDisabled();
                }
                catch (Exception ex)
                {
                    Context.Log.Error(
                        ZString.Format(
                            "Error executing {0} for {1} on {2}.",
                            nameof(WhenDisabled).FormatMethodForLogging(),
                            GetType().FormatForLogging(),
                            this == null ? DELETED : name.FormatNameForLogging()
                        ),
                        this,
                        ex
                    );

                    throw;
                }

                _hasBeenEnabled = false;
                _hasBeenDisabled = true;

                if (ReInitializeOnEnable)
                {
                    _eventFlags = _eventFlags.UnsetFlag(AppalachiaEventFlags.Initialized);
                }

                return;
            }

            if ((this == null) || (gameObject == null))
            {
                return;
            }

            if (!_eventFlags.Has(AppalachiaEventFlags.Initialized))
            {
                try
                {
                    await ExecuteInitialization();
                }
                catch (Exception ex)
                {
                    Context.Log.Error(
                        ZString.Format(
                            "Error executing {0} for {1} on {2}.",
                            nameof(ExecuteInitialization).FormatMethodForLogging(),
                            GetType().FormatForLogging(),
                            this == null ? DELETED : name.FormatNameForLogging()
                        ),
                        this,
                        ex
                    );

                    throw;
                }

                _hasBeenInitialized = true;
                _eventFlags |= AppalachiaEventFlags.Initialized;
                await AppaTask.Yield();
            }

            if ((this == null) || (gameObject == null))
            {
                return;
            }

            if ((currentUnityEventType == UnityEventFlags.OnEnable) && !_hasBeenEnabled)
            {
                _eventFlags = _eventFlags.SetFlag(AppalachiaEventFlags.Enabled);

                try
                {
                    await WhenEnabled();
                }
                catch (Exception ex)
                {
                    Context.Log.Error(
                        ZString.Format(
                            "Error executing {0} for {1} on {2}.",
                            nameof(WhenEnabled).FormatMethodForLogging(),
                            GetType().FormatForLogging(),
                            this == null ? DELETED : name.FormatNameForLogging()
                        ),
                        this,
                        ex
                    );

                    throw;
                }

                _hasBeenEnabled = true;
                _hasBeenDisabled = false;

                NotifyWhenEnabled();
            }
        }

        #region IEventDriven Members

        /// <summary>
        ///     Is this <see cref="Behaviour" /> disabled?
        /// </summary>
        public bool HasBeenDisabled => _hasBeenDisabled;

        /// <summary>
        ///     Is this <see cref="Behaviour" /> enabled?
        /// </summary>
        public bool HasBeenEnabled => _hasBeenEnabled;

        /// <summary>
        ///     Is this <see cref="Behaviour" /> initialized?
        /// </summary>
        public bool HasBeenInitialized => _hasBeenInitialized;

        public int AwakeDuration => enabled ? CoreClock.Instance.FrameCount - _awakeFrame : 0;

        /// <summary>
        ///     The frame on which this <see cref="Behaviour" /> received its call to <see cref="Start" />.
        /// </summary>
        public int AwakeFrame => _awakeFrame;

        public int DisabledDuration => enabled ? 0 : CoreClock.Instance.FrameCount - _onDisableFrame;
        public int EnabledDuration => enabled ? CoreClock.Instance.FrameCount - _onEnableFrame : 0;

        /// <summary>
        ///     The frame on which this <see cref="Behaviour" /> received its call to <see cref="Start" />.
        /// </summary>
        public int OnDisableFrame => _onDisableFrame;

        /// <summary>
        ///     The frame on which this <see cref="Behaviour" /> received its call to <see cref="Start" />.
        /// </summary>
        public int OnEnableFrame => _onEnableFrame;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_LogRemapException =
            new ProfilerMarker(_PRF_PFX + nameof(LogRemapException));

        private static readonly ProfilerMarker _PRF_Reset = new ProfilerMarker(_PRF_PFX + nameof(Reset));
        private static readonly ProfilerMarker _PRF_Awake = new ProfilerMarker(_PRF_PFX + nameof(Awake));

        private static readonly ProfilerMarker _PRF_Start = new ProfilerMarker(_PRF_PFX + nameof(Start));

        private static readonly ProfilerMarker
            _PRF_OnEnable = new ProfilerMarker(_PRF_PFX + nameof(OnEnable));

        private static readonly ProfilerMarker _PRF_OnDisable =
            new ProfilerMarker(_PRF_PFX + nameof(OnDisable));

        private static readonly ProfilerMarker _PRF_OnDestroy =
            new ProfilerMarker(_PRF_PFX + nameof(OnDestroy));

        #endregion
    }

    public partial class AppalachiaBehaviour<T>
    {
        /// <inheritdoc />
        protected override bool ShouldSkipUpdate => base.ShouldSkipUpdate || !DependenciesAreReady;

        /// <inheritdoc />
        protected override void NotifyWhenEnabled()
        {
            using (_PRF_NotifyWhenEnabled.Auto())
            {
                ObjectEnableEventRouter.Notify(GetType(), this as T, GuaranteedEventRouting);
            }
        }

        #region Profiling

        protected static readonly string _PRF_PFX8 = typeof(T).Name + ".";

        protected static readonly ProfilerMarker _PRF_NotifyWhenEnabled =
            new ProfilerMarker(_PRF_PFX8 + nameof(NotifyWhenEnabled));

        #endregion
    }

    public partial class SingletonAppalachiaBehaviour<T>
    {
    }

    public partial class AppalachiaSimpleBase
    {
    }

    public partial class AppalachiaBase
    {
        #region Static Fields and Autoproperties

        private static Queue<Func<AppaTask>> _initializationFunctions;

        #endregion

        #region Fields and Autoproperties

        [NonSerialized] private bool _hasBeenInitialized;
        [NonSerialized] private bool _hasBeenEnabled;
        [NonSerialized] private bool _hasBeenDisabled;

        #endregion

        public static Queue<Func<AppaTask>> InitializationFunctions => _initializationFunctions;

        protected virtual bool LogEventFunctions => false;
        public bool HasBeenDisabled => _hasBeenDisabled;
        public bool HasBeenEnabled => _hasBeenEnabled;
        public bool HasBeenInitialized => _hasBeenInitialized;

        /// <summary>
        ///     Runs after the <see cref="Initialize" /> call:
        ///     <list type="bullet">
        ///         <item>When the object is created for the first time (similar to a constructor).</item>
        ///         <item>Every time the object is deserialized.</item>
        ///     </list>
        /// </summary>
        protected virtual async AppaTask WhenEnabled()
        {
            await AppaTask.CompletedTask;
        }

        private async AppaTask HandleInitialization()
        {
            await AppaTask.NextFrame();
            await AppalachiaRepositoryDependencyManager.ValidateDependencies(GetType());

            await ExecuteInitialization();

            _hasBeenInitialized = true;

            await WhenEnabled();

            _hasBeenEnabled = true;
            _hasBeenDisabled = false;
        }
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

    public partial class AppalachiaSelectable<T> : IEventDriven
    {
        #region Static Fields and Autoproperties

        private static string _deleted;

        #endregion

        #region Fields and Autoproperties

        [NonSerialized] private int _awakeFrame;
        [NonSerialized] private int _startFrame;
        [NonSerialized] private int _resetFrame;
        [NonSerialized] private int _onEnableFrame;
        [NonSerialized] private int _onDisableFrame;

        [NonSerialized] private AppalachiaEventFlags _eventFlags;
        [NonSerialized] private UnityEventFlags _unityEventFlags;

        [NonSerialized] private bool _unityAwake;
        [NonSerialized] private bool _unityOnEnable;
        [NonSerialized] private bool _unityStart;
        [NonSerialized] private bool _unityReset;
        [NonSerialized] private bool _unityOnDisable;
        [NonSerialized] private bool _unityOnDestroy;

        [NonSerialized] private bool _hasBeenInitialized;
        [NonSerialized] private bool _hasBeenEnabled;
        [NonSerialized] private bool _hasBeenDisabled;

        #endregion

        protected static string DELETED
        {
            get
            {
                if (_deleted == null)
                {
                    _deleted = "[DELETED]".Bold().FormatNameForLogging();
                }

                return _deleted;
            }
        }

        protected virtual bool GuaranteeEnableEventRouting => false;

        protected virtual bool LogEventFunctions => false;

        protected virtual bool ShouldSkipUpdate =>
            !FullyInitialized ||
            !DependenciesAreReady ||
            !HasBeenEnabled ||
            AppalachiaApplication.IsCompiling;

        public int StartedDuration => enabled ? CoreClock.Instance.FrameCount - _startFrame : 0;
        public int StartFrame => _startFrame;

        #region Event Functions

        /// <inheritdoc />
        protected override void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                base.Awake();

                if ((this == null) || (gameObject == null))
                {
                    return;
                }

                if (LogEventFunctions)
                {
                    Context.Log.Info(nameof(Awake), this);
                }

                var cancellationToken = this.GetCancellationTokenOnDestroy();

                RemapUnityEvents(UnityEventFlags.Awake)
                   .AttachExternalCancellation(cancellationToken)
                   .Forget(LogRemapException);
            }
        }

        /// <inheritdoc />
        protected override void Reset()
        {
            using (_PRF_Reset.Auto())
            {
                base.Reset();

                if ((this == null) || (gameObject == null))
                {
                    return;
                }

                if (LogEventFunctions)
                {
                    Context.Log.Info(nameof(Reset), this);
                }

                _rectTransform = null;

#if UNITY_EDITOR
                transition = Transition.None;
#endif

                var cancellationToken = this.GetCancellationTokenOnDestroy();

                RemapUnityEvents(UnityEventFlags.Reset)
                   .AttachExternalCancellation(cancellationToken)
                   .Forget(LogRemapException);
            }
        }

        /// <inheritdoc />
        protected override void Start()
        {
            using (_PRF_Start.Auto())
            {
                base.Start();

                if ((this == null) || (gameObject == null))
                {
                    return;
                }

                if (LogEventFunctions)
                {
                    Context.Log.Info(nameof(Start), this);
                }

                var cancellationToken = this.GetCancellationTokenOnDestroy();

                RemapUnityEvents(UnityEventFlags.Start)
                   .AttachExternalCancellation(cancellationToken)
                   .Forget(LogRemapException);
            }
        }

        /// <inheritdoc />
        protected override void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                base.OnEnable();

                if ((this == null) || (gameObject == null))
                {
                    return;
                }

                if (LogEventFunctions)
                {
                    Context.Log.Info(nameof(OnEnable), this);
                }

                var cancellationToken = this.GetCancellationTokenOnDestroy();

                RemapUnityEvents(UnityEventFlags.OnEnable)
                   .AttachExternalCancellation(cancellationToken)
                   .Forget(LogRemapException);

#if UNITY_EDITOR
                if (!AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    var iconName = GetGameObjectIcon();
                    var icon = AssetDatabaseManager.FindFirstAssetMatch<Texture2D>(iconName);
                    UnityEditor.EditorGUIUtility.SetIconForObject(gameObject, icon);
                }
#endif
            }
        }

        /// <inheritdoc />
        protected override void OnDisable()
        {
            using (_PRF_OnDisable.Auto())
            {
                base.OnDisable();

                if ((this == null) || (gameObject == null))
                {
                    return;
                }

                _onDisableFrame = CoreClock.Instance.FrameCount;

                if (LogEventFunctions)
                {
                    Context.Log.Info(nameof(OnDisable), this);
                }

                var cancellationToken = this.GetCancellationTokenOnDestroy();

                RemapUnityEvents(UnityEventFlags.OnDisable)
                   .AttachExternalCancellation(cancellationToken)
                   .Forget(LogRemapException);
            }
        }

        /// <inheritdoc />
        protected override void OnDestroy()
        {
            using (_PRF_OnDestroy.Auto())
            {
                base.OnDestroy();

                if ((this == null) || (gameObject == null))
                {
                    return;
                }

                if (LogEventFunctions)
                {
                    Context.Log.Info(nameof(OnDestroy), this);
                }

                var cancellationToken = this.GetCancellationTokenOnDestroy();

                RemapUnityEvents(UnityEventFlags.OnDestroy)
                   .AttachExternalCancellation(cancellationToken)
                   .Forget(LogRemapException);
            }
        }

        #endregion

        protected virtual void AwakeActual()
        {
        }

        protected virtual void OnDestroyActual()
        {
        }

        protected virtual void OnDisableActual()
        {
        }

        protected virtual void OnEnableActual()
        {
        }

        protected virtual void ResetActual()
        {
        }

        protected virtual void StartActual()
        {
        }

        protected virtual async AppaTask WhenDestroyed()
        {
            await AppaTask.CompletedTask;
        }

        protected virtual async AppaTask WhenDisabled()
        {
            await AppaTask.CompletedTask;
        }

        protected virtual async AppaTask WhenEnabled()
        {
            await AppaTask.CompletedTask;
        }

        private void LogRemapException(Exception ex)
        {
            using (_PRF_LogRemapException.Auto())
            {
                if (ex is OperationCanceledException)
                {
                    return;
                }

                var stackSplits = ex.StackTrace.Split('\n').Select(l => l.Trim()).ToArray();

                Context.Log.Error(stackSplits[0], this, ex);
            }
        }

        private async AppaTask RemapUnityEvents(UnityEventFlags currentUnityEventType)
        {
            if ((gameObject == null) || (this == null))
            {
                return;
            }

            try
            {
                await RemapUnityEventsInternal(currentUnityEventType);
            }
            catch (Exception ex)
            {
                Context.Log.Error(
                    ZString.Format(
                        "Error remapping {0} for {1} on {2}.",
                        currentUnityEventType.ToString().FormatMethodForLogging(),
                        GetType().FormatForLogging(),
                        this == null ? DELETED : name.FormatNameForLogging()
                    ),
                    this,
                    ex
                );

                throw;
            }
        }

        private async AppaTask RemapUnityEventsInternal(UnityEventFlags currentUnityEventType)
        {
            await AppalachiaRepositoryDependencyManager.ValidateDependencies(GetType());

            if ((this == null) || (gameObject == null))
            {
                return;
            }

            if (_unityEventFlags.HasFlag(UnityEventFlags.OnDestroy))
            {
                return;
            }

            _unityEventFlags |= currentUnityEventType;

            switch (currentUnityEventType)
            {
                case UnityEventFlags.Awake:

                    _awakeFrame = CoreClock.Instance.FrameCount;
                    _unityAwake = true;
                    try
                    {
                        AwakeActual();
                    }
                    catch (Exception ex)
                    {
                        Context.Log.Error(
                            ZString.Format(
                                "Error executing {0} for {1} on {2}.",
                                nameof(AwakeActual).FormatMethodForLogging(),
                                GetType().FormatForLogging(),
                                this == null ? DELETED : name.FormatNameForLogging()
                            ),
                            this,
                            ex
                        );

                        throw;
                    }

                    break;
                case UnityEventFlags.OnEnable:

                    _onEnableFrame = CoreClock.Instance.FrameCount;
                    _unityOnEnable = true;
                    try
                    {
                        OnEnableActual();
                    }
                    catch (Exception ex)
                    {
                        Context.Log.Error(
                            ZString.Format(
                                "Error executing {0} for {1} on {2}.",
                                nameof(OnEnableActual).FormatMethodForLogging(),
                                GetType().FormatForLogging(),
                                this == null ? DELETED : name.FormatNameForLogging()
                            ),
                            this,
                            ex
                        );

                        throw;
                    }

                    break;
                case UnityEventFlags.Start:

                    _startFrame = CoreClock.Instance.FrameCount;
                    _unityStart = true;
                    try
                    {
                        StartActual();
                    }
                    catch (Exception ex)
                    {
                        Context.Log.Error(
                            ZString.Format(
                                "Error executing {0} for {1} on {2}.",
                                nameof(StartActual).FormatMethodForLogging(),
                                GetType().FormatForLogging(),
                                this == null ? DELETED : name.FormatNameForLogging()
                            ),
                            this,
                            ex
                        );

                        throw;
                    }

                    break;
                case UnityEventFlags.Reset:

                    _resetFrame = CoreClock.Instance.FrameCount;
                    _unityReset = true;
                    try
                    {
                        ResetActual();
                    }
                    catch (Exception ex)
                    {
                        Context.Log.Error(
                            ZString.Format(
                                "Error executing {0} for {1} on {2}.",
                                nameof(ResetActual).FormatMethodForLogging(),
                                GetType().FormatForLogging(),
                                this == null ? DELETED : name.FormatNameForLogging()
                            ),
                            this,
                            ex
                        );

                        throw;
                    }

                    break;
                case UnityEventFlags.OnDisable:

                    _onDisableFrame = CoreClock.Instance.FrameCount;
                    _unityOnDisable = true;
                    try
                    {
                        OnDisableActual();
                    }
                    catch (Exception ex)
                    {
                        Context.Log.Error(
                            ZString.Format(
                                "Error executing {0} for {1} on {2}.",
                                nameof(OnDisableActual).FormatMethodForLogging(),
                                GetType().FormatForLogging(),
                                this == null ? DELETED : name.FormatNameForLogging()
                            ),
                            this,
                            ex
                        );

                        throw;
                    }

                    break;
                case UnityEventFlags.OnDestroy:

                    _unityOnDestroy = true;
                    try
                    {
                        OnDestroyActual();
                    }
                    catch (Exception ex)
                    {
                        Context.Log.Error(
                            ZString.Format(
                                "Error executing {0} for {1} on {2}.",
                                nameof(OnDestroyActual).FormatMethodForLogging(),
                                GetType().FormatForLogging(),
                                this == null ? DELETED : name.FormatNameForLogging()
                            ),
                            this,
                            ex
                        );

                        throw;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_unityEventFlags), _unityEventFlags, null);
            }

            if ((this == null) || (gameObject == null))
            {
                return;
            }

            if (currentUnityEventType == UnityEventFlags.OnDestroy)
            {
                _eventFlags = AppalachiaEventFlags.None;

                try
                {
                    await WhenDestroyed();
                }
                catch (Exception ex)
                {
                    Context.Log.Error(
                        ZString.Format(
                            "Error executing {0} for {1} on {2}.",
                            nameof(WhenDestroyed).FormatMethodForLogging(),
                            GetType().FormatForLogging(),
                            this == null ? DELETED : name.FormatNameForLogging()
                        ),
                        this,
                        ex
                    );

                    throw;
                }

                _hasBeenInitialized = false;

                return;
            }

            if (currentUnityEventType == UnityEventFlags.OnDisable)
            {
                _eventFlags = _eventFlags.UnsetFlag(AppalachiaEventFlags.Enabled);

                try
                {
                    await WhenDisabled();
                }
                catch (Exception ex)
                {
                    Context.Log.Error(
                        ZString.Format(
                            "Error executing {0} for {1} on {2}.",
                            nameof(WhenDisabled).FormatMethodForLogging(),
                            GetType().FormatForLogging(),
                            this == null ? DELETED : name.FormatNameForLogging()
                        ),
                        this,
                        ex
                    );

                    throw;
                }

                _hasBeenEnabled = false;
                _hasBeenDisabled = true;

                return;
            }

            if ((this == null) || (gameObject == null))
            {
                return;
            }

            if (!_eventFlags.Has(AppalachiaEventFlags.Initialized))
            {
                try
                {
                    await ExecuteInitialization();
                }
                catch (Exception ex)
                {
                    Context.Log.Error(
                        ZString.Format(
                            "Error executing {0} for {1} on {2}.",
                            nameof(ExecuteInitialization).FormatMethodForLogging(),
                            GetType().FormatForLogging(),
                            this == null ? DELETED : name.FormatNameForLogging()
                        ),
                        this,
                        ex
                    );

                    throw;
                }

                _hasBeenInitialized = true;
                _eventFlags |= AppalachiaEventFlags.Initialized;
                await AppaTask.Yield();
            }

            if ((this == null) || (gameObject == null))
            {
                return;
            }

            if ((currentUnityEventType == UnityEventFlags.OnEnable) && !_hasBeenEnabled)
            {
                _eventFlags = _eventFlags.SetFlag(AppalachiaEventFlags.Enabled);

                try
                {
                    await WhenEnabled();
                }
                catch (Exception ex)
                {
                    Context.Log.Error(
                        ZString.Format(
                            "Error executing {0} for {1} on {2}.",
                            nameof(WhenEnabled).FormatMethodForLogging(),
                            GetType().FormatForLogging(),
                            this == null ? DELETED : name.FormatNameForLogging()
                        ),
                        this,
                        ex
                    );

                    throw;
                }

                _hasBeenEnabled = true;
                _hasBeenDisabled = false;

                ObjectEnableEventRouter.Notify(GetType(), this, GuaranteeEnableEventRouting);
            }
        }

        #region IEventDriven Members

        public bool HasBeenDisabled => _hasBeenDisabled;
        public bool HasBeenEnabled => _hasBeenEnabled;

        public bool HasBeenInitialized => _hasBeenInitialized;

        public int AwakeDuration => enabled ? CoreClock.Instance.FrameCount - _awakeFrame : 0;
        public int AwakeFrame => _awakeFrame;
        public int DisabledDuration => enabled ? 0 : CoreClock.Instance.FrameCount - _onDisableFrame;
        public int EnabledDuration => enabled ? CoreClock.Instance.FrameCount - _onEnableFrame : 0;
        public int OnDisableFrame => _onDisableFrame;
        public int OnEnableFrame => _onEnableFrame;

        #endregion

        #region Profiling

        protected static readonly string _PRF_PFX4 = typeof(T).Name + ".";
        protected static readonly ProfilerMarker _PRF_Awake = new ProfilerMarker(_PRF_PFX4 + nameof(Awake));

        protected static readonly ProfilerMarker _PRF_LogRemapException =
            new ProfilerMarker(_PRF_PFX4 + nameof(LogRemapException));

        protected static readonly ProfilerMarker _PRF_OnDestroy =
            new ProfilerMarker(_PRF_PFX4 + nameof(OnDestroy));

        protected static readonly ProfilerMarker _PRF_OnDisable =
            new ProfilerMarker(_PRF_PFX4 + nameof(OnDisable));

        protected static readonly ProfilerMarker _PRF_OnEnable =
            new ProfilerMarker(_PRF_PFX4 + nameof(OnEnable));

        protected static readonly ProfilerMarker _PRF_Reset = new ProfilerMarker(_PRF_PFX4 + nameof(Reset));

        protected static readonly ProfilerMarker _PRF_Start = new ProfilerMarker(_PRF_PFX4 + nameof(Start));

        protected static readonly ProfilerMarker _PRF_WhenDisabled =
            new ProfilerMarker(_PRF_PFX4 + nameof(WhenDisabled));

        protected static readonly ProfilerMarker _PRF_WhenEnabled =
            new ProfilerMarker(_PRF_PFX4 + nameof(WhenEnabled));

        #endregion
    }
}
#pragma warning restore CS0414
