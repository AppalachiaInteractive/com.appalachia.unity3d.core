#if UNITY_EDITOR
using System.Diagnostics.CodeAnalysis;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Strings;
using Unity.Profiling;

// ReSharper disable StaticMemberInGenericType

namespace Appalachia.Core.Objects.Root
{
    public abstract partial class AppalachiaObject
    {
        #region Static Fields and Autoproperties

        private static Utf16PreparedFormat<string, string> _validationFormat;

        #endregion

        protected virtual bool IsDataValid()
        {
            using (_PRF_IsDataValid.Auto())
            {
                return true;
            }
        }

        [SuppressMessage("ReSharper", "FormatStringProblem")]
        private void CheckLogFormat()
        {
            using (_PRF_CheckLogFormat.Auto())
            {
                _validationFormat ??=
                    new Utf16PreparedFormat<string, string>("Data validation failed for {0} {1}.");
            }
        }

        private void ValidateData()
        {
            using (_PRF_ValidateData.Auto())
            {
                if (!IsDataValid())
                {
                    CheckLogFormat();

                    Context.Log.Warn(
                        _validationFormat.Format(GetType().FormatForLogging(), name.FormatNameForLogging()),
                        this
                    );
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_IsDataValid =
            new ProfilerMarker(_PRF_PFX + nameof(IsDataValid));

        private static readonly ProfilerMarker _PRF_CheckLogFormat =
            new ProfilerMarker(_PRF_PFX + nameof(CheckLogFormat));

        private static readonly ProfilerMarker _PRF_ValidateData =
            new ProfilerMarker(_PRF_PFX + nameof(ValidateData));

        #endregion
    }
}

#endif
