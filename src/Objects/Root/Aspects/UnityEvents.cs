using System;
using System.Collections.Generic;
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Objects.Dependencies;
using Appalachia.Utility.Async;
using Appalachia.Utility.Enums;
using Appalachia.Utility.Execution;
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

                RemapUnityEvents(UnityEventFlags.Awake).Forget();
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

                RemapUnityEvents(UnityEventFlags.Reset).Forget();
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

                RemapUnityEvents(UnityEventFlags.OnEnable).Forget();
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

                RemapUnityEvents(UnityEventFlags.OnDisable).Forget();
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

                RemapUnityEvents(UnityEventFlags.OnDestroy).Forget();
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

        private async AppaTask RemapUnityEvents(UnityEventFlags currentUnityEventType)
        {
            using (_PRF_RemapUnityEvents.Auto())
            {
                await AppalachiaRepositoryDependencyManager.ValidateDependencies();

                _unityEventFlags |= currentUnityEventType;

                switch (currentUnityEventType)
                {
                    case UnityEventFlags.Awake:

                        _awakeFrame = Time.frameCount;
                        _unityAwake = true;
                        AwakeActual();

                        break;
                    case UnityEventFlags.OnEnable:

                        _onEnableFrame = Time.frameCount;
                        _unityOnEnable = true;
                        OnEnableActual();

                        break;
                    case UnityEventFlags.Reset:

                        _resetFrame = Time.frameCount;
                        _unityReset = true;
                        ResetActual();

                        break;
                    case UnityEventFlags.OnDisable:

                        _onDisableFrame = Time.frameCount;
                        _unityOnDisable = true;
                        OnDisableActual();

                        break;
                    case UnityEventFlags.OnDestroy:

                        _unityOnDestroy = true;
                        OnDestroyActual();

                        break;
                    default:
                        throw new ArgumentOutOfRangeException(
                            nameof(_unityEventFlags),
                            _unityEventFlags,
                            null
                        );
                }

                if (currentUnityEventType == UnityEventFlags.OnDestroy)
                {
                    _eventFlags = AppalachiaEventFlags.None;
                    await WhenDestroyed();

                    _hasBeenInitialized = false;
                    return;
                }

                if (currentUnityEventType == UnityEventFlags.OnDisable)
                {
                    _eventFlags = _eventFlags.UnsetFlag(AppalachiaEventFlags.Enabled);
                    await WhenDisabled();

                    _hasBeenEnabled = false;
                    _hasBeenDisabled = true;

                    return;
                }

                if (!_eventFlags.Has(AppalachiaEventFlags.Initialized))
                {
                    await ExecuteInitialization();

                    _eventFlags |= AppalachiaEventFlags.Initialized;
                }

                if (currentUnityEventType == UnityEventFlags.OnEnable)
                {
                    _eventFlags = _eventFlags.SetFlag(AppalachiaEventFlags.Enabled);
                    await WhenEnabled();

                    _hasBeenEnabled = true;
                    _hasBeenDisabled = false;
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_RemapUnityEvents =
            new ProfilerMarker(_PRF_PFX + nameof(RemapUnityEvents));

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

        protected virtual bool LogEventFunctions => false;
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
                if (LogEventFunctions)
                {
                    Context.Log.Info(nameof(Awake), this);
                }

                RemapUnityEvents(UnityEventFlags.Awake).Forget();
            }
        }

        protected void Reset()
        {
            using (_PRF_Reset.Auto())
            {
                if (LogEventFunctions)
                {
                    Context.Log.Info(nameof(Reset), this);
                }

                ___renderingBounds = default;
                ___transform = default;

                RemapUnityEvents(UnityEventFlags.Reset).Forget();
            }
        }

        protected void Start()
        {
            using (_PRF_Start.Auto())
            {
                if (LogEventFunctions)
                {
                    Context.Log.Info(nameof(Start), this);
                }

                RemapUnityEvents(UnityEventFlags.Start).Forget();
            }
        }

        protected void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                if (LogEventFunctions)
                {
                    Context.Log.Info(nameof(OnEnable), this);
                }

                RemapUnityEvents(UnityEventFlags.OnEnable).Forget();

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
                _onDisableFrame = Time.frameCount;

                if (LogEventFunctions)
                {
                    Context.Log.Info(nameof(OnDisable), this);
                }

                RemapUnityEvents(UnityEventFlags.OnDisable).Forget();
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

                RemapUnityEvents(UnityEventFlags.OnDestroy).Forget();
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

        private protected virtual async AppaTask Initialize()
        {
            BeforeInitialization();

            await AppaTask.CompletedTask;

            AfterInitialization();
        }

        private async AppaTask RemapUnityEvents(UnityEventFlags currentUnityEventType)
        {
            using (_PRF_RemapUnityEvents.Auto())
            {
                await AppalachiaRepositoryDependencyManager.ValidateDependencies();

                _unityEventFlags |= currentUnityEventType;

                switch (currentUnityEventType)
                {
                    case UnityEventFlags.Awake:

                        _awakeFrame = Time.frameCount;
                        _unityAwake = true;
                        AwakeActual();

                        break;
                    case UnityEventFlags.OnEnable:

                        _onEnableFrame = Time.frameCount;
                        _unityOnEnable = true;
                        OnEnableActual();

                        break;
                    case UnityEventFlags.Start:

                        _startFrame = Time.frameCount;
                        _unityStart = true;
                        StartActual();

                        break;
                    case UnityEventFlags.Reset:

                        _resetFrame = Time.frameCount;
                        _unityReset = true;
                        ResetActual();

                        break;
                    case UnityEventFlags.OnDisable:

                        _onDisableFrame = Time.frameCount;
                        _unityOnDisable = true;
                        OnDisableActual();

                        break;
                    case UnityEventFlags.OnDestroy:

                        _unityOnDestroy = true;
                        OnDestroyActual();

                        break;
                    default:
                        throw new ArgumentOutOfRangeException(
                            nameof(_unityEventFlags),
                            _unityEventFlags,
                            null
                        );
                }

                if (currentUnityEventType == UnityEventFlags.OnDestroy)
                {
                    _eventFlags = AppalachiaEventFlags.None;

                    await WhenDestroyed();

                    _hasBeenInitialized = false;

                    return;
                }

                if (currentUnityEventType == UnityEventFlags.OnDisable)
                {
                    _eventFlags = _eventFlags.UnsetFlag(AppalachiaEventFlags.Enabled);

                    await WhenDisabled();

                    _hasBeenEnabled = false;
                    _hasBeenDisabled = true;

                    return;
                }

                if (!_eventFlags.Has(AppalachiaEventFlags.Initialized))
                {
                    await Initialize();

                    _hasBeenInitialized = true;
                    _eventFlags |= AppalachiaEventFlags.Initialized;
                }

                if (currentUnityEventType == UnityEventFlags.OnEnable)
                {
                    _eventFlags = _eventFlags.SetFlag(AppalachiaEventFlags.Enabled);
                    await WhenEnabled();

                    _hasBeenEnabled = true;
                    _hasBeenDisabled = false;
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_RemapUnityEvents =
            new ProfilerMarker(_PRF_PFX + nameof(RemapUnityEvents));

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
    }

    public partial class SingletonAppalachiaBehaviour<T>
    {
    }

    public partial class AppalachiaSimpleBase
    {
    }

    public partial class AppalachiaBase : ISerializationCallbackReceiver
    {
        #region Fields and Autoproperties

        [NonSerialized] private bool _hasBeenInitialized;
        [NonSerialized] private bool _hasBeenEnabled;
        [NonSerialized] private bool _hasBeenDisabled;

        #endregion

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
            using (_PRF_HandleInitialization.Auto())
            {
                await AppaTask.NextFrame();
                await AppalachiaRepositoryDependencyManager.ValidateDependencies();

                BeforeInitialization();

                await Initialize(_initializer);

                _hasBeenInitialized = true;

                AfterInitialization();

                await WhenEnabled();

                _hasBeenEnabled = true;
                _hasBeenDisabled = false;
            }
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

        private static Queue<Func<AppaTask>> _initializationFunctions;
        public static Queue<Func<AppaTask>> InitializationFunctions => _initializationFunctions;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_HandleInitialization =
            new ProfilerMarker(_PRF_PFX + nameof(HandleInitialization));

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
}
#pragma warning restore CS0414
