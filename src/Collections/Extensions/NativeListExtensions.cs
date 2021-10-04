using Appalachia.Core.Collections.Native;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Appalachia.Core.Collections.Extensions
{
    public static class NativeListExtensions
    {
        public static unsafe void ClearMemory<T>(this NativeList<T> native)
            where T : unmanaged
        {
            UnsafeUtility.MemClear(
                native.GetUnsafePtr(),
                native.Length * UnsafeUtility.SizeOf<T>()
            );
        }

        public static void CompactMemory<T>(this NativeList<T> native)
            where T : unmanaged
        {
            native.Clear();
            native.Capacity = 0;
        }

        public static void EnsureCapacity<T>(
            this ref NativeList<T> native,
            int capacity,
            Allocator allocator = Allocator.Persistent)
            where T : unmanaged
        {
            if (native.ShouldAllocate())
            {
                native = new NativeList<T>(capacity, allocator);
            }

            if (native.Capacity < capacity)
            {
                native.Capacity = capacity;
            }
        }

        public static void EnsureCapacity<T>(
            this ref NativeList<T> native,
            int checkCapacity,
            int growCapacity,
            Allocator allocator = Allocator.Persistent)
            where T : unmanaged
        {
            if (native.ShouldAllocate())
            {
                native = new NativeList<T>(growCapacity, allocator);
            }

            if (native.Capacity < checkCapacity)
            {
                native.Capacity = growCapacity;
            }
        }

        public static void EnsureCapacityAndLength<T>(
            this ref NativeList<T> native,
            int checkCapacity,
            int growCapacity,
            int length,
            Allocator allocator = Allocator.Persistent)
            where T : unmanaged
        {
            EnsureCapacity(ref native, checkCapacity, growCapacity, allocator);

            native.Length = length;
        }

        public static void EnsureCapacityAndLength<T>(
            this ref NativeList<T> native,
            int length,
            Allocator allocator = Allocator.Persistent)
            where T : unmanaged
        {
            EnsureCapacityAndLength(ref native, length, length, length, allocator);
        }

        /*
        public static void ExactCapacity<T>(this ref NativeList<T> native, int capacity, Allocator allocator = Allocator.Persistent)
            where T: struct
        {
            if (native.ShouldAllocate())
            {
                native = new NativeList<T>(capacity, allocator);
            }

            if (native.Capacity != capacity)
            {
                native.Capacity = capacity;
            }
        }
        
        public static void ExactCapacityAndLength<T>(this ref NativeList<T> native, int capacity, Allocator allocator = Allocator.Persistent)
            where T: struct
        {
            ExactCapacity(ref native, capacity, allocator);
            
            native.Length = capacity;
        }*/
    }
}
