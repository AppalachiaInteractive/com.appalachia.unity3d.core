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

        private static readonly string _PRF_PFX4 = typeof(T).Name + ".";

        private static readonly ProfilerMarker _PRF_MarkAsModified =
            new ProfilerMarker(_PRF_PFX4 + nameof(MarkAsModified));

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

    public partial class AppalachiaSimplePlayable
    {
    }

    public partial class AppalachiaPlayable
    {
    }

    public partial class AppalachiaPlayable<T>
    {
    }

    public partial class AppalachiaSelectable<T>
    {
        public void MarkAsModified()
        {
            using (_PRF_MarkAsModified.Auto())
            {
                Modifications.MarkAsModified(this);
            }
        }

        #region Profiling

        protected static readonly string _PRF_PFX5 = typeof(T).Name + ".";

        private static readonly ProfilerMarker _PRF_MarkAsModified =
            new ProfilerMarker(_PRF_PFX5 + nameof(MarkAsModified));

        #endregion
    }
}
