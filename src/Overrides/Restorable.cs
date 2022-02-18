#region

using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using Object = UnityEngine.Object;

#endregion

namespace Appalachia.Core.Overrides
{
    [NonSerializable]
    public abstract class Restorable : AppalachiaBase, IDisposable
    {
        protected Restorable(Object owner) : base(owner)
        {
        }

        #region Fields and Autoproperties

        public Restorable next;

        #endregion

        public static Restorable<TR> New<TR>(TR initial, TR newValue, Action<TR> assignment, Object owner)
        {
            using (_PRF_Restore.Auto())
            {
                return new Restorable<TR>(initial, newValue, assignment, owner);
            }
        }

        public Restorable CombineWith(Restorable next)
        {
            using (_PRF_CombineWith.Auto())
            {
                this.next = next;

                return next;
            }
        }

        #region IDisposable Members

        public abstract void Dispose();

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(Restorable) + ".";

        private static readonly ProfilerMarker _PRF_Restore = new ProfilerMarker(_PRF_PFX + nameof(New));

        private static readonly ProfilerMarker _PRF_CombineWith =
            new ProfilerMarker(_PRF_PFX + nameof(CombineWith));

        #endregion
    }

    public sealed class Restorable<T> : Restorable
    {
        public Restorable(T initial, T newValue, Action<T> assignment, Object owner) : base(owner)
        {
            using (_PRF_Restorable.Auto())
            {
                _initial = initial;
                _assignment = assignment;
                Name = ZString.Format("{0}<{1}>", nameof(Restorable), typeof(T).Name);

                _assignment(newValue);
            }
        }

        #region Fields and Autoproperties

        private Action<T> _assignment;
        private T _initial;

        /// <inheritdoc />
        public override string Name { get; }

        #endregion

        /// <inheritdoc />
        public override void Dispose()
        {
            using (_PRF_Dispose.Auto())
            {
                _assignment(_initial);
                _initial = default;
                _assignment = null;

                if (next != null)
                {
                    next.Dispose();
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(Restorable<T>) + ".";

        private static readonly ProfilerMarker _PRF_Restorable =
            new ProfilerMarker(_PRF_PFX + nameof(Restorable));

        private static readonly ProfilerMarker _PRF_Dispose = new ProfilerMarker(_PRF_PFX + nameof(Dispose));

        #endregion
    }
}
