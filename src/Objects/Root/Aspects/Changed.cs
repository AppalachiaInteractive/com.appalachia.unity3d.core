using System;
using Appalachia.Utility.Events;
using Appalachia.Utility.Events.Contracts;
using Appalachia.Utility.Events.Extensions;
using Appalachia.Utility.Reflection.Extensions;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Root
{
    public partial class AppalachiaObject : IChangePublisher
    {
        #region Fields and Autoproperties

        [HideInInspector] public AppaEvent.Data Changed;

        #endregion

        public virtual void OnChanged()
        {
            if (_PRF_OnChanged.Handle == IntPtr.Zero)
            {
                _PRF_OnChanged = new ProfilerMarker(GetType().GetReadableName() + "." + nameof(OnChanged));
            }

            using (_PRF_OnChanged.Auto())
            {
                Changed.RaiseEvent();
                MarkAsModified();
            }
        }

        public void SuspendChanges()
        {
            using (_PRF_SuspendChanges.Auto())
            {
                Changed.Suspend();
            }
        }

        public void UnsuspendChanges()
        {
            using (_PRF_UnsuspendChanges.Auto())
            {
                Changed.Unsuspend();
            }
        }

        #region IChangePublisher Members

        void IChangePublisher.OnChanged()
        {
            OnChanged();
        }

        public void SubscribeToChanges(AppaEvent.Handler handler)
        {
            using (_PRF_SubscribeToChanges.Auto())
            {
                Changed.Event += handler;
            }
        }

        public void UnsubscribeFromChanges(AppaEvent.Handler handler)
        {
            using (_PRF_UnsubscribeFromChanges.Auto())
            {
                Changed.Event -= handler;
            }
        }

        #endregion

        #region Profiling

        protected ProfilerMarker _PRF_OnChanged;

        protected static readonly ProfilerMarker _PRF_SuspendChanges =
            new ProfilerMarker(_PRF_PFX + nameof(SuspendChanges));

        protected static readonly ProfilerMarker _PRF_UnsuspendChanges =
            new ProfilerMarker(_PRF_PFX + nameof(UnsuspendChanges));

        protected static readonly ProfilerMarker _PRF_UnsubscribeFromChanges =
            new ProfilerMarker(_PRF_PFX + nameof(UnsubscribeFromChanges));

        protected static readonly ProfilerMarker _PRF_SubscribeToChanges =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeToChanges));

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

    public partial class AppalachiaBehaviour : IChangePublisher
    {
        #region Fields and Autoproperties

        [HideInInspector] public AppaEvent.Data Changed;

        #endregion

        public void SuspendChanges()
        {
            using (_PRF_SuspendChanges.Auto())
            {
                Changed.Suspend();
            }
        }

        public void UnsuspendChanges()
        {
            using (_PRF_UnsuspendChanges.Auto())
            {
                Changed.Unsuspend();
            }
        }

        protected virtual void OnChanged()
        {
            if (_PRF_OnChanged.Handle == IntPtr.Zero)
            {
                _PRF_OnChanged = new ProfilerMarker(GetType().GetReadableName() + "." + nameof(OnChanged));
            }

            using (_PRF_OnChanged.Auto())
            {
                Changed.RaiseEvent();
            }
        }

        #region IChangePublisher Members

        void IChangePublisher.OnChanged()
        {
            OnChanged();
        }

        public void SubscribeToChanges(AppaEvent.Handler handler)
        {
            using (_PRF_SubscribeToChanges.Auto())
            {
                Changed.Event += handler;
            }
        }

        public void UnsubscribeFromChanges(AppaEvent.Handler handler)
        {
            using (_PRF_UnsubscribeFromChanges.Auto())
            {
                Changed.Event -= handler;
            }
        }

        #endregion

        #region Profiling

        protected ProfilerMarker _PRF_OnChanged;

        private static readonly ProfilerMarker _PRF_SuspendChanges =
            new ProfilerMarker(_PRF_PFX + nameof(SuspendChanges));

        private static readonly ProfilerMarker _PRF_UnsuspendChanges =
            new ProfilerMarker(_PRF_PFX + nameof(UnsuspendChanges));

        private static readonly ProfilerMarker _PRF_UnsubscribeFromChanges =
            new ProfilerMarker(_PRF_PFX + nameof(UnsubscribeFromChanges));

        private static readonly ProfilerMarker _PRF_SubscribeToChanges =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeToChanges));

        #endregion
    }

    public partial class AppalachiaBehaviour<T>
    {
    }

    public partial class SingletonAppalachiaBehaviour<T>
    {
    }

    public partial class AppalachiaSimpleBase : IChangePublisher
    {
        #region Fields and Autoproperties

        [HideInInspector] public AppaEvent.Data Changed;

        #endregion

        public virtual void SuspendChanges()
        {
            using (_PRF_SuspendChanges.Auto())
            {
                Changed.Suspend();
            }
        }

        public virtual void UnsuspendChanges()
        {
            using (_PRF_UnsuspendChanges.Auto())
            {
                Changed.Unsuspend();
            }
        }

        protected virtual void OnChanged()
        {
            if (_PRF_OnChanged.Handle == IntPtr.Zero)
            {
                _PRF_OnChanged = new ProfilerMarker(GetType().GetReadableName() + "." + nameof(OnChanged));
            }

            using (_PRF_OnChanged.Auto())
            {
                Changed.RaiseEvent();
            }
        }

        #region IChangePublisher Members

        void IChangePublisher.OnChanged()
        {
            OnChanged();
        }

        public void SubscribeToChanges(AppaEvent.Handler handler)
        {
            using (_PRF_SubscribeToChanges.Auto())
            {
                Changed.Event += handler;
            }
        }

        public void UnsubscribeFromChanges(AppaEvent.Handler handler)
        {
            using (_PRF_UnsubscribeFromChanges.Auto())
            {
                Changed.Event -= handler;
            }
        }

        #endregion

        #region Profiling

        protected ProfilerMarker _PRF_OnChanged;

        protected static readonly ProfilerMarker _PRF_SuspendChanges =
            new ProfilerMarker(_PRF_PFX + nameof(SuspendChanges));

        protected static readonly ProfilerMarker _PRF_UnsuspendChanges =
            new ProfilerMarker(_PRF_PFX + nameof(UnsuspendChanges));

        private static readonly ProfilerMarker _PRF_UnsubscribeFromChanges =
            new ProfilerMarker(_PRF_PFX + nameof(UnsubscribeFromChanges));

        private static readonly ProfilerMarker _PRF_SubscribeToChanges =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeToChanges));

        #endregion
    }

    public partial class AppalachiaBase
    {
    }

    public partial class AppalachiaBase<T>
    {
    }

    public partial class AppalachiaSimplePlayable : IChangePublisher
    {
        #region Fields and Autoproperties

        [HideInInspector] public AppaEvent.Data Changed;

        #endregion

        public void SuspendChanges()
        {
            using (_PRF_SuspendChanges.Auto())
            {
                Changed.Suspend();
            }
        }

        public void UnsuspendChanges()
        {
            using (_PRF_UnsuspendChanges.Auto())
            {
                Changed.Unsuspend();
            }
        }

        protected virtual void OnChanged()
        {
            if (_PRF_OnChanged.Handle == IntPtr.Zero)
            {
                _PRF_OnChanged = new ProfilerMarker(GetType().GetReadableName() + "." + nameof(OnChanged));
            }

            using (_PRF_OnChanged.Auto())
            {
                Changed.RaiseEvent();
            }
        }

        #region IChangePublisher Members

        void IChangePublisher.OnChanged()
        {
            OnChanged();
        }

        public void SubscribeToChanges(AppaEvent.Handler handler)
        {
            using (_PRF_SubscribeToChanges.Auto())
            {
                Changed.Event += handler;
            }
        }

        public void UnsubscribeFromChanges(AppaEvent.Handler handler)
        {
            using (_PRF_UnsubscribeFromChanges.Auto())
            {
                Changed.Event -= handler;
            }
        }

        #endregion

        #region Profiling

        protected ProfilerMarker _PRF_OnChanged = new ProfilerMarker(_PRF_PFX + nameof(OnChanged));

        private static readonly ProfilerMarker _PRF_SuspendChanges =
            new ProfilerMarker(_PRF_PFX + nameof(SuspendChanges));

        private static readonly ProfilerMarker _PRF_UnsuspendChanges =
            new ProfilerMarker(_PRF_PFX + nameof(UnsuspendChanges));

        private static readonly ProfilerMarker _PRF_UnsubscribeFromChanges =
            new ProfilerMarker(_PRF_PFX + nameof(UnsubscribeFromChanges));

        private static readonly ProfilerMarker _PRF_SubscribeToChanges =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeToChanges));

        #endregion
    }

    public partial class AppalachiaPlayable
    {
    }

    public partial class AppalachiaPlayable<T>
    {
    }

    public partial class AppalachiaSelectable<T> : IChangePublisher
    {
        #region Fields and Autoproperties

        [HideInInspector] public AppaEvent.Data Changed;

        #endregion

        public void SuspendChanges()
        {
            using (_PRF_SuspendChanges.Auto())
            {
                Changed.Suspend();
            }
        }

        public void UnsuspendChanges()
        {
            using (_PRF_UnsuspendChanges.Auto())
            {
                Changed.Unsuspend();
            }
        }

        protected virtual void OnChanged()
        {
            using (_PRF_OnChanged.Auto())
            {
                Changed.RaiseEvent();
            }
        }

        #region IChangePublisher Members

        void IChangePublisher.OnChanged()
        {
            OnChanged();
        }

        public void SubscribeToChanges(AppaEvent.Handler handler)
        {
            using (_PRF_SubscribeToChanges.Auto())
            {
                Changed.Event += handler;
            }
        }

        public void UnsubscribeFromChanges(AppaEvent.Handler handler)
        {
            using (_PRF_UnsubscribeFromChanges.Auto())
            {
                Changed.Event -= handler;
            }
        }

        #endregion

        #region Profiling

        protected static readonly string _PRF_PFX8 = typeof(T).Name + ".";

        protected static readonly ProfilerMarker _PRF_OnChanged = new ProfilerMarker(_PRF_PFX8 + nameof(OnChanged));

        private static readonly ProfilerMarker _PRF_SubscribeToChanges =
            new ProfilerMarker(_PRF_PFX8 + nameof(SubscribeToChanges));

        private static readonly ProfilerMarker _PRF_SuspendChanges =
            new ProfilerMarker(_PRF_PFX8 + nameof(SuspendChanges));

        private static readonly ProfilerMarker _PRF_UnsubscribeFromChanges =
            new ProfilerMarker(_PRF_PFX8 + nameof(UnsubscribeFromChanges));

        private static readonly ProfilerMarker _PRF_UnsuspendChanges =
            new ProfilerMarker(_PRF_PFX8 + nameof(UnsuspendChanges));

        #endregion
    }
}
