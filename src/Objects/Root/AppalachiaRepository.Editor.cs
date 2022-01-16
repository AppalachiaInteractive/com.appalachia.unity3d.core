#if UNITY_EDITOR

#region

using Appalachia.Utility.Execution;
using Sirenix.OdinInspector;
using Unity.Profiling;

#endregion

namespace Appalachia.Core.Objects.Root
{
    public sealed partial class AppalachiaRepository
    {
        #region Constants and Static Readonly

        private const int REPO_BOTTOM = 1000;
        private const int REPO_BOTTOM_CLEAR = REPO_BOTTOM + 20;
        private const int REPO_BOTTOM_SORT = REPO_BOTTOM + 10;
        private const int REPO_TOP = -1000;
        private const int REPO_TOP_CLEAR = REPO_TOP + 20;
        private const int REPO_TOP_SORT = REPO_TOP + 10;

        #endregion

        [ButtonGroup(nameof(REPO_TOP),    Order = REPO_TOP)]
        [ButtonGroup(nameof(REPO_BOTTOM), Order = REPO_BOTTOM)]
        public void Update()
        {
            using (_PRF_Update.Auto())
            {
                UpdateSingletons();
                UpdatePrefabs();
            }
        }

        private static void StaticInitializerInEditor()
        {
            InstanceAvailable += i => i.InitializeInEditor();
        }

        [ButtonGroup(nameof(REPO_TOP),    Order = REPO_TOP_CLEAR)]
        [ButtonGroup(nameof(REPO_BOTTOM), Order = REPO_BOTTOM_CLEAR)]
        private void Clear()
        {
            using (_PRF_Clear.Auto())
            {
                ClearSingletons();
                ClearPrefabs();

                MarkAsModified();
            }
        }

        private void InitializeInEditor()
        {
            using (_PRF_InitializeInEditor.Auto())
            {
                if (!AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    Update();
                }
            }
        }

        [ButtonGroup(nameof(REPO_TOP),    Order = REPO_TOP_SORT)]
        [ButtonGroup(nameof(REPO_BOTTOM), Order = REPO_BOTTOM_SORT)]
        private void Sort()
        {
            using (_PRF_Sort.Auto())
            {
                SortSingletons();
                SortPrefabs();

                MarkAsModified();
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_InitializeInEditor =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeInEditor));

        private static readonly ProfilerMarker _PRF_Update = new ProfilerMarker(_PRF_PFX + nameof(Update));
        private static readonly ProfilerMarker _PRF_Sort = new ProfilerMarker(_PRF_PFX + nameof(Sort));
        private static readonly ProfilerMarker _PRF_Clear = new ProfilerMarker(_PRF_PFX + nameof(Clear));

        #endregion
    }
}

#endif
