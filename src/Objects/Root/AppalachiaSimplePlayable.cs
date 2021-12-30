using Appalachia.Utility.Async;
using Unity.Profiling;
using UnityEngine.Playables;

namespace Appalachia.Core.Objects.Root
{
    public abstract partial class AppalachiaSimplePlayable : PlayableBehaviour
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

        public override void OnPlayableCreate(Playable playable)
        {
            using (_PRF_OnPlayableCreate.Auto())
            {
                Context.Log.Debug(nameof(OnPlayableCreate), this);
                ExecuteInitialization(playable).Forget();
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

        protected abstract void OnPause(Playable playable, FrameData info);

        protected abstract void OnPlay(Playable playable, FrameData info);

        protected abstract bool ShouldRun(PlayableEvent playableEvent);

        protected abstract void WhenDestroyed(Playable playable);

        protected virtual async AppaTask InitializePlayable(Playable playable)
        {
            await AppaTask.CompletedTask;
        }

        private async AppaTask ExecuteInitialization(Playable playable)
        {
            using (_PRF_ExecuteInitialization.Auto())
            {
                BeforeInitialization();

                await Initialize(_initializer);

                await InitializePlayable(playable);

                AfterInitialization();
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AppalachiaSimplePlayable) + ".";

        private static readonly ProfilerMarker _PRF_OnPlayableCreate =
            new ProfilerMarker(_PRF_PFX + nameof(OnPlayableCreate));

        private static readonly ProfilerMarker _PRF_OnPlayableDestroy =
            new ProfilerMarker(_PRF_PFX + nameof(OnPlayableDestroy));

        private static readonly ProfilerMarker _PRF_ExecuteInitialization =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteInitialization));

        private static readonly ProfilerMarker _PRF_OnBehaviourPause =
            new ProfilerMarker(_PRF_PFX + nameof(OnBehaviourPause));

        private static readonly ProfilerMarker _PRF_OnBehaviourPlay =
            new ProfilerMarker(_PRF_PFX + nameof(OnBehaviourPlay));

        #endregion
    }
}
