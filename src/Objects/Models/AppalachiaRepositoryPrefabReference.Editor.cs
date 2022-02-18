#if UNITY_EDITOR
using Unity.Profiling;

namespace Appalachia.Core.Objects.Models
{
    public sealed partial class AppalachiaRepositoryPrefabReference
    {
        /// <inheritdoc />
        protected override bool _showAssetRefDisplayValue => prefab == null;

        /// <inheritdoc />
        protected override string GetReferenceName()
        {
            using (_PRF_GetReferenceName.Auto())
            {
                if (prefab == null)
                {
                    return _prefabAddress;
                }

                return prefab.name;
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetReferenceName =
            new ProfilerMarker(_PRF_PFX + nameof(GetReferenceName));

        #endregion
    }
}

#endif
