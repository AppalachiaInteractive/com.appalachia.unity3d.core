using System;
using Appalachia.Core.Objects.Root.Contracts;
using Unity.Profiling;

// ReSharper disable StaticMemberInGenericType

namespace Appalachia.Core.Objects.Root
{
    public partial class AppalachiaRepository : ISingleton<AppalachiaRepository>, ISingleton
    {
        #region Static Fields and Autoproperties

        [NonSerialized] private static AppalachiaRepository ___instance;

        [NonSerialized] private static object _instanceLock;

        #endregion

        public static bool HasInstance => ___instance != null;

        private static AppalachiaRepository instance => ___instance;

        private static object InstanceWriteLock
        {
            get
            {
                _instanceLock ??= new object();

                return _instanceLock;
            }
        }

        internal static void SetInstance(AppalachiaRepository i)
        {
            using (_PRF_SetInstance.Auto())
            {
                var original = ___instance;
                var current = i;

                lock (InstanceWriteLock)
                {
                    ___instance = current;
                }

                InvokeInstanceAvailable(original, current);
            }
        }

        private static void InvokeInstanceAvailable(
            AppalachiaRepository original,
            AppalachiaRepository current)
        {
            using (_PRF_InvokeInstanceAvailable.Auto())
            {
                if (_instanceAvailableSubscribers == null) // no subscribers
                {
                    return;
                }

                if (_instanceAvailableSubscribers.Count == 0) // no subscribers
                {
                    return;
                }

                if (current == null) // instance was unset
                {
                    return;
                }

                if (original == current) // no change in instance
                {
                    return;
                }

                foreach (var subscriber in _instanceAvailableSubscribers)
                {
                    subscriber?.Invoke(current);
                }
            }
        }

        #region ISingleton Members

        bool ISingleton.IsReady => HasInstance;

        object ISingleton.InstanceWriteLock => InstanceWriteLock;

        ISingleton ISingleton.instance => instance;

        void ISingleton.InvokeInstanceAvailable(ISingleton original, ISingleton current)
        {
            InvokeInstanceAvailable(original as AppalachiaRepository, current as AppalachiaRepository);
        }

        void ISingleton.SetSingletonInstance(ISingleton i)
        {
            SetInstance(i as AppalachiaRepository);
        }

        #endregion

        #region ISingleton<AppalachiaRepository> Members

        bool ISingleton<AppalachiaRepository>.HasInstance => HasInstance;

        object ISingleton<AppalachiaRepository>.InstanceWriteLock => InstanceWriteLock;

        AppalachiaRepository ISingleton<AppalachiaRepository>.instance => instance;

        void ISingleton<AppalachiaRepository>.InvokeInstanceAvailable(
            AppalachiaRepository original,
            AppalachiaRepository current)
        {
            InvokeInstanceAvailable(original, current);
        }

        void ISingleton<AppalachiaRepository>.SetInstance(AppalachiaRepository i)
        {
            SetInstance(i);
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_SetInstance =
            new ProfilerMarker(_PRF_PFX + nameof(SetInstance));

        private static readonly ProfilerMarker _PRF_InvokeInstanceAvailable =
            new ProfilerMarker(_PRF_PFX + nameof(InvokeInstanceAvailable));

        #endregion
    }

    public partial class AppalachiaObject
    {
    }

    public partial class AppalachiaObject<T>
    {
    }

    public partial class SingletonAppalachiaObject<T> : ISingleton<T>, ISingleton
    {
        #region Static Fields and Autoproperties

        [NonSerialized] private static object ___instanceLock;
        [NonSerialized] private static T ___instance;

        #endregion

        public static bool HasInstance => ___instance != null;

        protected static T instance => ___instance;

        private static object InstanceWriteLock
        {
            get
            {
                ___instanceLock ??= new object();

                return ___instanceLock;
            }
        }

        internal static void SetInstance(T i)
        {
            using (_PRF_SetInstance.Auto())
            {
                var original = ___instance;
                var current = i;

                lock (InstanceWriteLock)
                {
                    ___instance = current;
                }

                InvokeInstanceAvailable(original, current);
            }
        }

        private static void InvokeInstanceAvailable(T original, T current)
        {
            using (_PRF_InvokeInstanceAvailable.Auto())
            {
                if (_instanceAvailableSubscribers == null) // no subscribers
                {
                    return;
                }

                if (_instanceAvailableSubscribers.Count == 0) // no subscribers
                {
                    return;
                }

                if (current == null) // instance was unset
                {
                    return;
                }

                if (original == current) // no change in instance
                {
                    return;
                }

                foreach (var subscriber in _instanceAvailableSubscribers)
                {
                    subscriber?.Invoke(current);
                }
            }
        }

        #region ISingleton Members

        bool ISingleton.IsReady => HasInstance;

        object ISingleton.InstanceWriteLock => InstanceWriteLock;

        ISingleton ISingleton.instance => instance;

        void ISingleton.InvokeInstanceAvailable(ISingleton original, ISingleton current)
        {
            InvokeInstanceAvailable(original as T, current as T);
        }

        void ISingleton.SetSingletonInstance(ISingleton i)
        {
            SetInstance(i as T);
        }

        #endregion

        #region ISingleton<T> Members

        bool ISingleton<T>.HasInstance => HasInstance;

        object ISingleton<T>.InstanceWriteLock => InstanceWriteLock;

        T ISingleton<T>.instance => instance;

        void ISingleton<T>.InvokeInstanceAvailable(T original, T current)
        {
            InvokeInstanceAvailable(original, current);
        }

        void ISingleton<T>.SetInstance(T i)
        {
            SetInstance(i);
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_SetInstance =
            new ProfilerMarker(_PRF_PFX + nameof(SetInstance));

        private static readonly ProfilerMarker _PRF_InvokeInstanceAvailable =
            new ProfilerMarker(_PRF_PFX + nameof(InvokeInstanceAvailable));

        #endregion
    }

    public partial class AppalachiaBehaviour
    {
    }

    public partial class AppalachiaBehaviour<T>
    {
    }

    public partial class SingletonAppalachiaBehaviour<T> : ISingleton<T>, ISingleton, ISingletonBehaviour
    {
        #region Static Fields and Autoproperties

        [NonSerialized] private static object ___instanceLock;
        [NonSerialized] private static T ___instance;

        #endregion

        public static bool HasInstance => ___instance != null;

        protected static T instance => ___instance;

        private static object InstanceWriteLock
        {
            get
            {
                ___instanceLock ??= new object();

                return ___instanceLock;
            }
        }

        internal static void SetInstance(T i)
        {
            using (_PRF_SetInstance.Auto())
            {
                var original = ___instance;
                var current = i;

                lock (InstanceWriteLock)
                {
                    ___instance = current;
                }

                InvokeInstanceAvailable(original, current);
            }
        }

        private static void InvokeInstanceAvailable(T original, T current)
        {
            using (_PRF_InvokeInstanceAvailable.Auto())
            {
                if (_instanceAvailableSubscribers == null) // no subscribers
                {
                    return;
                }

                if (_instanceAvailableSubscribers.Count == 0) // no subscribers
                {
                    return;
                }

                if (current == null) // instance was unset
                {
                    return;
                }

                if (original == current) // no change in instance
                {
                    return;
                }

                foreach (var subscriber in _instanceAvailableSubscribers)
                {
                    subscriber?.Invoke(current);
                }
            }
        }

        #region ISingleton Members

        bool ISingleton.IsReady => HasInstance;

        object ISingleton.InstanceWriteLock => InstanceWriteLock;

        ISingleton ISingleton.instance => instance;

        void ISingleton.InvokeInstanceAvailable(ISingleton original, ISingleton current)
        {
            InvokeInstanceAvailable(original as T, current as T);
        }

        void ISingleton.SetSingletonInstance(ISingleton i)
        {
            SetInstance(i as T);
        }

        #endregion

        #region ISingleton<T> Members

        bool ISingleton<T>.HasInstance => HasInstance;

        object ISingleton<T>.InstanceWriteLock => InstanceWriteLock;

        T ISingleton<T>.instance => instance;

        void ISingleton<T>.InvokeInstanceAvailable(T original, T current)
        {
            InvokeInstanceAvailable(original, current);
        }

        void ISingleton<T>.SetInstance(T i)
        {
            SetInstance(i);
        }

        #endregion

        #region ISingletonBehaviour Members

        void ISingletonBehaviour.EnsureInstanceIsPrepared(ISingletonBehaviour callingInstance)
        {
            if (callingInstance is T i)
            {
                EnsureInstanceIsPrepared(i);
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_SetInstance =
            new ProfilerMarker(_PRF_PFX + nameof(SetInstance));

        private static readonly ProfilerMarker _PRF_InvokeInstanceAvailable =
            new ProfilerMarker(_PRF_PFX + nameof(InvokeInstanceAvailable));

        #endregion
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
