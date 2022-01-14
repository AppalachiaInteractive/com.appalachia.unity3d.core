using Appalachia.Utility.Extensions;
using Unity.Profiling;

namespace Appalachia.Core.Objects.Root
{
    public partial class AppalachiaObject
    {
    }

    public partial class AppalachiaRepository
    {
    }

    public partial class AppalachiaObject<T>
    {
    }

    public partial class SingletonAppalachiaObject<T>
    {
#if UNITY_EDITOR

        #region Constants and Static Readonly

        private static readonly string _PRF_PFX7 = typeof(T).Name + ".";

        #endregion

        protected void ValidateAddressableInformation()
        {
            using (_PRF_ValidateAddressableInformation.Auto())
            {
                if (this.EnsureIsAddressable(out var guid))
                {
                    var targetInfo = this.GetAddressableTargetInfo();

                    var correctName = typeof(T).Name;

                    if (targetInfo.Address != correctName)
                    {
                        targetInfo.MainAssetEntry.SetAddress(correctName);
                    }
                }
                else
                {
                    this.AddToAddressableGroup();
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_ValidateAddressableInformation =
            new ProfilerMarker(_PRF_PFX7 + nameof(ValidateAddressableInformation));

        #endregion

#endif
    }

    public partial class AppalachiaBehaviour
    {
    }

    public partial class AppalachiaBehaviour<T>
    {
    }

    public partial class SingletonAppalachiaBehaviour<T>
    {
    }

    public partial class AppalachiaSimpleBase
    {
    }

    public partial class AppalachiaBase
    {
    }

    public partial class AppalachiaBase<T>
    {
    }

    public partial class AppalachiaSimplePlayable
    {
    }

    public partial class AppalachiaPlayable
    {
    }

    public partial class AppalachiaPlayable<T>
    {
    }
}
