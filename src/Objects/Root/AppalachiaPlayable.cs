using System;
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

    public abstract partial class AppalachiaPlayable : AppalachiaSimplePlayable
    {
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

        protected abstract void Update(Playable playable, FrameData info, object playerData);

        protected abstract void WhenStarted(Playable playable);

        protected abstract void WhenStopped(Playable playable);

        #region Profiling

        private const string _PRF_PFX = nameof(AppalachiaPlayable) + ".";

        private static readonly ProfilerMarker _PRF_OnGraphStop =
            new ProfilerMarker(_PRF_PFX + nameof(OnGraphStop));

        private static readonly ProfilerMarker _PRF_OnGraphStart =
            new ProfilerMarker(_PRF_PFX + nameof(OnGraphStart));

        private static readonly ProfilerMarker _PRF_ProcessFrame =
            new ProfilerMarker(_PRF_PFX + nameof(ProcessFrame));

        #endregion
    }

    public abstract partial class AppalachiaPlayable<T> : AppalachiaPlayable
        where T : AppalachiaPlayable<T>
    {
        #region Constants and Static Readonly

        protected static readonly string _PRF_PFX = typeof(T).Name + ".";

        protected static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        protected static readonly ProfilerMarker _PRF_InitializePlayable =
            new ProfilerMarker(_PRF_PFX + nameof(InitializePlayable));

        protected static readonly ProfilerMarker _PRF_OnBehaviourPause =
            new ProfilerMarker(_PRF_PFX + nameof(OnBehaviourPause));

        protected static readonly ProfilerMarker _PRF_OnBehaviourPlay =
            new ProfilerMarker(_PRF_PFX + nameof(OnBehaviourPlay));

        protected static readonly ProfilerMarker _PRF_OnGraphStart =
            new ProfilerMarker(_PRF_PFX + nameof(OnGraphStart));

        protected static readonly ProfilerMarker _PRF_OnGraphStop =
            new ProfilerMarker(_PRF_PFX + nameof(OnGraphStop));

        protected static readonly ProfilerMarker _PRF_OnPause =
            new ProfilerMarker(_PRF_PFX + nameof(OnPause));

        protected static readonly ProfilerMarker _PRF_OnPlay = new ProfilerMarker(_PRF_PFX + nameof(OnPlay));

        protected static readonly ProfilerMarker _PRF_OnPlayableCreate =
            new ProfilerMarker(_PRF_PFX + nameof(OnPlayableCreate));

        protected static readonly ProfilerMarker _PRF_OnPlayableDestroy =
            new ProfilerMarker(_PRF_PFX + nameof(OnPlayableDestroy));

        protected static readonly ProfilerMarker _PRF_ProcessFrame =
            new ProfilerMarker(_PRF_PFX + nameof(ProcessFrame));

        protected static readonly ProfilerMarker _PRF_ShouldRun =
            new ProfilerMarker(_PRF_PFX + nameof(ShouldRun));

        protected static readonly ProfilerMarker _PRF_Update = new ProfilerMarker(_PRF_PFX + nameof(Update));

        protected static readonly ProfilerMarker _PRF_WhenDestroyed =
            new ProfilerMarker(_PRF_PFX + nameof(WhenDestroyed));

        protected static readonly ProfilerMarker _PRF_WhenStarted =
            new ProfilerMarker(_PRF_PFX + nameof(WhenStarted));

        protected static readonly ProfilerMarker _PRF_WhenStopped =
            new ProfilerMarker(_PRF_PFX + nameof(WhenStopped));

        #endregion

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

        #endregion
    }
}
