using System;
using System.Linq;
using Appalachia.Core.Objects.Dependencies;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Enums;
using Appalachia.Utility.Strings;
using Appalachia.Utility.Timing;
using Unity.Profiling;
using UnityEngine;

// ReSharper disable NotAccessedField.Local
// ReSharper disable StaticMemberInGenericType
#pragma warning disable CS0414

namespace Appalachia.Core.Objects.Root
{
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

        [NonSerialized] private bool _isBeingDestroyed;
        [NonSerialized] private bool _isBeingDisabled;
        [NonSerialized] private bool _isBeingEnabled;
        [NonSerialized] private bool _isBeingInitialized;

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

                _eventFlags |= AppalachiaEventFlags.Initialized;
                _isBeingInitialized = false;
                _hasBeenInitialized = true;
                await AppaTask.Yield();
            }

            if (currentUnityEventType == UnityEventFlags.OnEnable)
            {
                _eventFlags = _eventFlags.SetFlag(AppalachiaEventFlags.Enabled);

                try
                {
                    _isBeingEnabled = true;
                    await WhenEnabled();
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

                _hasBeenEnabled = true;
                _hasBeenDisabled = false;
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
}
#pragma warning restore CS0414
