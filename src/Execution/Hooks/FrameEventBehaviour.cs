#region

using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Core.Execution.Hooks
{
    [ExecuteAlways]
    public abstract class FrameEventBehaviour<T> : SingletonAppalachiaBehaviour<FrameEventBehaviour<T>>
        where T : FrameEventBehaviour<T>
    {
        #region Static Fields and Autoproperties

        private static readonly ProfilerMarker _PRF_Awake = new(_PRF_PFX + nameof(Awake));
        private static readonly ProfilerMarker _PRF_Start = new(_PRF_PFX + nameof(Start));
        private static readonly ProfilerMarker _PRF_OnEnable = new(_PRF_PFX + nameof(OnEnable));
        private static readonly ProfilerMarker _PRF_OnDisable = new(_PRF_PFX + nameof(OnDisable));
        private static readonly ProfilerMarker _PRF_Update = new(_PRF_PFX + nameof(Update));
        private static readonly ProfilerMarker _PRF_FixedUpdate = new(_PRF_PFX + nameof(FixedUpdate));

        private static readonly ProfilerMarker _PRF_OnApplicationQuit =
            new(_PRF_PFX + nameof(OnApplicationQuit));

        private static readonly ProfilerMarker _PRF_OnDestroy = new(_PRF_PFX + nameof(OnDestroy));
        private static FrameEventDelegates<T> _eventDelegates;

        #endregion

        #region Fields and Autoproperties

        private bool _onEnableQueued;

        #endregion

        public static FrameEventDelegates<T> EventDelegates
        {
            get
            {
                if (_eventDelegates == null)
                {
                    _eventDelegates = new FrameEventDelegates<T>();
                }

                return _eventDelegates;
            }
        }

        #region Event Functions

        protected virtual void Update()
        {
            using (_PRF_Update.Auto())
            {
#if UNITY_EDITOR
                if (FrameEventSettings._ENABLE_DISABLE.v)
                {
                    Context.Log.Info(ZString.Format("[{0}]: [Update]", GetReadableName()));
                }
#endif

                if (_onEnableQueued)
                {
                    _onEnableQueued = false;

                    EventDelegates.InvokeOnEnable();
                }

                EventDelegates.InvokeUpdate();
            }
        }

        protected virtual void FixedUpdate()
        {
            using (_PRF_FixedUpdate.Auto())
            {
#if UNITY_EDITOR
                if (FrameEventSettings._ENABLE_DISABLE.v)
                {
                    Context.Log.Info(ZString.Format("[{0}]: [FixedUpdate]", GetReadableName()));
                }
#endif

                EventDelegates.InvokeFixedUpdate();
            }
        }

        protected override async AppaTask WhenDisabled()
        {
            {
                await base.WhenDisabled();

#if UNITY_EDITOR
                if (FrameEventSettings._ENABLE_DISABLE.v)
                {
                    Context.Log.Info(ZString.Format("[{0}]: [OnDisable]", GetReadableName()));
                }
#endif

                EventDelegates.InvokeOnDisable();
            }
        }

        protected override async AppaTask WhenDestroyed()
        {
            using (_PRF_OnDestroy.Auto())
            {
                await base.WhenDestroyed();

#if UNITY_EDITOR
                if (FrameEventSettings._ENABLE_DESTROY.v)
                {
                    Context.Log.Info(ZString.Format("[{0}]: [OnDestroy]", GetReadableName()));
                }
#endif

                EventDelegates.InvokeOnDestroy();
            }
        }

        protected virtual void OnApplicationQuit()
        {
            using (_PRF_OnApplicationQuit.Auto())
            {
#if UNITY_EDITOR
                if (FrameEventSettings._ENABLE_QUIT.v)
                {
                    Context.Log.Info(ZString.Format("[{0}]: [OnApplicationQuit]", GetReadableName()));
                }
#endif

                EventDelegates.InvokeOnApplicationQuit();
            }
        }

        protected virtual void OnPreCull()
        {
            using (_PRF_FixedUpdate.Auto())
            {
#if UNITY_EDITOR
                if (FrameEventSettings._ENABLE_DISABLE.v)
                {
                    Context.Log.Info(ZString.Format("[{0}]: [OnPreCull]", GetReadableName()));
                }
#endif

                EventDelegates.InvokeOnPreCull(Camera.current);
            }
        }

        #endregion

        protected abstract string GetReadableName();

        protected override void AwakeActual()
        {
            using (_PRF_Awake.Auto())
            {
                base.AwakeActual();

#if UNITY_EDITOR
                if (FrameEventSettings._ENABLE_AWAKE.v)
                {
                    Context.Log.Info(ZString.Format("[{0}]: [Awake]", GetReadableName()));
                }
#endif

                EventDelegates.InvokeAwake();
            }
        }

        protected override void OnEnableActual()
        {
            using (_PRF_OnEnable.Auto())
            {
                base.OnEnableActual();

                StaticApplicationState.HasOnEnableExecuted = true;

#if UNITY_EDITOR
                if (FrameEventSettings._ENABLE_ENABLE.v)
                {
                    Context.Log.Info(ZString.Format("[{0}]: [OnEnable]", GetReadableName()));
                }
#endif

                _onEnableQueued = true;
            }
        }

        protected override void StartActual()
        {
            using (_PRF_Start.Auto())
            {
                base.StartActual();

#if UNITY_EDITOR
                if (FrameEventSettings._ENABLE_START.v)
                {
                    Context.Log.Info(ZString.Format("[{0}]: [Start]", GetReadableName()));
                }
#endif

                EventDelegates.InvokeStart();
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(FrameEventBehaviour<T>) + ".";

        #endregion

        // ReSharper disable once StaticMemberInGenericType
    }
}
