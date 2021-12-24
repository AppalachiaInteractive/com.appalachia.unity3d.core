using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Extensions;
using Unity.Profiling;

namespace Appalachia.Core.Objects.Root
{
    public partial class AppalachiaObject : IUnitySerializable
    {
        #region IUnitySerializable Members

        public void MarkAsModified()
        {
            using (_PRF_MarkAsModified.Auto())
            {
                Modifications.MarkAsModified(this);
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_MarkAsModified =
            new ProfilerMarker(_PRF_PFX + nameof(MarkAsModified));

        #endregion
    }

    public partial class AppalachiaRepository
    {
    }

    public partial class AppalachiaObject<T>
    {
    }

    public partial class SingletonAppalachiaObject<T>
    {
    }

    public partial class AppalachiaBehaviour
    {
    }

    public partial class AppalachiaBehaviour<T> : IUnitySerializable
    {
        #region IUnitySerializable Members

        public void MarkAsModified()
        {
            using (_PRF_MarkAsModified.Auto())
            {
                Modifications.MarkAsModified(this);
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_MarkAsModified =
            new ProfilerMarker(_PRF_PFX + nameof(MarkAsModified));

        #endregion
    }

    public partial class SingletonAppalachiaBehaviour<T>
    {
    }

    public partial class AppalachiaSimpleBase
    {
    }

    public partial class AppalachiaBase : IUnitySerializable
    {
        #region IUnitySerializable Members

        public void MarkAsModified()
        {
            using (_PRF_MarkAsModified.Auto())
            {
                Extensions.MarkAsModified(this);
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_MarkAsModified =
            new ProfilerMarker(_PRF_PFX + nameof(MarkAsModified));

        #endregion
    }

    public partial class AppalachiaBase<T>
    {
    }

    public partial class AppalachiaPlayable
    {
    }

    public partial class AppalachiaPlayable<T>
    {
    }
}
