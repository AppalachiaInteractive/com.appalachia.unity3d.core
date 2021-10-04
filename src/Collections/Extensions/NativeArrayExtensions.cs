using System;
using System.Collections.Generic;
using Appalachia.Core.Collections.Native;
using Appalachia.Core.Extensions;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Appalachia.Core.Collections.Extensions
{
    public static class NativeArrayExtensions
    {
        public static unsafe void CopyToFast<T>(this NativeArray<T> native, T[] array)
            where T : unmanaged
        {
            if (array == null)
            {
                throw new NullReferenceException(nameof(array) + " is null");
            }

            var nativeLength = native.Length;
            if (array.Length < nativeLength)
            {
                throw new IndexOutOfRangeException(
                    nameof(array) + " is shorter than " + nameof(native)
                );
            }

            var byteLength = native.Length * UnsafeUtility.SizeOf<T>();
            var managedBuffer = UnsafeUtility.AddressOf(ref array[0]);
            var nativeBuffer = native.GetUnsafePtr();
            UnsafeUtility.MemCpy(managedBuffer, nativeBuffer, byteLength);
        }

        public static unsafe void CopyToFast<T>(this NativeSlice<T> nativeSlice, T[] array)
            where T : unmanaged
        {
            if (array == null)
            {
                throw new NullReferenceException(nameof(array) + " is null");
            }

            var nativeLength = nativeSlice.Length;
            if (array.Length < nativeLength)
            {
                throw new IndexOutOfRangeException(
                    nameof(array) + " is shorter than " + nameof(nativeSlice)
                );
            }

            var byteLength = nativeSlice.Length * UnsafeUtility.SizeOf<T>();
            var managedBuffer = UnsafeUtility.AddressOf(ref array[0]);
            var nativeBuffer = nativeSlice.GetUnsafePtr();
            UnsafeUtility.MemCpy(managedBuffer, nativeBuffer, byteLength);
        }

        public static unsafe void CopyToFast<T>(this NativeArray<T> native, T[,,] array)
            where T : unmanaged
        {
            if (array == null)
            {
                throw new NullReferenceException(nameof(array) + " is null");
            }

            var nativeLength = native.Length;
            var managedArrayLength = array.GetLength(0) * array.GetLength(1) * array.GetLength(2);
            if (managedArrayLength < nativeLength)
            {
                throw new IndexOutOfRangeException(
                    nameof(array) + " is shorter than " + nameof(native)
                );
            }

            var byteLength = native.Length * UnsafeUtility.SizeOf<T>();
            var managedBuffer = UnsafeUtility.AddressOf(ref array[0, 0, 0]);
            var nativeBuffer = native.GetUnsafePtr();
            UnsafeUtility.MemCpy(managedBuffer, nativeBuffer, byteLength);
        }

        public static unsafe void CopyFromFast<T>(this NativeArray<T> native, T[,] array)
            where T : unmanaged
        {
            if (array == null)
            {
                throw new NullReferenceException(nameof(array) + " is null");
            }

            var nativeLength = native.Length;
            var managedArrayLength = array.GetLength(0) * array.GetLength(1);
            if (managedArrayLength > nativeLength)
            {
                throw new IndexOutOfRangeException(
                    nameof(native) + " is shorter than " + nameof(array)
                );
            }

            var byteLength = managedArrayLength * UnsafeUtility.SizeOf<T>();
            var managedBuffer = UnsafeUtility.AddressOf(ref array[0, 0]);
            var nativeBuffer = native.GetUnsafePtr();
            UnsafeUtility.MemCpy(nativeBuffer, managedBuffer, byteLength);
        }

        public static unsafe void CopyFromFast<T>(this NativeArray<T> native, T[,,] array)
            where T : unmanaged
        {
            if (array == null)
            {
                throw new NullReferenceException(nameof(array) + " is null");
            }

            var nativeLength = native.Length;
            var managedArrayLength = array.GetLength(0) * array.GetLength(1) * array.GetLength(2);
            if (managedArrayLength > nativeLength)
            {
                throw new IndexOutOfRangeException(
                    nameof(native) + " is shorter than " + nameof(array)
                );
            }

            var byteLength = managedArrayLength * UnsafeUtility.SizeOf<T>();
            var managedBuffer = UnsafeUtility.AddressOf(ref array[0, 0, 0]);
            var nativeBuffer = native.GetUnsafePtr();
            UnsafeUtility.MemCpy(nativeBuffer, managedBuffer, byteLength);
        }

        public static unsafe void CopyFromFast<T>(this NativeArray<T> native, T[] array)
            where T : unmanaged
        {
            if (array == null)
            {
                throw new NullReferenceException(nameof(array) + " is null");
            }

            var nativeLength = native.Length;
            var managedArrayLength = array.GetLength(0);
            if (managedArrayLength > nativeLength)
            {
                throw new IndexOutOfRangeException(
                    nameof(native) + " is shorter than " + nameof(array)
                );
            }

            var byteLength = managedArrayLength * UnsafeUtility.SizeOf<T>();
            var managedBuffer = UnsafeUtility.AddressOf(ref array[0]);
            var nativeBuffer = native.GetUnsafePtr();

            UnsafeUtility.MemCpy(nativeBuffer, managedBuffer, byteLength);
        }

        public static unsafe void CopyFromFast<T>(this NativeArray<T> native, List<T> managedList)
            where T : unmanaged
        {
            if (managedList == null)
            {
                throw new NullReferenceException(nameof(managedList) + " is null");
            }

            var nativeLength = native.Length;
            var managedListLength = managedList.Count;
            var managedInternalArray = managedList.GetInternalArray();

            if (managedListLength > nativeLength)
            {
                throw new IndexOutOfRangeException(
                    nameof(native) + " is shorter than " + nameof(managedInternalArray)
                );
            }

            var byteLength = managedListLength * UnsafeUtility.SizeOf<T>();
            var managedBuffer = UnsafeUtility.AddressOf(ref managedInternalArray[0]);
            var nativeBuffer = native.GetUnsafePtr();

            UnsafeUtility.MemCpy(nativeBuffer, managedBuffer, byteLength);
        }

        public static void EnsureLengthAtLeast<T>(
            this ref NativeArray<T> native,
            int atLeast,
            int newLength,
            Allocator allocator)
            where T : unmanaged
        {
            if (native.ShouldAllocate())
            {
                native = new NativeArray<T>(newLength, allocator);
            }

            if (native.Length < atLeast)
            {
                native.SafeDispose();
                native = new NativeArray<T>(newLength, allocator);
            }
        }

        public static void EnsureLength<T>(
            this ref NativeArray<T> native,
            int length,
            Allocator allocator)
            where T : unmanaged
        {
            if (native.ShouldAllocate())
            {
                native = new NativeArray<T>(length, allocator);
            }

            if (native.Length != length)
            {
                native.SafeDispose();
                native = new NativeArray<T>(length, allocator);
            }
        }
    }
}
