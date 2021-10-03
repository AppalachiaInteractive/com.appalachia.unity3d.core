#region

using System;
using Appalachia.Core.Aspects.Tracing;
using Appalachia.Core.Behaviours;
using Unity.Profiling;

#endregion

namespace Appalachia.Core.Editing
{
    public abstract class Restorable : InternalBase<Restorable>, IDisposable
    {
        
        public Restorable next;

        public abstract void Dispose();



        private static readonly ProfilerMarker _PRF_Restorable_Restore = new ProfilerMarker("Restorable.Restore");
        public static Restorable Restore<TR>(TR initial, TR newValue, Action<TR> assignment)
        {
            using (_PRF_Restorable_Restore.Auto())
            {
                return new Restorable<TR>(initial, newValue, assignment);
            }
        }


        private static readonly ProfilerMarker _PRF_Restorable_DisableTracing = new ProfilerMarker("Restorable.DisableTracing");
        public static Restorable DisableTracing()
        {
            using (_PRF_Restorable_DisableTracing.Auto())
            {
                return new Restorable<bool>(TraceMarkerSet.InternalDisable, false, orig => TraceMarkerSet.InternalDisable = orig);
            }
        }


        private static readonly ProfilerMarker _PRF_Restorable_CombineWith = new ProfilerMarker("Restorable.CombineWith");
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
        private T _initial;
        private Action<T> _assignment;


        private static readonly ProfilerMarker _PRF_Restorable_Restorable = new ProfilerMarker("Restorable.Restorable");
        public Restorable(T initial, T newValue, Action<T> assignment)
        {
            using (_PRF_Restorable_Restorable.Auto())
            {
                _initial = initial;
                _assignment = assignment;

                _assignment(newValue);
            }
        }


        private static readonly ProfilerMarker _PRF_Restorable_Dispose = new ProfilerMarker("Restorable.Dispose");
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
