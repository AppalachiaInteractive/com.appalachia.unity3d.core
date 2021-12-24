using System;
using Appalachia.Utility.Async;
using Unity.Profiling;
using UnityEngine.Playables;

// ReSharper disable StaticMemberInGenericType
// ReSharper disable UnusedParameter.Global

namespace Appalachia.Core.Objects.Root
{
    public enum PlayableEvent
    {
        OnBehaviourPause = 0,
        OnBehaviourPlay = 10,
        OnGraphStart = 20,
        OnGraphStop = 30,
        ProcessFrame = 60,
    }

    public abstract partial class AppalachiaPlayable : PlayableBehaviour
    {
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            using (_PRF_OnBehaviourPause.Auto())
            {
                Context.Log.Debug(nameof(OnBehaviourPause), this);

                if (!ShouldRun(PlayableEvent.OnBehaviourPause))
                {
                    return;
                }

                OnPause(playable, info);
            }
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            using (_PRF_OnBehaviourPlay.Auto())
            {
                Context.Log.Debug(nameof(OnBehaviourPlay), this);

                if (!ShouldRun(PlayableEvent.OnBehaviourPlay))
                {
                    return;
                }

                OnPlay(playable, info);
            }
        }

        public override void OnGraphStart(Playable playable)
        {
            using (_PRF_OnGraphStart.Auto())
            {
                Context.Log.Debug(nameof(OnGraphStart), this);

                if (!ShouldRun(PlayableEvent.OnGraphStart))
                {
                    return;
                }

                WhenStarted(playable);
            }
        }

        public override void OnGraphStop(Playable playable)
        {
            using (_PRF_OnGraphStop.Auto())
            {
                Context.Log.Debug(nameof(OnGraphStop), this);

                if (!ShouldRun(PlayableEvent.OnGraphStop))
                {
                    return;
                }

                WhenStopped(playable);
            }
        }

        public override void OnPlayableCreate(Playable playable)
        {
            using (_PRF_OnPlayableCreate.Auto())
            {
                Context.Log.Debug(nameof(OnPlayableCreate), this);
                ExecuteInitialization().Forget();
            }
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            using (_PRF_OnPlayableDestroy.Auto())
            {
                Context.Log.Debug(nameof(OnPlayableDestroy), this);
                WhenDestroyed(playable);
            }
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            using (_PRF_ProcessFrame.Auto())
            {
                if (!ShouldRun(PlayableEvent.ProcessFrame))
                {
                    return;
                }

                Context.Log.Debug(nameof(ProcessFrame), this);

                Update(playable, info, playerData);
            }
        }

        protected abstract void OnPause(Playable playable, FrameData info);

        protected abstract void OnPlay(Playable playable, FrameData info);

        protected abstract bool ShouldRun(PlayableEvent playableEvent);

        protected abstract void Update(Playable playable, FrameData info, object playerData);

        protected abstract void WhenDestroyed(Playable playable);

        protected abstract void WhenStarted(Playable playable);

        protected abstract void WhenStopped(Playable playable);

        private async AppaTask ExecuteInitialization()
        {
            using (_PRF_ExecuteInitialization.Auto())
            {
                BeforeInitialization();

                await Initialize(_initializer);

                AfterInitialization();
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AppalachiaPlayable) + ".";

        private static readonly ProfilerMarker _PRF_ExecuteInitialization =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteInitialization));

        private static readonly ProfilerMarker _PRF_OnGraphStop =
            new ProfilerMarker(_PRF_PFX + nameof(OnGraphStop));

        private static readonly ProfilerMarker _PRF_OnGraphStart =
            new ProfilerMarker(_PRF_PFX + nameof(OnGraphStart));

        private static readonly ProfilerMarker _PRF_OnPlayableCreate =
            new ProfilerMarker(_PRF_PFX + nameof(OnPlayableCreate));

        private static readonly ProfilerMarker _PRF_OnPlayableDestroy =
            new ProfilerMarker(_PRF_PFX + nameof(OnPlayableDestroy));

        private static readonly ProfilerMarker _PRF_OnBehaviourPlay =
            new ProfilerMarker(_PRF_PFX + nameof(OnBehaviourPlay));

        private static readonly ProfilerMarker _PRF_OnBehaviourPause =
            new ProfilerMarker(_PRF_PFX + nameof(OnBehaviourPause));

        private static readonly ProfilerMarker _PRF_ProcessFrame =
            new ProfilerMarker(_PRF_PFX + nameof(ProcessFrame));

        #endregion
    }

    public abstract partial class AppalachiaPlayable<T> : AppalachiaPlayable
        where T : AppalachiaPlayable<T>
    {
        protected override bool ShouldRun(PlayableEvent playableEvent)
        {
            switch (playableEvent)
            {
                case PlayableEvent.OnBehaviourPause:
                case PlayableEvent.OnBehaviourPlay:
                case PlayableEvent.OnGraphStart:
                case PlayableEvent.OnGraphStop:
                case PlayableEvent.ProcessFrame:
                    return DependenciesAreReady;
                default:
                    throw new ArgumentOutOfRangeException(nameof(playableEvent), playableEvent, null);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AppalachiaPlayable<T>) + ".";

        #endregion
    }
}
