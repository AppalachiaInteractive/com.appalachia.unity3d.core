using Appalachia.Utility.Extensions;
using Unity.Profiling;

namespace Appalachia.Core.Objects.Root
{
    public static class Extensions
    {
        public static void MarkAsModified(this AppalachiaBase target)
        {
            using (_PRF_MarkAsModified.Auto())
            {
#if UNITY_EDITOR
                if (target.Owner == null)
                {
                    return;
                }
                
                target.Owner.MarkAsModified();

                if (Modifications.CanModifyAssets())
                {
                    target.Owner.MarkAsModified();
                }
#endif
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(Extensions) + ".";

        private static readonly ProfilerMarker _PRF_MarkAsModified =
            new ProfilerMarker(_PRF_PFX + nameof(MarkAsModified));

        #endregion
    }
}
