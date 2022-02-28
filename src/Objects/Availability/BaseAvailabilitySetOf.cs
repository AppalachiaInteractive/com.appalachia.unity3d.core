using System;
using Unity.Profiling;

namespace Appalachia.Core.Objects.Availability
{
    public abstract class BaseAvailabilitySetOf<T>
        where T : MulticastDelegate
    {
        protected BaseAvailabilitySetOf(int sortOrder, bool allowMultipleCalls = false)
        {
            _sortOrder = sortOrder;
            _allowMultipleCalls = allowMultipleCalls;
        }

        #region Fields and Autoproperties

        [NonSerialized] private bool _actionCalled;

        private int _sortOrder;
        private readonly bool _allowMultipleCalls;
        private T _action;

        #endregion

        public abstract bool IsFullyAvailable { get; }

        public int SortOrder
        {
            get => _sortOrder;
            set => _sortOrder = value;
        }

        protected bool ActionCalled
        {
            get => _actionCalled;
            set => _actionCalled = value;
        }

        protected T Action
        {
            get => _action;
            set => _action = value;
        }

        public void AreAvailableThen(T availabilityAction)
        {
            using (_PRF_AreAvailableThen.Auto())
            {
                if (availabilityAction == null)
                {
                    throw new ArgumentNullException(nameof(availabilityAction));
                }

                _action = availabilityAction;

                OnAvailabilityChanged();
            }
        }

        public void IsAvailableThen(T availabilityAction)
        {
            using (_PRF_IsAvailableThen.Auto())
            {
                if (availabilityAction == null)
                {
                    throw new ArgumentNullException(nameof(availabilityAction));
                }

                _action = availabilityAction;

                OnAvailabilityChanged();
            }
        }

        public void OnAvailabilityChanged()
        {
            using (_PRF_OnAvailabilityChanged.Auto())
            {
                if (IsFullyAvailable)
                {
                    if (!_allowMultipleCalls && _actionCalled)
                    {
                        return;
                    }

                    if (_action == null)
                    {
                        return;
                    }

                    OnFullyAvailable(_action);

                    _actionCalled = true;
                }
            }
        }

        protected abstract void OnFullyAvailable(T action);

        #region Profiling

        private const string _PRF_PFX = nameof(BaseAvailabilitySetOf<T>) + ".";

        private static readonly ProfilerMarker _PRF_AreAvailableThen =
            new ProfilerMarker(_PRF_PFX + nameof(AreAvailableThen));

        private static readonly ProfilerMarker _PRF_OnAvailabilityChanged =
            new ProfilerMarker(_PRF_PFX + nameof(OnAvailabilityChanged));

        private static readonly ProfilerMarker _PRF_IsAvailableThen =
            new ProfilerMarker(_PRF_PFX + nameof(IsAvailableThen));

        protected static readonly ProfilerMarker _PRF_OnFullyAvailable =
            new ProfilerMarker(_PRF_PFX + nameof(OnFullyAvailable));

        protected static readonly ProfilerMarker _PRF_IsFullyAvailable =
            new ProfilerMarker(_PRF_PFX + nameof(IsFullyAvailable));

        #endregion
    }
}
