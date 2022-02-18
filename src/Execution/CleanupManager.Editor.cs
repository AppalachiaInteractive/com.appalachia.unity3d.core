#if UNITY_EDITOR
using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Execution
{
    [ExecuteAlways]
    public sealed partial class CleanupManager
    {
        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                UnityEditor.Compilation.CompilationPipeline.compilationStarted +=
                    OnCompilationPipelineStarted;
            }
        }

        private void OnCompilationPipelineStarted(object _)
        {
            using (_PRF_OnCompilationPipelineStarted.Auto())
            {
                Dispose();
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_OnCompilationPipelineStarted =
            new ProfilerMarker(_PRF_PFX + nameof(OnCompilationPipelineStarted));

        #endregion
    }
}

#endif
