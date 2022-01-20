using Appalachia.Core.Objects.Delegates;
using Appalachia.Core.Objects.Delegates.Extensions;
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
        public event ValueArgs<T>.Handler SettingsChanged;

        protected void InvokeSettingsChanged()
        {
            using (_PRF_InvokeSettingsChanged.Auto())
            {
                if (this is T t)
                {
                    SettingsChanged.RaiseEvent(t);
                }
            }
        }

        #region Profiling

        private static readonly string _PRF_PFX1 = typeof(T).Name + ".";

        protected static readonly ProfilerMarker _PRF_InvokeSettingsChanged =
            new ProfilerMarker(_PRF_PFX1 + nameof(InvokeSettingsChanged));

        #endregion
    }

    public partial class SingletonAppalachiaObject<T>
    {
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

    public partial class AppalachiaSelectable<T>
    {
    }
}
