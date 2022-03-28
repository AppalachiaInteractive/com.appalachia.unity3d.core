using Appalachia.CI.Constants;
using Appalachia.Utility.Strings;
using Unity.Profiling;

namespace Appalachia.Core.ControlModel
{
    public class AppaControlNamer
    {
        public static string GetElementPrefix(int i)
        {
            using (_PRF_GetElementPrefix.Auto())
            {
                var prefix = $"Element {i + 1}";

                return prefix;
            }
        }

        public static string GetStyledName(string prefix, string postfix)
        {
            using (_PRF_GetStyledName.Auto())
            {
                var fullName = $"{prefix.Nicify()} | {postfix.Nicify()}";

                return fullName;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AppaControlNamer) + ".";

        private static readonly ProfilerMarker _PRF_GetElementPrefix =
            new ProfilerMarker(_PRF_PFX + nameof(GetElementPrefix));

        private static readonly ProfilerMarker _PRF_GetStyledName =
            new ProfilerMarker(_PRF_PFX + nameof(GetStyledName));

        #endregion
    }
}
