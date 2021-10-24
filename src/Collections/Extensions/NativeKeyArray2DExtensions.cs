using Appalachia.Core.Collections.Native;
using Unity.Collections;
using Unity.Mathematics;

namespace Appalachia.Core.Collections.Extensions
{
    public static class NativeKeyArray2DExtensions
    {
        public static void EnsureCapacity<TK, TV>(
            this ref NativeKeyArray2D<TK, TV> native,
            int capacity0,
            int capacity1,
            Allocator allocator = Allocator.Persistent)
            where TK : struct
            where TV : struct
        {
            if (native.ShouldAllocate())
            {
                native = new NativeKeyArray2D<TK, TV>(capacity0, capacity1, allocator);
            }
            else if ((native.Capacity0 < capacity0) || (native.Capacity1 < capacity1))
            {
                native.Dispose();
                native = new NativeKeyArray2D<TK, TV>(
                    math.max(native.Capacity0, capacity0),
                    math.max(native.Capacity1, capacity1),
                    allocator
                );
            }
        }

        public static void EnsureCapacityAndLength<TK, TV>(
            this ref NativeKeyArray2D<TK, TV> native,
            int capacity0,
            int capacity1,
            int length0,
            int length1,
            Allocator allocator = Allocator.Persistent)
            where TK : struct
            where TV : struct
        {
            EnsureCapacity(ref native, capacity0, capacity1, allocator);

            native.Length0 = length0;
            native.Length1 = length1;
        }

        public static void EnsureCapacityAndLength<TK, TV>(
            this ref NativeKeyArray2D<TK, TV> native,
            int capacity0,
            int capacity1,
            Allocator allocator = Allocator.Persistent)
            where TK : struct
            where TV : struct
        {
            EnsureCapacityAndLength(ref native, capacity0, capacity1, capacity0, capacity1, allocator);
        }
    }
}
