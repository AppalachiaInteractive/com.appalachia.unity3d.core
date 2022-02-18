using System;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Unity.Profiling;

namespace Appalachia.Core.Objects.Availability
{
    public class AvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8> :
        BaseAvailabilitySetOf<Action<T1, T2, T3, T4, T5, T6, T7, T8>>,
        IAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        public AvailabilitySet(int sortOrder) : base(sortOrder)
        {
        }

        #region Fields and Autoproperties

        public AvailabilityData Data1;
        public AvailabilityData Data2;
        public AvailabilityData Data3;
        public AvailabilityData Data4;
        public AvailabilityData Data5;
        public AvailabilityData Data6;
        public AvailabilityData Data7;
        public AvailabilityData Data8;

        #endregion

        /// <inheritdoc />
        public override bool IsFullyAvailable =>
            Data1.IsAvailable &&
            Data2.IsAvailable &&
            Data3.IsAvailable &&
            Data4.IsAvailable &&
            Data5.IsAvailable &&
            Data6.IsAvailable &&
            Data7.IsAvailable &&
            Data8.IsAvailable;

        public void CopyTo(AvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8> other)
        {
            using (_PRF_CopyTo.Auto())
            {
                other.Data1 = Data1;
                other.Data2 = Data2;
                other.Data3 = Data3;
                other.Data4 = Data4;
                other.Data5 = Data5;
                other.Data6 = Data6;
                other.Data7 = Data7;
                other.Data8 = Data8;
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

                if (other.Data3 != Data3)
                {
                    throw new ArgumentNullException(nameof(Data3));
                }

                if (other.Data4 != Data4)
                {
                    throw new ArgumentNullException(nameof(Data4));
                }

                if (other.Data5 != Data5)
                {
                    throw new ArgumentNullException(nameof(Data5));
                }

                if (other.Data6 != Data6)
                {
                    throw new ArgumentNullException(nameof(Data6));
                }

                if (other.Data7 != Data7)
                {
                    throw new ArgumentNullException(nameof(Data7));
                }

                if (other.Data8 != Data8)
                {
                    throw new ArgumentNullException(nameof(Data8));
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
                other.Data3.AvailabilityChanged.Event += other.OnAvailabilityChanged;
                other.Data4.AvailabilityChanged.Event += other.OnAvailabilityChanged;
                other.Data5.AvailabilityChanged.Event += other.OnAvailabilityChanged;
                other.Data6.AvailabilityChanged.Event += other.OnAvailabilityChanged;
                other.Data7.AvailabilityChanged.Event += other.OnAvailabilityChanged;
                other.Data8.AvailabilityChanged.Event += other.OnAvailabilityChanged;
            }
        }

        /// <inheritdoc />
        protected override void OnFullyAvailable(Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            using (_PRF_OnFullyAvailable.Auto())
            {
                var instance1 = ((AvailabilityData<T1>)Data1).instance;
                var instance2 = ((AvailabilityData<T2>)Data2).instance;
                var instance3 = ((AvailabilityData<T3>)Data3).instance;
                var instance4 = ((AvailabilityData<T4>)Data4).instance;
                var instance5 = ((AvailabilityData<T5>)Data5).instance;
                var instance6 = ((AvailabilityData<T6>)Data6).instance;
                var instance7 = ((AvailabilityData<T7>)Data7).instance;
                var instance8 = ((AvailabilityData<T8>)Data8).instance;

                action(
                    instance1,
                    instance2,
                    instance3,
                    instance4,
                    instance5,
                    instance6,
                    instance7,
                    instance8
                );
            }
        }

        private IAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, TNext> And<TNext>(
            Func<AvailabilityData<TNext>> newDataFunction)
        {
            using (_PRF_And.Auto())
            {
                var result = new AvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, TNext>(SortOrder)
                {
                    Data1 = Data1,
                    Data2 = Data2,
                    Data3 = Data3,
                    Data4 = Data4,
                    Data5 = Data5,
                    Data6 = Data6,
                    Data7 = Data7,
                    Data8 = Data8,
                    Data9 = newDataFunction()
                };

                result.Data1.AvailabilityChanged.Event -= OnAvailabilityChanged;
                result.Data2.AvailabilityChanged.Event -= OnAvailabilityChanged;
                result.Data3.AvailabilityChanged.Event -= OnAvailabilityChanged;
                result.Data4.AvailabilityChanged.Event -= OnAvailabilityChanged;
                result.Data5.AvailabilityChanged.Event -= OnAvailabilityChanged;
                result.Data6.AvailabilityChanged.Event -= OnAvailabilityChanged;
                result.Data7.AvailabilityChanged.Event -= OnAvailabilityChanged;
                result.Data8.AvailabilityChanged.Event -= OnAvailabilityChanged;
                result.Data9.AvailabilityChanged.Event -= OnAvailabilityChanged;
                result.Data1.AvailabilityChanged.Event += result.OnAvailabilityChanged;
                result.Data2.AvailabilityChanged.Event += result.OnAvailabilityChanged;
                result.Data3.AvailabilityChanged.Event += result.OnAvailabilityChanged;
                result.Data4.AvailabilityChanged.Event += result.OnAvailabilityChanged;
                result.Data5.AvailabilityChanged.Event += result.OnAvailabilityChanged;
                result.Data6.AvailabilityChanged.Event += result.OnAvailabilityChanged;
                result.Data7.AvailabilityChanged.Event += result.OnAvailabilityChanged;
                result.Data8.AvailabilityChanged.Event += result.OnAvailabilityChanged;
                result.Data9.AvailabilityChanged.Event += result.OnAvailabilityChanged;

                return result;
            }
        }

        #region IAvailabilitySet<T1,T2,T3,T4,T5,T6,T7,T8> Members

        public IAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, TNext> AndBehaviour<TNext>(TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>
        {
            return And(() => AvailabilityData<TNext>.ForBehaviour<TNext>(SortOrder));
        }

        public IAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, TNext> AndObject<TNext>(TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>
        {
            return And(() => AvailabilityData<TNext>.ForObject<TNext>(SortOrder));
        }

        public IAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, TNext> AndBehaviour<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>
        {
            return And(() => AvailabilityData<TNext>.ForBehaviour<TNext>(SortOrder));
        }

        public IAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, TNext> AndObject<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>
        {
            return And(() => AvailabilityData<TNext>.ForObject<TNext>(SortOrder));
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(AvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8>) + ".";
        private static readonly ProfilerMarker _PRF_CopyTo = new ProfilerMarker(_PRF_PFX + nameof(CopyTo));
        private static readonly ProfilerMarker _PRF_And = new ProfilerMarker(_PRF_PFX + nameof(And));

        #endregion
    }
}
