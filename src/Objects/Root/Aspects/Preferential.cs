using System.Diagnostics.CodeAnalysis;
using Appalachia.Core.Objects.Root.Contracts;
using Unity.Profiling;

namespace Appalachia.Core.Objects.Root
{
    public partial class AppalachiaObject : IPreferential
    {
        [SuppressMessage("ReSharper", "NotAccessedVariable")]
        public virtual void InitializePreferences()
        {
            using (_PRF_InitializePreferences.Auto())
            {
            }
        }

        #region IPreferential Members

        void IPreferential.InitializePreferences()
        {
            InitializePreferences();
        }

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_InitializePreferences =
            new ProfilerMarker(_PRF_PFX + nameof(InitializePreferences));

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

    public partial class AppalachiaBehaviour : IPreferential
    {
        public virtual void InitializePreferences()
        {
            using (_PRF_InitializePreferences.Auto())
            {
            }
        }

        #region IPreferential Members

        void IPreferential.InitializePreferences()
        {
            InitializePreferences();
        }

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_InitializePreferences =
            new ProfilerMarker(_PRF_PFX + nameof(InitializePreferences));

        #endregion
    }

    public partial class AppalachiaBehaviour<T>
    {
    }

    public partial class SingletonAppalachiaBehaviour<T>
    {
    }

    public partial class AppalachiaSimpleBase : IPreferential
    {
        public virtual void InitializePreferences()
        {
            using (_PRF_InitializePreferences.Auto())
            {
            }
        }

        #region IPreferential Members

        void IPreferential.InitializePreferences()
        {
            InitializePreferences();
        }

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_InitializePreferences =
            new ProfilerMarker(_PRF_PFX + nameof(InitializePreferences));

        #endregion
    }

    public partial class AppalachiaBase
    {
    }

    public partial class AppalachiaBase<T>
    {
    }

    public partial class AppalachiaSimplePlayable : IPreferential
    {
        public virtual void InitializePreferences()
        {
            using (_PRF_InitializePreferences.Auto())
            {
            }
        }

        #region IPreferential Members

        void IPreferential.InitializePreferences()
        {
            InitializePreferences();
        }

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_InitializePreferences =
            new ProfilerMarker(_PRF_PFX + nameof(InitializePreferences));

        #endregion
    }

    public partial class AppalachiaPlayable
    {
    }

    public partial class AppalachiaPlayable<T>
    {
    }

    public partial class AppalachiaSelectable<T>
    {
    }
}
