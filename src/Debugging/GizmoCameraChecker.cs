using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Debugging
{
    public static class GizmoCameraChecker
    {
        private const string _PRF_PFX = nameof(GizmoCameraChecker) + ".";

        private static Camera _mainCamera;
        private static Camera _sceneViewCamera;

        private static readonly ProfilerMarker _PRF_ShouldRenderGizmos =
            new(_PRF_PFX + nameof(ShouldRenderGizmos));

        public static bool ShouldRenderGizmos()
        {
            using (_PRF_ShouldRenderGizmos.Auto())
            {
                if (_mainCamera == null)
                {
                    _mainCamera = Camera.main;
                }

#if UNITY_EDITOR

                if (_sceneViewCamera == null)
                {
                    var sceneviewCameras = UnityEditor.SceneView.GetAllSceneCameras();
                    _sceneViewCamera = sceneviewCameras.Length > 0 ? sceneviewCameras[0] : null;
                }
#endif

                var current = Camera.current;

                if ((current != _mainCamera) && (current != _sceneViewCamera))
                {
                    return false;
                }

                return true;
            }
        }
    }
}
