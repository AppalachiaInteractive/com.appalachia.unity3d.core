using System;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Unity.Profiling;

namespace Appalachia.Core.Objects.Availability
{
    public class AvailabilitySet<T1, T2> : BaseAvailabilitySetOf<Action<T1, T2>>, IAvailabilitySet<T1, T2>
    {
        public AvailabilitySet(int sortOrder) : base(sortOrder)
        {
        }

        #region Fields and Autoproperties

        public AvailabilityData Data1;
        public AvailabilityData Data2;

        #endregion

        /// <inheritdoc />
        public override bool IsFullyAvailable => Data1.IsAvailable && Data2.IsAvailable;

        public void CopyTo(AvailabilitySet<T1, T2> other)
        {
            using (_PRF_CopyTo.Auto())
            {
                other.Data1 = Data1;
                other.Data2 = Data2;
                other.SortOrder = SortOrder;
                other.Action = Action;
                other.ActionCalled = ActionCalled;

                if (other.Data1 != Data1)
                {
                    throw new ArgumentNullException(nameof(Data1));
                }

                if (other.Data2 != Data2)
                {
                    throw new ArgumentNullException(nameof(Data2));
                }

                if (other.SortOrder != SortOrder)
                {
                    throw new ArgumentNullException(nameof(SortOrder));
                }

                if (other.Action != Action)
                {
                    throw new ArgumentNullException(nameof(Action));
                }

                if (other.ActionCalled != ActionCalled)
                {
                    throw new ArgumentNullException(nameof(ActionCalled));
                }

                other.Data1.AvailabilityChanged.Event += other.OnAvailabilityChanged;
                other.Data2.AvailabilityChanged.Event += other.OnAvailabilityChanged;
            }
        }

        /// <inheritdoc />
        protected override void OnFullyAvailable(Action<T1, T2> action)
        {
            using (_PRF_OnFullyAvailable.Auto())
            {
                var instance1 = ((AvailabilityData<T1>)Data1).instance;
                var instance2 = ((AvailabilityData<T2>)Data2).instance;

                action(instance1, instance2);
            }
        }

        private IAvailabilitySet<T1, T2, TNext> And<TNext>(Func<AvailabilityData<TNext>> newDataFunction)
        {
            using (_PRF_And.Auto())
            {
                var result = new AvailabilitySet<T1, T2, TNext>(SortOrder)
                {
                    Data1 = Data1, Data2 = Data2, Data3 = newDataFunction()
                };

                result.Data1.AvailabilityChanged.Event -= OnAvailabilityChanged;
                result.Data2.AvailabilityChanged.Event -= OnAvailabilityChanged;
                result.Data3.AvailabilityChanged.Event -= OnAvailabilityChanged;
                result.Data1.AvailabilityChanged.Event += result.OnAvailabilityChanged;
                result.Data2.AvailabilityChanged.Event += result.OnAvailabilityChanged;
                result.Data3.AvailabilityChanged.Event += result.OnAvailabilityChanged;

                return result;
            }
        }

        #region IAvailabilitySet<T1,T2> Members

        public IAvailabilitySet<T1, T2, TNext> AndBehaviour<TNext>(TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>
        {
            using (_PRF_AndBehaviour.Auto())
            {
                return And(() => AvailabilityData<TNext>.ForBehaviour<TNext>(SortOrder));
            }
        }

        public IAvailabilitySet<T1, T2, TNext> AndObject<TNext>(TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>
        {
            using (_PRF_AndObject.Auto())
            {
                return And(() => AvailabilityData<TNext>.ForObject<TNext>(SortOrder));
            }
        }

        public IAvailabilitySet<T1, T2, TNext> AndBehaviour<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>
        {
            using (_PRF_AndBehaviour.Auto())
            {
                return And(() => AvailabilityData<TNext>.ForBehaviour<TNext>(SortOrder));
            }
        }

        public IAvailabilitySet<T1, T2, TNext> AndObject<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>
        {
            using (_PRF_AndObject.Auto())
            {
                return And(() => AvailabilityData<TNext>.ForObject<TNext>(SortOrder));
            }
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(AvailabilitySet<T1, T2>) + ".";

        private static readonly ProfilerMarker _PRF_CopyTo = new ProfilerMarker(_PRF_PFX + nameof(CopyTo));

        private static readonly ProfilerMarker _PRF_AndBehaviour =
            new ProfilerMarker(_PRF_PFX + nameof(AndBehaviour));

        private static readonly ProfilerMarker _PRF_AndObject =
            new ProfilerMarker(_PRF_PFX + nameof(AndObject));

        private static readonly ProfilerMarker _PRF_And = new ProfilerMarker(_PRF_PFX + nameof(And));

        #endregion
    }
}
