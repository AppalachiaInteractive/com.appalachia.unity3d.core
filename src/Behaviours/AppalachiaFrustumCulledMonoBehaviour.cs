using Unity.Profiling;

namespace Appalachia.Core.Behaviours
{
    public class AppalachiaFrustumCulledMonoBehaviour<T> : AppalachiaMonoBehaviour
        where T : AppalachiaFrustumCulledMonoBehaviour<T>
    {
        private const string _PRF_PFX = nameof(AppalachiaFrustumCulledMonoBehaviour<T>) + ".";

        private static readonly ProfilerMarker _PRF_OnBecameVisible =
            new(_PRF_PFX + nameof(OnBecameVisible));

        private static readonly ProfilerMarker _PRF_OnBecameInvisible =
            new(_PRF_PFX + nameof(OnBecameInvisible));

        private void OnBecameInvisible()
        {
            using (_PRF_OnBecameInvisible.Auto())
            {
                BeforeInvisible();
                enabled = false;
            }
        }

        private void OnBecameVisible()
        {
            using (_PRF_OnBecameVisible.Auto())
            {
                BeforeVisible();
                enabled = true;
            }
        }

        protected virtual void BeforeVisible()
        {
        }

        protected virtual void BeforeInvisible()
        {
        }
    }
}
