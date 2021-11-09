#region

using System;
using System.Diagnostics;
using Appalachia.Utility.Reflection.Extensions;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

#endregion

namespace Appalachia.Core.Collections.Native
{
    public static class SafetyUtility
    {
        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        [BurstDiscard]
        public static void CheckElementReadAccess(
            AtomicSafetyHandle safety,
            int index,
            int minimum,
            int maximum,
            int length,
            int capacity)
        {
            if ((index < minimum) || (index > maximum))
            {
                FailOutOfRangeError(index, minimum, maximum, length, capacity);
            }

            /*if (this.m_Safety.version == (*(int*) (void*) this.m_Safety.versionNode & -7))
                return;*/
            AtomicSafetyHandle.CheckReadAndThrow(safety);
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        [BurstDiscard]
        public static void CheckElementWriteAccess(
            AtomicSafetyHandle safety,
            int index,
            int minimum,
            int maximum,
            int length,
            int capacity)
        {
            if ((index < minimum) || (index > maximum))
            {
                FailOutOfRangeError(index, minimum, maximum, length, capacity);
            }

            /*if (this.m_Safety.version == (*(int*) (void*) this.m_Safety.versionNode & -6))
                return;*/
            AtomicSafetyHandle.CheckWriteAndThrow(safety);
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        [BurstDiscard]
        public static void CheckReadAccess(AtomicSafetyHandle safety)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckReadAndThrow(safety);
#endif
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        [BurstDiscard]
        public static void CheckWriteAccess(AtomicSafetyHandle safety)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndThrow(safety);
#endif
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        [BurstDiscard]
        public static void FailOutOfRangeError(int index, int minimum, int maximum, int length, int capacity)
        {
            if ((index < length) && ((minimum != 0) || (maximum != (capacity - 1))))
            {
                throw new IndexOutOfRangeException(
                    $"Index {index} is out of restricted IJobParallelFor range [{minimum}...{maximum}] in ReadWriteBuffer.\nReadWriteBuffers are restricted to only read & write the element at the job index. You can use double buffering strategies to avoid race conditions due to reading & writing in parallel to the same elements from a job."
                );
            }

            RequireIndexInBounds(index, length, capacity);
            RequireLengthWithinCapacity(length, capacity, -1);
        }

        [BurstDiscard]
        public static void IsUnmanagedAndThrow<TC, TE1, TE2>()
        {
            IsUnmanagedAndThrow<TC, TE1>();
            IsUnmanagedAndThrow<TC, TE2>();
        }

        [BurstDiscard]
        public static void IsUnmanagedAndThrow<TC, TE>()
        {
            if (!UnsafeUtility.IsValidNativeContainerElementType<TE>())
            {
                throw new InvalidOperationException(
                    $"{typeof(TE).GetReadableName()} used in {typeof(TC).GetReadableName()} must be unmanaged (contain no managed types) and cannot itself be a native container type."
                );
            }
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        [BurstDiscard]
        public static void RequireIndexInBounds(
            int index0,
            int length0,
            int capacity0,
            int index1,
            int length1,
            int capacity1)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if ((index0 < 0) ||
                (index0 >= length0) ||
                (index0 >= capacity0) ||
                (index1 < 0) ||
                (index1 >= length1) ||
                (index1 >= capacity1))
            {
                throw new IndexOutOfRangeException(
                    $"Index out of bounds.  Idx0 [{index0}] Len0: [{length0}] Cap0: [{capacity0}] / Idx1 [{index1}] Len1: [{length1}] Cap1: [{capacity1}] / "
                );
            }
#endif
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        [BurstDiscard]
        public static void RequireIndexInBounds(
            int index0,
            int length0,
            int capacity0,
            int index1,
            int length1,
            int capacity1,
            int index2,
            int length2,
            int capacity2)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if ((index0 < 0) ||
                (index0 >= length0) ||
                (index0 >= capacity0) ||
                (index1 < 0) ||
                (index1 >= length1) ||
                (index1 >= capacity1) ||
                (index2 < 0) ||
                (index2 >= length2) ||
                (index2 >= capacity2))
            {
                throw new IndexOutOfRangeException(
                    $"Index out of bounds.  Idx0 [{index0}] Len0: [{length0}] Cap0: [{capacity0}] / Idx1 [{index1}] Len1: [{length1}] Cap1: [{capacity1}] / Idx2 [{index2}] Len2: [{length2}] Cap2: [{capacity2}] / "
                );
            }
#endif
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        [BurstDiscard]
        public static void RequireIndexInBounds(int index, int length, int capacity)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if ((index < 0) || (index >= length) || (index >= capacity))
            {
                throw new IndexOutOfRangeException(
                    $"Index [{index}] out of bounds.  Length: [{length}]  Capacity: [{capacity}]."
                );
            }
#endif
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        [BurstDiscard]
        public static void RequireLengthWithinCapacity(int length, int capacity, int v)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if ((length < 0) || (length > capacity))
            {
                if (v >= 0)
                {
                    throw new IndexOutOfRangeException(
                        $"Length{v} [{length}] out of bounds.  Capacity{v}: [{capacity}]."
                    );
                }

                throw new IndexOutOfRangeException(
                    $"Length [{length}] out of bounds.  Capacity: [{capacity}]."
                );
            }
#endif
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        [BurstDiscard]
        public static void RequireValidAllocator<T>(Allocator allocator)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (!UnsafeUtility.IsValidAllocator(allocator))
            {
                throw new InvalidOperationException(
                    $"The {typeof(T).GetReadableName()} can not be Disposed because it was not allocated with a valid allocator."
                );
            }
#endif
        }
    }
}
