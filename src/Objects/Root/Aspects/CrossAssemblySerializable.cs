using Appalachia.Core.Objects.Root.Contracts;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Root
{
    public partial class AppalachiaRepository : ICrossAssemblySerializable
    {
        #region ICrossAssemblySerializable Members

        public ScriptableObject GetSerializable()
        {
            using (_PRF_GetSerializable.Auto())
            {
                return this;
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetSerializable =
            new ProfilerMarker(_PRF_PFX + nameof(GetSerializable));

        #endregion
    }

    public partial class AppalachiaObject<T> : ICrossAssemblySerializable
    {
        #region Constants and Static Readonly

        private static readonly string _PRF_PFX5 = typeof(T).Name + ".";

        #endregion

        #region ICrossAssemblySerializable Members

        public ScriptableObject GetSerializable()
        {
            using (_PRF_GetSerializable.Auto())
            {
                return this;
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetSerializable =
            new ProfilerMarker(_PRF_PFX5 + nameof(GetSerializable));

        #endregion
    }

    public partial class SingletonAppalachiaObject<T>
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
