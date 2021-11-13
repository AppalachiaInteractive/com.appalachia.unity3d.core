#region

using Appalachia.Core.Behaviours;
using Appalachia.Utility.Logging;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Core.Execution.Hooks
{
    [ExecuteAlways]
    public abstract class FrameEventBehaviour<T> : SingletonAppalachiaBehaviour<FrameEventBehaviour<T>>
        where T : FrameEventBehaviour<T>
    {
        #region Profiling And Tracing Markers

        private const string _PRF_PFX = nameof(FrameEventBehaviour<T>) + ".";

        // ReSharper disable once StaticMemberInGenericType
        private static readonly ProfilerMarker _PRF_Awake = new(_PRF_PFX + nameof(Awake));

        private static readonly ProfilerMarker _PRF_Start = new(_PRF_PFX + nameof(Start));
        private static readonly ProfilerMarker _PRF_OnEnable = new(_PRF_PFX + nameof(OnEnable));
        private static readonly ProfilerMarker _PRF_OnDisable = new(_PRF_PFX + nameof(OnDisable));
        private static readonly ProfilerMarker _PRF_Update = new(_PRF_PFX + nameof(Update));
        private static readonly ProfilerMarker _PRF_FixedUpdate = new(_PRF_PFX + nameof(FixedUpdate));

        private static readonly ProfilerMarker _PRF_OnApplicationQuit =
            new(_PRF_PFX + nameof(OnApplicationQuit));

        private static readonly ProfilerMarker _PRF_OnDestroy = new(_PRF_PFX + nameof(OnDestroy));

        #endregion
        
        private static FrameEventDelegates<T> _eventDelegates;

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

        protected virtual void Awake()
        {
            using (_PRF_Awake.Auto())
            {
#if UNITY_EDITOR
                if (FrameEventSettings._ENABLE_AWAKE.v)
                {
                    AppaLog.Info($"[{GetReadableName()}]: [Awake]");
                }
#endif

                EventDelegates.InvokeAwake();
            }
        }

        protected virtual void Start()
        {
            using (_PRF_Start.Auto())
            {
#if UNITY_EDITOR
                if (FrameEventSettings._ENABLE_START.v)
                {
                    AppaLog.Info($"[{GetReadableName()}]: [Start]");
                }
#endif

                EventDelegates.InvokeStart();
            }
        }

        protected virtual void Update()
        {
            using (_PRF_Update.Auto())
            {
#if UNITY_EDITOR
                if (FrameEventSettings._ENABLE_DISABLE.v)
                {
                    AppaLog.Info($"[{GetReadableName()}]: [Update]");
                }
#endif

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
                    AppaLog.Info($"[{GetReadableName()}]: [FixedUpdate]");
                }
#endif

                EventDelegates.InvokeFixedUpdate();
            }
        }

        protected virtual void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                StaticApplicationState.HasOnEnableExecuted = true;

#if UNITY_EDITOR
                if (FrameEventSettings._ENABLE_ENABLE.v)
                {
                    AppaLog.Info($"[{GetReadableName()}]: [OnEnable]");
                }
#endif

                EventDelegates.InvokeOnEnable();
            }
        }

        protected virtual void OnDisable()
        {
            using (_PRF_OnDisable.Auto())
            {
#if UNITY_EDITOR
                if (FrameEventSettings._ENABLE_DISABLE.v)
                {
                    AppaLog.Info($"[{GetReadableName()}]: [OnDisable]");
                }
#endif

                EventDelegates.InvokeOnDisable();
            }
        }

        protected virtual void OnDestroy()
        {
            using (_PRF_OnDestroy.Auto())
            {
#if UNITY_EDITOR
                if (FrameEventSettings._ENABLE_DESTROY.v)
                {
                    AppaLog.Info($"[{GetReadableName()}]: [OnDestroy]");
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
                    AppaLog.Info($"[{GetReadableName()}]: [OnApplicationQuit]");
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
                    AppaLog.Info($"[{GetReadableName()}]: [OnPreCull]");
                }
#endif

                EventDelegates.InvokeOnPreCull(Camera.current);
            }
        }

        protected abstract string GetReadableName();
    }
}
