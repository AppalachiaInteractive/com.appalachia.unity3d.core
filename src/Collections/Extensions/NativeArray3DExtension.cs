using Appalachia.Core.Collections.Native;
using Unity.Collections;
using Unity.Mathematics;

namespace Appalachia.Core.Collections.Extensions
{
    public static class NativeArray3DExtension
    {
        public static void EnsureCapacity<T>(
            this ref NativeArray3D<T> native,
            int capacity0,
            int capacity1,
            int capacity2,
            Allocator allocator = Allocator.Persistent)
            where T : struct
        {
            if (native.ShouldAllocate())
            {
                native = new NativeArray3D<T>(capacity0, capacity1, capacity2, allocator);
            }
            else if ((native.Capacity0 < capacity0) ||
                     (native.Capacity1 < capacity1) ||
                     (native.Capacity2 < capacity2))
            {
                native.Dispose();
                native = new NativeArray3D<T>(
                    math.max(native.Capacity0, capacity0),
                    math.max(native.Capacity1, capacity1),
                    math.max(native.Capacity2, capacity2),
                    allocator
                );
            }
        }

        public static void EnsureCapacityAndLength<T>(
            this ref NativeArray3D<T> native,
            int capacity0,
            int capacity1,
            int capacity2,
            int length0,
            int length1,
            int length2,
            Allocator allocator = Allocator.Persistent)
            where T : struct
        {
            EnsureCapacity(ref native, capacity0, capacity1, capacity2, allocator);

            native.Length0 = length0;
            native.Length1 = length1;
            native.Length2 = length2;
        }

        public static void EnsureCapacityAndLength<T>(
            this ref NativeArray3D<T> native,
            int capacity0,
            int capacity1,
            int capacity2,
            Allocator allocator = Allocator.Persistent)
            where T : struct
        {
            EnsureCapacityAndLength(
                ref native,
                capacity0,
                capacity1,
                capacity2,
                capacity0,
                capacity1,
                capacity2,
                allocator
            );
        }
    }
}
