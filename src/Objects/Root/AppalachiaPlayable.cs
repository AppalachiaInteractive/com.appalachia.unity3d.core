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
