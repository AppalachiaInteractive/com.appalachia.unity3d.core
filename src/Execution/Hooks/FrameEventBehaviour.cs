#region

using Appalachia.Core.Behaviours;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Core.Execution.Hooks
{
    [ExecuteAlways]
    public abstract class FrameEventBehaviour<T> : SingletonMonoBehaviour<FrameEventBehaviour<T>>
        where T : FrameEventBehaviour<T>
    {
#region Profiling

        private const string _PRF_PFX = nameof(FrameEventBehaviour<T>) + ".";

#endregion

        // ReSharper disable once StaticMemberInGenericType
        private static readonly ProfilerMarker _PRF_Awake = new(_PRF_PFX + nameof(Awake));
        private static readonly ProfilerMarker _PRF_Start = new(_PRF_PFX + nameof(Start));
        private static readonly ProfilerMarker _PRF_OnEnable = new(_PRF_PFX + nameof(OnEnable));
        private static readonly ProfilerMarker _PRF_OnDisable = new(_PRF_PFX + nameof(OnDisable));
        private static readonly ProfilerMarker _PRF_Update = new(_PRF_PFX + nameof(Update));

        private static readonly ProfilerMarker _PRF_FixedUpdate =
            new(_PRF_PFX + nameof(FixedUpdate));

        private static readonly ProfilerMarker _PRF_OnApplicationQuit =
            new(_PRF_PFX + nameof(OnApplicationQuit));

        private static readonly ProfilerMarker _PRF_OnDestroy = new(_PRF_PFX + nameof(OnDestroy));
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
                if (FrameEventSettings._ENABLE_AWAKE.v)
                {
                    Debug.Log($"[{GetReadableName()}]: [Awake]");
                }

                EventDelegates.InvokeAwake();
            }
        }

        protected virtual void Start()
        {
            using (_PRF_Start.Auto())
            {
                if (FrameEventSettings._ENABLE_START.v)
                {
                    Debug.Log($"[{GetReadableName()}]: [Start]");
                }

                EventDelegates.InvokeStart();
            }
        }

        protected virtual void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (FrameEventSettings._ENABLE_DISABLE.v)
                {
                    Debug.Log($"[{GetReadableName()}]: [Update]");
                }

                EventDelegates.InvokeUpdate();
            }
        }

        protected virtual void FixedUpdate()
        {
            using (_PRF_FixedUpdate.Auto())
            {
                if (FrameEventSettings._ENABLE_DISABLE.v)
                {
                    Debug.Log($"[{GetReadableName()}]: [FixedUpdate]");
                }

                EventDelegates.InvokeFixedUpdate();
            }
        }

        protected virtual void OnPreCull()
        {
            using (_PRF_FixedUpdate.Auto())
            {
                if (FrameEventSettings._ENABLE_DISABLE.v)
                {
                    Debug.Log($"[{GetReadableName()}]: [OnPreCull]");
                }

                EventDelegates.InvokeOnPreCull(Camera.current);
            }
        }

        protected virtual void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                StaticApplicationState.HasOnEnableExecuted = true;

                if (FrameEventSettings._ENABLE_ENABLE.v)
                {
                    Debug.Log($"[{GetReadableName()}]: [OnEnable]");
                }

                EventDelegates.InvokeOnEnable();
            }
        }

        protected virtual void OnDisable()
        {
            using (_PRF_OnDisable.Auto())
            {
                if (FrameEventSettings._ENABLE_DISABLE.v)
                {
                    Debug.Log($"[{GetReadableName()}]: [OnDisable]");
                }

                EventDelegates.InvokeOnDisable();
            }
        }

        protected virtual void OnDestroy()
        {
            using (_PRF_OnDestroy.Auto())
            {
                if (FrameEventSettings._ENABLE_DESTROY.v)
                {
                    Debug.Log($"[{GetReadableName()}]: [OnDestroy]");
                }

                EventDelegates.InvokeOnDestroy();
            }
        }

        protected virtual void OnApplicationQuit()
        {
            using (_PRF_OnApplicationQuit.Auto())
            {
                if (FrameEventSettings._ENABLE_QUIT.v)
                {
                    Debug.Log($"[{GetReadableName()}]: [OnApplicationQuit]");
                }

                EventDelegates.InvokeOnApplicationQuit();
            }
        }

        protected abstract string GetReadableName();
    }
}
