using System;
using System.Linq;
using Appalachia.Core.Objects.Dependencies;
using Appalachia.Core.Objects.Root.Contracts;
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

        [NonSerialized] private bool _isBeingDestroyed;
        [NonSerialized] private bool _isBeingDisabled;
        [NonSerialized] private bool _isBeingEnabled;
        [NonSerialized] private bool _isBeingInitialized;

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
            !FullyInitialized ||
            !HasBeenEnabled ||
            AppalachiaApplication.IsCompiling ||
            AppalachiaApplication.IsQuitting;

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
        ///     A chance to finalize the enable process.
        /// </summary>
        protected virtual async AppaTask AfterEnabled()
        {
            await AppaTask.CompletedTask;
        }

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
                    _isBeingDestroyed = true;
                    await WhenDestroyed();
                }
                catch (Exception ex)
                {
                    _isBeingDestroyed = false;

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

                _isBeingDestroyed = false;
                _hasBeenInitialized = false;

                return;
            }

            if (currentUnityEventType == UnityEventFlags.OnDisable)
            {
                _eventFlags = _eventFlags.UnsetFlag(AppalachiaEventFlags.Enabled);

                try
                {
                    _isBeingDisabled = true;
                    await WhenDisabled();
                }
                catch (Exception ex)
                {
                    _isBeingDisabled = false;
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

                _isBeingDisabled = false;
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
                    _isBeingInitialized = true;
                    await ExecuteInitialization();
                }
                catch (Exception ex)
                {
                    _isBeingInitialized = false;

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

                _isBeingInitialized = false;
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
                    _isBeingEnabled = true;
                    await WhenEnabled();
                    await AfterEnabled();
                }
                catch (Exception ex)
                {
                    _isBeingEnabled = false;
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

                _isBeingEnabled = false;
                _hasBeenEnabled = true;
                _hasBeenDisabled = false;

                RaiseNotificationWhenEnabled();
            }
        }

        #region IEventDriven Members

        /// <summary>
        ///     Is this <see cref="Behaviour" /> being disabled presently?
        /// </summary>
        public bool IsBeingDisabled => _isBeingDisabled;

        /// <summary>
        ///     Is this <see cref="Behaviour" /> disabled?
        /// </summary>
        public bool HasBeenDisabled => _hasBeenDisabled;

        /// <summary>
        ///     Is this <see cref="Behaviour" /> being enabled presently?
        /// </summary>
        public bool IsBeingEnabled => _isBeingEnabled;

        /// <summary>
        ///     Is this <see cref="Behaviour" /> enabled?
        /// </summary>
        public bool HasBeenEnabled => _hasBeenEnabled;

        /// <summary>
        ///     Is this <see cref="Behaviour" /> being initialized presently?
        /// </summary>
        public bool IsBeingInitialized => _isBeingInitialized;

        /// <summary>
        ///     Is this <see cref="Behaviour" /> initialized?
        /// </summary>
        public bool HasBeenInitialized => _hasBeenInitialized;

        /// <summary>
        ///     Is this <see cref="Behaviour" /> being disabled presently, or has it been disabled previously?
        /// </summary>
        public bool HasBeenOrIsBeingDisabled => HasBeenDisabled || IsBeingDisabled;

        /// <summary>
        ///     Is this <see cref="Behaviour" /> being initialized presently, or has it been initialized previously?
        /// </summary>
        public bool HasBeenOrIsBeingInitialized => HasBeenInitialized || IsBeingInitialized;

        /// <summary>
        ///     Is this <see cref="Behaviour" /> being enabled presently, or has it been enabled previously?
        /// </summary>
        public bool HasBeenOrIsBeingEnabled => HasBeenEnabled || IsBeingEnabled;

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
}
#pragma warning restore CS0414
