using System.Linq;
using Unity.Profiling;

namespace Appalachia.Core.Objects.Root
{
    public sealed partial class AppalachiaRepository
    {
        public string[] GetPrefabAddresses()
        {
            using (_PRF_GetPrefabAddresses.Auto())
            {
                return _prefabs.Select(p => p.PrefabAddress).ToArray();
            }
        }

        #region Nested type: PrefabAddresses

        public static class PrefabAddresses
        {
            #region Constants and Static Readonly

            public const string WIND_ARROW = "WIND_ARROW";

            #endregion
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetPrefabAddresses =
            new ProfilerMarker(_PRF_PFX + nameof(GetPrefabAddresses));

        #endregion
    }
}
