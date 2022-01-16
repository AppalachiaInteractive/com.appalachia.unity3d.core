using System;
using System.Collections.Generic;
using System.Linq;
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Objects.Dependencies;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Enums;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Strings;
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

    public partial class AppalachiaObject
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
        public bool HasBeenDisabled => _hasBeenDisabled;
        public bool HasBeenEnabled => _hasBeenEnabled;

        public bool HasBeenInitialized => _hasBeenInitialized;

        public int AwakeDuration => Time.frameCount - _awakeFrame;
        public int AwakeFrame => _awakeFrame;
        public int DisabledDuration => Time.frameCount - _onDisableFrame;
        public int EnabledDuration => Time.frameCount - _onEnableFrame;
        public int OnDisableFrame => _onDisableFrame;
        public int OnEnableFrame => _onEnableFrame;

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
                _onEnableFrame = Time.frameCount;

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
                _onDisableFrame = Time.frameCount;

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
                        currentUnityEventType,
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

                    _awakeFrame = Time.frameCount;
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

                    _onEnableFrame = Time.frameCount;
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

                    _resetFrame = Time.frameCount;
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

                    _onDisableFrame = Time.frameCount;
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

    public partial class AppalachiaBehaviour
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

        protected virtual bool LogEventFunctions => false;

        protected virtual bool ShouldSkipUpdate => !FullyInitialized || AppalachiaApplication.IsCompiling;
        public bool HasBeenDisabled => _hasBeenDisabled;
        public bool HasBeenEnabled => _hasBeenEnabled;

        public bool HasBeenInitialized => _hasBeenInitialized;

        public int AwakeDuration => enabled ? Time.frameCount - _awakeFrame : 0;
        public int AwakeFrame => _awakeFrame;
        public int DisabledDuration => enabled ? 0 : Time.frameCount - _onDisableFrame;
        public int EnabledDuration => enabled ? Time.frameCount - _onEnableFrame : 0;
        public int OnDisableFrame => _onDisableFrame;
        public int OnEnableFrame => _onEnableFrame;
        public int StartedDuration => enabled ? Time.frameCount - _startFrame : 0;
        public int StartFrame => _startFrame;

        #region Event Functions

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

                ___renderingBounds = default;
                ___transform = default;

                var cancellationToken = this.GetCancellationTokenOnDestroy();

                RemapUnityEvents(UnityEventFlags.Reset)
                   .AttachExternalCancellation(cancellationToken)
                   .Forget(LogRemapException);
            }
        }

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
                    var iconName = GetGameObjectIcon();
                    var icon = AssetDatabaseManager.FindFirstAssetMatch<Texture2D>(iconName);
                    UnityEditor.EditorGUIUtility.SetIconForObject(gameObject, icon);
                }
#endif
            }
        }

        protected void OnDisable()
        {
            using (_PRF_OnDisable.Auto())
            {
                if ((this == null) || (gameObject == null))
                {
                    return;
                }

                _onDisableFrame = Time.frameCount;

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
                        currentUnityEventType,
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

                    _awakeFrame = Time.frameCount;
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

                    _onEnableFrame = Time.frameCount;
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

                    _startFrame = Time.frameCount;
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

                    _resetFrame = Time.frameCount;
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

                    _onDisableFrame = Time.frameCount;
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
            }
        }

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
        protected override bool ShouldSkipUpdate => base.ShouldSkipUpdate || !DependenciesAreReady;
    }

    public partial class SingletonAppalachiaBehaviour<T>
    {
    }

    public partial class AppalachiaSimpleBase
    {
    }

    public partial class AppalachiaBase : ISerializationCallbackReceiver
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

        protected virtual async AppaTask WhenEnabled()
        {
            await AppaTask.CompletedTask;
        }

        private async AppaTask HandleInitialization()
        {
            await AppaTask.NextFrame();
            await AppalachiaRepositoryDependencyManager.ValidateDependencies(GetType());

            BeforeInitialization();

            await Initialize(_initializer);

            _hasBeenInitialized = true;

            AfterInitialization();

            initializationState.hasInitializationFinished = true;

            await WhenEnabled();

            _hasBeenEnabled = true;
            _hasBeenDisabled = false;
        }

        #region ISerializationCallbackReceiver Members

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            if (_hasBeenInitialized)
            {
                return;
            }

            _initializationFunctions ??= new Queue<Func<AppaTask>>();
            _initializationFunctions.Enqueue(HandleInitialization);
        }

        #endregion
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

    public partial class AppalachiaSelectable<T>
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

        protected virtual bool LogEventFunctions => false;

        protected virtual bool ShouldSkipUpdate =>
            !FullyInitialized || !DependenciesAreReady || AppalachiaApplication.IsCompiling;

        public bool HasBeenDisabled => _hasBeenDisabled;
        public bool HasBeenEnabled => _hasBeenEnabled;

        public bool HasBeenInitialized => _hasBeenInitialized;

        public int AwakeDuration => enabled ? Time.frameCount - _awakeFrame : 0;
        public int AwakeFrame => _awakeFrame;
        public int DisabledDuration => enabled ? 0 : Time.frameCount - _onDisableFrame;
        public int EnabledDuration => enabled ? Time.frameCount - _onEnableFrame : 0;
        public int OnDisableFrame => _onDisableFrame;
        public int OnEnableFrame => _onEnableFrame;
        public int StartedDuration => enabled ? Time.frameCount - _startFrame : 0;
        public int StartFrame => _startFrame;

        #region Event Functions

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

        protected override void OnDisable()
        {
            using (_PRF_OnDisable.Auto())
            {
                base.OnDisable();

                if ((this == null) || (gameObject == null))
                {
                    return;
                }

                _onDisableFrame = Time.frameCount;

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
                        currentUnityEventType,
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

                    _awakeFrame = Time.frameCount;
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

                    _onEnableFrame = Time.frameCount;
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

                    _startFrame = Time.frameCount;
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

                    _resetFrame = Time.frameCount;
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

                    _onDisableFrame = Time.frameCount;
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
            }
        }

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
