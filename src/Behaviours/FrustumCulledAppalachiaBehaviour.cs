using Unity.Profiling;

namespace Appalachia.Core.Behaviours
{
    public abstract class FrustumCulledAppalachiaBehaviour<T> : AppalachiaBehaviour
        where T : FrustumCulledAppalachiaBehaviour<T>
    {
        private const string _PRF_PFX = nameof(FrustumCulledAppalachiaBehaviour<T>) + ".";

        private static readonly ProfilerMarker _PRF_OnBecameVisible = new(_PRF_PFX + nameof(OnBecameVisible));

        private static readonly ProfilerMarker _PRF_OnBecameInvisible =
            new(_PRF_PFX + nameof(OnBecameInvisible));

        protected virtual void BeforeInvisible()
        {
        }

        protected virtual void BeforeVisible()
        {
        }

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
    }
}