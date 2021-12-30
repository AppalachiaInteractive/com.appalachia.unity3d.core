using System.Collections.Generic;

namespace Appalachia.Core.Objects.Root
{
    /*[CallStaticConstructorInEditor]*/

    public partial class AppalachiaObject
    {
    }

    public partial class AppalachiaRepository
    {
        public delegate void InstanceAvailableHandler(AppalachiaRepository instance);

        #region Static Fields and Autoproperties

        private static HashSet<InstanceAvailableHandler> _instanceAvailableSubscribers;

        #endregion

        public static bool IsInstanceAvailable => instance != null;

        public static event InstanceAvailableHandler InstanceAvailable
        {
            add
            {
                _instanceAvailableSubscribers ??= new HashSet<InstanceAvailableHandler>();

                if (IsInstanceAvailable)
                {
                    value?.Invoke(instance);
                }
                else
                {
                    _instanceAvailableSubscribers.Add(value);
                }
            }
            remove
            {
                _instanceAvailableSubscribers ??= new HashSet<InstanceAvailableHandler>();
                _instanceAvailableSubscribers.Remove(value);
            }
        }
    }

    public partial class AppalachiaObject<T>
    {
    }

    public partial class SingletonAppalachiaObject<T>
    {
        public delegate void InstanceAvailableHandler(T instance);

        #region Static Fields and Autoproperties

        private static HashSet<InstanceAvailableHandler> _instanceAvailableSubscribers;

        #endregion

        public static bool IsInstanceAvailable => instance != null;

        public static event InstanceAvailableHandler InstanceAvailable
        {
            add
            {
                _instanceAvailableSubscribers ??= new HashSet<InstanceAvailableHandler>();

                if (IsInstanceAvailable)
                {
                    value?.Invoke(instance);
                }
                else
                {
                    _instanceAvailableSubscribers.Add(value);
                }
            }
            remove
            {
                _instanceAvailableSubscribers ??= new HashSet<InstanceAvailableHandler>();
                _instanceAvailableSubscribers.Remove(value);
            }
        }
    }

    public partial class AppalachiaBehaviour
    {
    }

    public partial class AppalachiaBehaviour<T>
    {
    }

    public partial class SingletonAppalachiaBehaviour<T>
    {
        public delegate void InstanceAvailableHandler(T instance);

        #region Static Fields and Autoproperties

        private static HashSet<InstanceAvailableHandler> _instanceAvailableSubscribers;

        #endregion

        public static bool IsInstanceAvailable => instance != null;

        public static event InstanceAvailableHandler InstanceAvailable
        {
            add
            {
                _instanceAvailableSubscribers ??= new HashSet<InstanceAvailableHandler>();

                if (IsInstanceAvailable)
                {
                    value?.Invoke(instance);
                }
                else
                {
                    _instanceAvailableSubscribers.Add(value);
                }
            }
            remove
            {
                _instanceAvailableSubscribers ??= new HashSet<InstanceAvailableHandler>();
                _instanceAvailableSubscribers.Remove(value);
            }
        }
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
