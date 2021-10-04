#region

using System;
using Appalachia.Base.Behaviours;
using Appalachia.Core.Aspects.Tracing;
using Unity.Profiling;

#endregion

namespace Appalachia.Core.Overrides
{
    public abstract class Restorable : InternalBase<Restorable>, IDisposable
    {
        private static readonly ProfilerMarker _PRF_Restorable_Restore = new("Restorable.Restore");

        private static readonly ProfilerMarker _PRF_Restorable_DisableTracing =
            new("Restorable.DisableTracing");

        private static readonly ProfilerMarker _PRF_Restorable_CombineWith =
            new("Restorable.CombineWith");

        public Restorable next;

        public abstract void Dispose();

        public static Restorable Restore<TR>(TR initial, TR newValue, Action<TR> assignment)
        {
            using (_PRF_Restorable_Restore.Auto())
            {
                return new Restorable<TR>(initial, newValue, assignment);
            }
        }

        public static Restorable DisableTracing()
        {
            using (_PRF_Restorable_DisableTracing.Auto())
            {
                return new Restorable<bool>(
                    TraceMarkerSet.InternalDisable,
                    false,
                    orig => TraceMarkerSet.InternalDisable = orig
                );
            }
        }

        public Restorable CombineWith(Restorable next)
        {
            using (_PRF_Restorable_CombineWith.Auto())
            {
                this.next = next;

                return next;
            }
        }
    }

    public class Restorable<T> : Restorable
    {
        private static readonly ProfilerMarker _PRF_Restorable_Restorable =
            new("Restorable.Restorable");

        private static readonly ProfilerMarker _PRF_Restorable_Dispose = new("Restorable.Dispose");
        private Action<T> _assignment;
        private T _initial;

        public Restorable(T initial, T newValue, Action<T> assignment)
        {
            using (_PRF_Restorable_Restorable.Auto())
            {
                _initial = initial;
                _assignment = assignment;

                _assignment(newValue);
            }
        }

        public override void Dispose()
        {
            using (_PRF_Restorable_Dispose.Auto())
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
    }
}
