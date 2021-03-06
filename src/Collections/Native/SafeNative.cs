#region

using System;
using Unity.Collections;
using Unity.Jobs;
using Unity.Profiling;

#endregion

namespace Appalachia.Core.Collections.Native
{
    public static class SafeNative
    {
        #region Profiling

        private const string _PRF_PFX = nameof(SafeNative) + ".";

        #endregion

        #region Safe Dispose

        private static readonly ProfilerMarker _PRF_IsSafe = new(_PRF_PFX + nameof(IsSafe));

        public static bool IsSafe<T>(this NativeList<T> native)
            where T : unmanaged
        {
            using (_PRF_IsSafe.Auto())
            {
                if (!native.IsCreated)
                {
                    return false;
                }

                try
                {
                    if (native.Length > 0)
                    {
                        var n = native[0];
                    }
                }
                catch (NullReferenceException)
                {
                    return false;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }

                return true;
            }
        }

        public static bool IsSafe<T>(this NativeArray<T> native)
            where T : unmanaged
        {
            using (_PRF_IsSafe.Auto())
            {
                if (!native.IsCreated)
                {
                    return false;
                }

                try
                {
                    if (native.Length > 0)
                    {
                        var n = native[0];
                    }
                }
                catch (NullReferenceException)
                {
                    return false;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }

                return true;
            }
        }

        private static readonly ProfilerMarker _PRF_SafeDispose = new(_PRF_PFX + nameof(SafeDisposeAll));

        public static void SafeDispose<T>(this NativeArray<T> native, JobHandle handle)
            where T : unmanaged
        {
            using (_PRF_SafeDispose.Auto())
            {
                try
                {
                    var l = native.Length;

                    if (l > 0)
                    {
                        var x = native[0];
                    }

                    native.Dispose(handle);
                }
                catch (NullReferenceException)
                {
                }
                catch (InvalidOperationException)
                {
                }
            }
        }

        public static void SafeDispose<T>(this AppaList<NativeList<T>> native)
            where T : unmanaged
        {
            using (_PRF_SafeDispose.Auto())
            {
                try
                {
                    if (native == null)
                    {
                        return;
                    }

                    var l = native.Count;

                    if (l > 0)
                    {
                        var x = native[0];
                    }

                    for (var i = 0; i < native.Count; i++)
                    {
                        native[i].SafeDispose();
                    }
                }
                catch (NullReferenceException)
                {
                }
                catch (InvalidOperationException)
                {
                }
            }
        }

        public static void SafeDispose<T>(this AppaList<NativeArray<T>> native)
            where T : unmanaged
        {
            using (_PRF_SafeDispose.Auto())
            {
                try
                {
                    if (native == null)
                    {
                        return;
                    }

                    var l = native.Count;

                    if (l > 0)
                    {
                        var x = native[0];
                    }

                    for (var i = 0; i < native.Count; i++)
                    {
                        native[i].SafeDispose();
                    }
                }
                catch (NullReferenceException)
                {
                }
                catch (InvalidOperationException)
                {
                }
            }
        }

        public static void SafeDispose<T>(this NativeList<T>[] native)
            where T : unmanaged
        {
            using (_PRF_SafeDispose.Auto())
            {
                try
                {
                    if (native == null)
                    {
                        return;
                    }

                    var l = native.Length;

                    if (l > 0)
                    {
                        var x = native[0];
                    }

                    for (var i = 0; i < native.Length; i++)
                    {
                        native[i].SafeDispose();
                    }
                }
                catch (NullReferenceException)
                {
                }
                catch (InvalidOperationException)
                {
                }
            }
        }

        public static void SafeDispose<T>(this NativeList<T>[] native, JobHandle handle)
            where T : unmanaged
        {
            using (_PRF_SafeDispose.Auto())
            {
                try
                {
                    var l = native.Length;

                    if (l > 0)
                    {
                        var x = native[0];
                    }

                    for (var i = 0; i < native.Length; i++)
                    {
                        native[i].Dispose(handle);
                    }
                }
                catch (NullReferenceException)
                {
                }
                catch (InvalidOperationException)
                {
                }
            }
        }

        public static void SafeDispose<T>(this NativeList<T> native, JobHandle handle)
            where T : unmanaged
        {
            using (_PRF_SafeDispose.Auto())
            {
                try
                {
                    var l = native.Length;

                    if (l > 0)
                    {
                        var x = native[0];
                    }

                    native.Dispose(handle);
                }
                catch (NullReferenceException)
                {
                }
                catch (InvalidOperationException)
                {
                }
            }
        }

        private static readonly ProfilerMarker _PRF_SafeDisposeAll = new(_PRF_PFX + nameof(SafeDisposeAll));

        public static void SafeDisposeAll(this NativeList<JobHandle> native)
        {
            using (_PRF_SafeDisposeAll.Auto())
            {
                try
                {
                    JobHandle.CompleteAll(native);
                }
                catch (NullReferenceException)
                {
                }
                catch (InvalidOperationException)
                {
                }

                try
                {
                    native.Dispose();
                }
                catch (NullReferenceException)
                {
                }
                catch (InvalidOperationException)
                {
                }
            }
        }

        public static void SafeDispose(this IDisposable native)
        {
            using (_PRF_SafeDispose.Auto())
            {
                try
                {
                    native.Dispose();
                }
                catch (NullReferenceException)
                {
                }
                catch (InvalidOperationException)
                {
                }
            }
        }

        private static readonly ProfilerMarker _PRF_IsDisposed = new(_PRF_PFX + nameof(IsDisposed));

        public static bool IsDisposed<T>(this INativeList<T> native)
            where T : unmanaged
        {
            using (_PRF_IsDisposed.Auto())
            {
                try
                {
                    var x = native.Capacity;

                    return false;
                }

                catch (InvalidOperationException)
                {
                    return true;
                }
            }
        }

        #endregion

        #region Safe Check

        private static readonly ProfilerMarker _PRF_ShouldAllocate = new(_PRF_PFX + nameof(ShouldAllocate));

        public static bool ShouldAllocate<T>(this NativeList<T> native)
            where T : unmanaged
        {
            using (_PRF_ShouldAllocate.Auto())
            {
                try
                {
                    if (!native.IsCreated)
                    {
                        return true;
                    }

                    var x = native.Length;
                    return false;
                }
                catch (NullReferenceException)
                {
                    return true;
                }
                catch (InvalidOperationException)
                {
                    return true;
                }
            }
        }

        public static bool ShouldAllocate<T>(this NativeArray<T> native)
            where T : unmanaged
        {
            using (_PRF_ShouldAllocate.Auto())
            {
                try
                {
                    if (!native.IsCreated)
                    {
                        return true;
                    }

                    if (native.Length > 0)
                    {
                        var item = native[0];

                        return false;
                    }

                    return false;
                }
                catch (NullReferenceException)
                {
                    return true;
                }
                catch (InvalidOperationException)
                {
                    return true;
                }
            }
        }

        public static bool ShouldAllocate<T>(this NativeArray2D<T> native)
            where T : unmanaged
        {
            using (_PRF_ShouldAllocate.Auto())
            {
                try
                {
                    if (!native.IsCreated)
                    {
                        return true;
                    }

                    if (native.TotalLength > 0)
                    {
                        var item = native[0, 0];

                        return false;
                    }

                    return false;
                }
                catch (NullReferenceException)
                {
                    return true;
                }
                catch (InvalidOperationException)
                {
                    return true;
                }
            }
        }

        public static bool ShouldAllocate<TK, TV>(this NativeHashMap<TK, TV> native)
            where TK : unmanaged, IEquatable<TK>
            where TV : unmanaged
        {
            using (_PRF_ShouldAllocate.Auto())
            {
                try
                {
                    if (!native.IsCreated)
                    {
                        return true;
                    }

                    var x = native.Capacity;
                    return false;
                }
                catch (NullReferenceException)
                {
                    return true;
                }
                catch (InvalidOperationException)
                {
                    return true;
                }
            }
        }

        #endregion

        #region Safe Clear

        private static readonly ProfilerMarker _PRF_SafeClear = new(_PRF_PFX + nameof(SafeClear));

        public static void SafeClear<T>(ref NativeList<T> list)
            where T : unmanaged
        {
            using (_PRF_SafeClear.Auto())
            {
                try
                {
                    list.Clear();
                }
                catch (InvalidOperationException)
                {
                    list = new NativeList<T>(Allocator.Persistent);
                }
            }
        }

        public static void SafeClear<T0, T1>(ref NativeList<T0> d0, ref NativeList<T1> d1)
            where T0 : unmanaged
            where T1 : unmanaged
        {
            SafeClear(ref d0);
            SafeClear(ref d1);
        }

        public static void SafeClear<T0, T1, T2>(
            ref NativeList<T0> d0,
            ref NativeList<T1> d1,
            ref NativeList<T2> d2)
            where T0 : unmanaged
            where T1 : unmanaged
            where T2 : unmanaged
        {
            SafeClear(ref d0);
            SafeClear(ref d1);
            SafeClear(ref d2);
        }

        public static void SafeClear<T0, T1, T2, T3>(
            ref NativeList<T0> d0,
            ref NativeList<T1> d1,
            ref NativeList<T2> d2,
            ref NativeList<T3> d3)
            where T0 : unmanaged
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            SafeClear(ref d0);
            SafeClear(ref d1);
            SafeClear(ref d2);
            SafeClear(ref d3);
        }

        public static void SafeClear<T0, T1, T2, T3, T4>(
            ref NativeList<T0> d0,
            ref NativeList<T1> d1,
            ref NativeList<T2> d2,
            ref NativeList<T3> d3,
            ref NativeList<T4> d4)
            where T0 : unmanaged
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            SafeClear(ref d0);
            SafeClear(ref d1);
            SafeClear(ref d2);
            SafeClear(ref d3);
            SafeClear(ref d4);
        }

        public static void SafeClear<T0, T1, T2, T3, T4, T5>(
            ref NativeList<T0> d0,
            ref NativeList<T1> d1,
            ref NativeList<T2> d2,
            ref NativeList<T3> d3,
            ref NativeList<T4> d4,
            ref NativeList<T5> d5)
            where T0 : unmanaged
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            SafeClear(ref d0);
            SafeClear(ref d1);
            SafeClear(ref d2);
            SafeClear(ref d3);
            SafeClear(ref d4);
            SafeClear(ref d5);
        }

        public static void SafeClear<T0, T1, T2, T3, T4, T5, T6>(
            ref NativeList<T0> d0,
            ref NativeList<T1> d1,
            ref NativeList<T2> d2,
            ref NativeList<T3> d3,
            ref NativeList<T4> d4,
            ref NativeList<T5> d5,
            ref NativeList<T6> d6)
            where T0 : unmanaged
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
        {
            SafeClear(ref d0);
            SafeClear(ref d1);
            SafeClear(ref d2);
            SafeClear(ref d3);
            SafeClear(ref d4);
            SafeClear(ref d5);
            SafeClear(ref d6);
        }

        public static void SafeClear<T0, T1, T2, T3, T4, T5, T6, T7>(
            ref NativeList<T0> d0,
            ref NativeList<T1> d1,
            ref NativeList<T2> d2,
            ref NativeList<T3> d3,
            ref NativeList<T4> d4,
            ref NativeList<T5> d5,
            ref NativeList<T6> d6,
            ref NativeList<T7> d7)
            where T0 : unmanaged
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
            where T7 : unmanaged
        {
            SafeClear(ref d0);
            SafeClear(ref d1);
            SafeClear(ref d2);
            SafeClear(ref d3);
            SafeClear(ref d4);
            SafeClear(ref d5);
            SafeClear(ref d6);
            SafeClear(ref d7);
        }

        public static void SafeClear<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
            ref NativeList<T0> d0,
            ref NativeList<T1> d1,
            ref NativeList<T2> d2,
            ref NativeList<T3> d3,
            ref NativeList<T4> d4,
            ref NativeList<T5> d5,
            ref NativeList<T6> d6,
            ref NativeList<T7> d7,
            ref NativeList<T8> d8)
            where T0 : unmanaged
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
            where T7 : unmanaged
            where T8 : unmanaged
        {
            SafeClear(ref d0);
            SafeClear(ref d1);
            SafeClear(ref d2);
            SafeClear(ref d3);
            SafeClear(ref d4);
            SafeClear(ref d5);
            SafeClear(ref d6);
            SafeClear(ref d7);
            SafeClear(ref d8);
        }

        public static void SafeClear<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            ref NativeList<T0> d0,
            ref NativeList<T1> d1,
            ref NativeList<T2> d2,
            ref NativeList<T3> d3,
            ref NativeList<T4> d4,
            ref NativeList<T5> d5,
            ref NativeList<T6> d6,
            ref NativeList<T7> d7,
            ref NativeList<T8> d8,
            ref NativeList<T9> d9)
            where T0 : unmanaged
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
            where T7 : unmanaged
            where T8 : unmanaged
            where T9 : unmanaged
        {
            SafeClear(ref d0);
            SafeClear(ref d1);
            SafeClear(ref d2);
            SafeClear(ref d3);
            SafeClear(ref d4);
            SafeClear(ref d5);
            SafeClear(ref d6);
            SafeClear(ref d7);
            SafeClear(ref d8);
            SafeClear(ref d9);
        }

        public static void SafeClear<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            ref NativeList<T0> d0,
            ref NativeList<T1> d1,
            ref NativeList<T2> d2,
            ref NativeList<T3> d3,
            ref NativeList<T4> d4,
            ref NativeList<T5> d5,
            ref NativeList<T6> d6,
            ref NativeList<T7> d7,
            ref NativeList<T8> d8,
            ref NativeList<T9> d9,
            ref NativeList<T10> d10)
            where T0 : unmanaged
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
            where T7 : unmanaged
            where T8 : unmanaged
            where T9 : unmanaged
            where T10 : unmanaged
        {
            SafeClear(ref d0);
            SafeClear(ref d1);
            SafeClear(ref d2);
            SafeClear(ref d3);
            SafeClear(ref d4);
            SafeClear(ref d5);
            SafeClear(ref d6);
            SafeClear(ref d7);
            SafeClear(ref d8);
            SafeClear(ref d9);
            SafeClear(ref d10);
        }

        public static void SafeClear<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            ref NativeList<T0> d0,
            ref NativeList<T1> d1,
            ref NativeList<T2> d2,
            ref NativeList<T3> d3,
            ref NativeList<T4> d4,
            ref NativeList<T5> d5,
            ref NativeList<T6> d6,
            ref NativeList<T7> d7,
            ref NativeList<T8> d8,
            ref NativeList<T9> d9,
            ref NativeList<T10> d10,
            ref NativeList<T11> d11)
            where T0 : unmanaged
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
            where T7 : unmanaged
            where T8 : unmanaged
            where T9 : unmanaged
            where T10 : unmanaged
            where T11 : unmanaged
        {
            SafeClear(ref d0);
            SafeClear(ref d1);
            SafeClear(ref d2);
            SafeClear(ref d3);
            SafeClear(ref d4);
            SafeClear(ref d5);
            SafeClear(ref d6);
            SafeClear(ref d7);
            SafeClear(ref d8);
            SafeClear(ref d9);
            SafeClear(ref d10);
            SafeClear(ref d11);
        }

        public static void SafeClear<T00, T01, T02, T03, T04, T05, T06, T07, T08, T09, T10, T11, T12>(
            ref NativeList<T00> d00,
            ref NativeList<T01> d01,
            ref NativeList<T02> d02,
            ref NativeList<T03> d03,
            ref NativeList<T04> d04,
            ref NativeList<T05> d05,
            ref NativeList<T06> d06,
            ref NativeList<T07> d07,
            ref NativeList<T08> d08,
            ref NativeList<T09> d09,
            ref NativeList<T10> d10,
            ref NativeList<T11> d11,
            ref NativeList<T12> d12)
            where T00 : unmanaged
            where T01 : unmanaged
            where T02 : unmanaged
            where T03 : unmanaged
            where T04 : unmanaged
            where T05 : unmanaged
            where T06 : unmanaged
            where T07 : unmanaged
            where T08 : unmanaged
            where T09 : unmanaged
            where T10 : unmanaged
            where T11 : unmanaged
            where T12 : unmanaged
        {
            SafeClear(ref d00);
            SafeClear(ref d01);
            SafeClear(ref d02);
            SafeClear(ref d03);
            SafeClear(ref d04);
            SafeClear(ref d05);
            SafeClear(ref d06);
            SafeClear(ref d07);
            SafeClear(ref d08);
            SafeClear(ref d09);
            SafeClear(ref d10);
            SafeClear(ref d11);
            SafeClear(ref d12);
        }

        public static void SafeClear<T00, T01, T02, T03, T04, T05, T06, T07, T08, T09, T10, T11, T12, T13>(
            ref NativeList<T00> d00,
            ref NativeList<T01> d01,
            ref NativeList<T02> d02,
            ref NativeList<T03> d03,
            ref NativeList<T04> d04,
            ref NativeList<T05> d05,
            ref NativeList<T06> d06,
            ref NativeList<T07> d07,
            ref NativeList<T08> d08,
            ref NativeList<T09> d09,
            ref NativeList<T10> d10,
            ref NativeList<T11> d11,
            ref NativeList<T12> d12,
            ref NativeList<T13> d13)
            where T00 : unmanaged
            where T01 : unmanaged
            where T02 : unmanaged
            where T03 : unmanaged
            where T04 : unmanaged
            where T05 : unmanaged
            where T06 : unmanaged
            where T07 : unmanaged
            where T08 : unmanaged
            where T09 : unmanaged
            where T10 : unmanaged
            where T11 : unmanaged
            where T12 : unmanaged
            where T13 : unmanaged
        {
            SafeClear(ref d00);
            SafeClear(ref d01);
            SafeClear(ref d02);
            SafeClear(ref d03);
            SafeClear(ref d04);
            SafeClear(ref d05);
            SafeClear(ref d06);
            SafeClear(ref d07);
            SafeClear(ref d08);
            SafeClear(ref d09);
            SafeClear(ref d10);
            SafeClear(ref d11);
            SafeClear(ref d12);
            SafeClear(ref d13);
        }

        public static void
            SafeClear<T00, T01, T02, T03, T04, T05, T06, T07, T08, T09, T10, T11, T12, T13, T14>(
                ref NativeList<T00> d00,
                ref NativeList<T01> d01,
                ref NativeList<T02> d02,
                ref NativeList<T03> d03,
                ref NativeList<T04> d04,
                ref NativeList<T05> d05,
                ref NativeList<T06> d06,
                ref NativeList<T07> d07,
                ref NativeList<T08> d08,
                ref NativeList<T09> d09,
                ref NativeList<T10> d10,
                ref NativeList<T11> d11,
                ref NativeList<T12> d12,
                ref NativeList<T13> d13,
                ref NativeList<T14> d14)
            where T00 : unmanaged
            where T01 : unmanaged
            where T02 : unmanaged
            where T03 : unmanaged
            where T04 : unmanaged
            where T05 : unmanaged
            where T06 : unmanaged
            where T07 : unmanaged
            where T08 : unmanaged
            where T09 : unmanaged
            where T10 : unmanaged
            where T11 : unmanaged
            where T12 : unmanaged
            where T13 : unmanaged
            where T14 : unmanaged
        {
            SafeClear(ref d00);
            SafeClear(ref d01);
            SafeClear(ref d02);
            SafeClear(ref d03);
            SafeClear(ref d04);
            SafeClear(ref d05);
            SafeClear(ref d06);
            SafeClear(ref d07);
            SafeClear(ref d08);
            SafeClear(ref d09);
            SafeClear(ref d10);
            SafeClear(ref d11);
            SafeClear(ref d12);
            SafeClear(ref d13);
            SafeClear(ref d14);
        }

        public static void
            SafeClear<T00, T01, T02, T03, T04, T05, T06, T07, T08, T09, T10, T11, T12, T13, T14, T15>(
                ref NativeList<T00> d00,
                ref NativeList<T01> d01,
                ref NativeList<T02> d02,
                ref NativeList<T03> d03,
                ref NativeList<T04> d04,
                ref NativeList<T05> d05,
                ref NativeList<T06> d06,
                ref NativeList<T07> d07,
                ref NativeList<T08> d08,
                ref NativeList<T09> d09,
                ref NativeList<T10> d10,
                ref NativeList<T11> d11,
                ref NativeList<T12> d12,
                ref NativeList<T13> d13,
                ref NativeList<T14> d14,
                ref NativeList<T15> d15)
            where T00 : unmanaged
            where T01 : unmanaged
            where T02 : unmanaged
            where T03 : unmanaged
            where T04 : unmanaged
            where T05 : unmanaged
            where T06 : unmanaged
            where T07 : unmanaged
            where T08 : unmanaged
            where T09 : unmanaged
            where T10 : unmanaged
            where T11 : unmanaged
            where T12 : unmanaged
            where T13 : unmanaged
            where T14 : unmanaged
            where T15 : unmanaged
        {
            SafeClear(ref d00);
            SafeClear(ref d01);
            SafeClear(ref d02);
            SafeClear(ref d03);
            SafeClear(ref d04);
            SafeClear(ref d05);
            SafeClear(ref d06);
            SafeClear(ref d07);
            SafeClear(ref d08);
            SafeClear(ref d09);
            SafeClear(ref d10);
            SafeClear(ref d11);
            SafeClear(ref d12);
            SafeClear(ref d13);
            SafeClear(ref d14);
            SafeClear(ref d15);
        }

        public static void
            SafeClear<T00, T01, T02, T03, T04, T05, T06, T07, T08, T09, T10, T11, T12, T13, T14, T15, T16>(
                ref NativeList<T00> d00,
                ref NativeList<T01> d01,
                ref NativeList<T02> d02,
                ref NativeList<T03> d03,
                ref NativeList<T04> d04,
                ref NativeList<T05> d05,
                ref NativeList<T06> d06,
                ref NativeList<T07> d07,
                ref NativeList<T08> d08,
                ref NativeList<T09> d09,
                ref NativeList<T10> d10,
                ref NativeList<T11> d11,
                ref NativeList<T12> d12,
                ref NativeList<T13> d13,
                ref NativeList<T14> d14,
                ref NativeList<T15> d15,
                ref NativeList<T16> d16)
            where T00 : unmanaged
            where T01 : unmanaged
            where T02 : unmanaged
            where T03 : unmanaged
            where T04 : unmanaged
            where T05 : unmanaged
            where T06 : unmanaged
            where T07 : unmanaged
            where T08 : unmanaged
            where T09 : unmanaged
            where T10 : unmanaged
            where T11 : unmanaged
            where T12 : unmanaged
            where T13 : unmanaged
            where T14 : unmanaged
            where T15 : unmanaged
            where T16 : unmanaged
        {
            SafeClear(ref d00);
            SafeClear(ref d01);
            SafeClear(ref d02);
            SafeClear(ref d03);
            SafeClear(ref d04);
            SafeClear(ref d05);
            SafeClear(ref d06);
            SafeClear(ref d07);
            SafeClear(ref d08);
            SafeClear(ref d09);
            SafeClear(ref d10);
            SafeClear(ref d11);
            SafeClear(ref d12);
            SafeClear(ref d13);
            SafeClear(ref d14);
            SafeClear(ref d15);
            SafeClear(ref d16);
        }

        public static void
            SafeClear<T00, T01, T02, T03, T04, T05, T06, T07, T08, T09, T10, T11, T12, T13, T14, T15, T16,
                      T17>(
                ref NativeList<T00> d00,
                ref NativeList<T01> d01,
                ref NativeList<T02> d02,
                ref NativeList<T03> d03,
                ref NativeList<T04> d04,
                ref NativeList<T05> d05,
                ref NativeList<T06> d06,
                ref NativeList<T07> d07,
                ref NativeList<T08> d08,
                ref NativeList<T09> d09,
                ref NativeList<T10> d10,
                ref NativeList<T11> d11,
                ref NativeList<T12> d12,
                ref NativeList<T13> d13,
                ref NativeList<T14> d14,
                ref NativeList<T15> d15,
                ref NativeList<T16> d16,
                ref NativeList<T17> d17)
            where T00 : unmanaged
            where T01 : unmanaged
            where T02 : unmanaged
            where T03 : unmanaged
            where T04 : unmanaged
            where T05 : unmanaged
            where T06 : unmanaged
            where T07 : unmanaged
            where T08 : unmanaged
            where T09 : unmanaged
            where T10 : unmanaged
            where T11 : unmanaged
            where T12 : unmanaged
            where T13 : unmanaged
            where T14 : unmanaged
            where T15 : unmanaged
            where T16 : unmanaged
            where T17 : unmanaged
        {
            SafeClear(ref d00);
            SafeClear(ref d01);
            SafeClear(ref d02);
            SafeClear(ref d03);
            SafeClear(ref d04);
            SafeClear(ref d05);
            SafeClear(ref d06);
            SafeClear(ref d07);
            SafeClear(ref d08);
            SafeClear(ref d09);
            SafeClear(ref d10);
            SafeClear(ref d11);
            SafeClear(ref d12);
            SafeClear(ref d13);
            SafeClear(ref d14);
            SafeClear(ref d15);
            SafeClear(ref d16);
            SafeClear(ref d17);
        }

        public static void
            SafeClear<T00, T01, T02, T03, T04, T05, T06, T07, T08, T09, T10, T11, T12, T13, T14, T15, T16,
                      T17, T18>(
                ref NativeList<T00> d00,
                ref NativeList<T01> d01,
                ref NativeList<T02> d02,
                ref NativeList<T03> d03,
                ref NativeList<T04> d04,
                ref NativeList<T05> d05,
                ref NativeList<T06> d06,
                ref NativeList<T07> d07,
                ref NativeList<T08> d08,
                ref NativeList<T09> d09,
                ref NativeList<T10> d10,
                ref NativeList<T11> d11,
                ref NativeList<T12> d12,
                ref NativeList<T13> d13,
                ref NativeList<T14> d14,
                ref NativeList<T15> d15,
                ref NativeList<T16> d16,
                ref NativeList<T17> d17,
                ref NativeList<T18> d18)
            where T00 : unmanaged
            where T01 : unmanaged
            where T02 : unmanaged
            where T03 : unmanaged
            where T04 : unmanaged
            where T05 : unmanaged
            where T06 : unmanaged
            where T07 : unmanaged
            where T08 : unmanaged
            where T09 : unmanaged
            where T10 : unmanaged
            where T11 : unmanaged
            where T12 : unmanaged
            where T13 : unmanaged
            where T14 : unmanaged
            where T15 : unmanaged
            where T16 : unmanaged
            where T17 : unmanaged
            where T18 : unmanaged
        {
            SafeClear(ref d00);
            SafeClear(ref d01);
            SafeClear(ref d02);
            SafeClear(ref d03);
            SafeClear(ref d04);
            SafeClear(ref d05);
            SafeClear(ref d06);
            SafeClear(ref d07);
            SafeClear(ref d08);
            SafeClear(ref d09);
            SafeClear(ref d10);
            SafeClear(ref d11);
            SafeClear(ref d12);
            SafeClear(ref d13);
            SafeClear(ref d14);
            SafeClear(ref d15);
            SafeClear(ref d16);
            SafeClear(ref d17);
            SafeClear(ref d18);
        }

        public static void
            SafeClear<T00, T01, T02, T03, T04, T05, T06, T07, T08, T09, T10, T11, T12, T13, T14, T15, T16,
                      T17, T18, T19>(
                ref NativeList<T00> d00,
                ref NativeList<T01> d01,
                ref NativeList<T02> d02,
                ref NativeList<T03> d03,
                ref NativeList<T04> d04,
                ref NativeList<T05> d05,
                ref NativeList<T06> d06,
                ref NativeList<T07> d07,
                ref NativeList<T08> d08,
                ref NativeList<T09> d09,
                ref NativeList<T10> d10,
                ref NativeList<T11> d11,
                ref NativeList<T12> d12,
                ref NativeList<T13> d13,
                ref NativeList<T14> d14,
                ref NativeList<T15> d15,
                ref NativeList<T16> d16,
                ref NativeList<T17> d17,
                ref NativeList<T18> d18,
                ref NativeList<T19> d19)
            where T00 : unmanaged
            where T01 : unmanaged
            where T02 : unmanaged
            where T03 : unmanaged
            where T04 : unmanaged
            where T05 : unmanaged
            where T06 : unmanaged
            where T07 : unmanaged
            where T08 : unmanaged
            where T09 : unmanaged
            where T10 : unmanaged
            where T11 : unmanaged
            where T12 : unmanaged
            where T13 : unmanaged
            where T14 : unmanaged
            where T15 : unmanaged
            where T16 : unmanaged
            where T17 : unmanaged
            where T18 : unmanaged
            where T19 : unmanaged
        {
            SafeClear(ref d00);
            SafeClear(ref d01);
            SafeClear(ref d02);
            SafeClear(ref d03);
            SafeClear(ref d04);
            SafeClear(ref d05);
            SafeClear(ref d06);
            SafeClear(ref d07);
            SafeClear(ref d08);
            SafeClear(ref d09);
            SafeClear(ref d10);
            SafeClear(ref d11);
            SafeClear(ref d12);
            SafeClear(ref d13);
            SafeClear(ref d14);
            SafeClear(ref d15);
            SafeClear(ref d16);
            SafeClear(ref d17);
            SafeClear(ref d18);
            SafeClear(ref d19);
        }

        public static void
            SafeClear<T00, T01, T02, T03, T04, T05, T06, T07, T08, T09, T10, T11, T12, T13, T14, T15, T16,
                      T17, T18, T19, T20>(
                ref NativeList<T00> d00,
                ref NativeList<T01> d01,
                ref NativeList<T02> d02,
                ref NativeList<T03> d03,
                ref NativeList<T04> d04,
                ref NativeList<T05> d05,
                ref NativeList<T06> d06,
                ref NativeList<T07> d07,
                ref NativeList<T08> d08,
                ref NativeList<T09> d09,
                ref NativeList<T10> d10,
                ref NativeList<T11> d11,
                ref NativeList<T12> d12,
                ref NativeList<T13> d13,
                ref NativeList<T14> d14,
                ref NativeList<T15> d15,
                ref NativeList<T16> d16,
                ref NativeList<T17> d17,
                ref NativeList<T18> d18,
                ref NativeList<T19> d19,
                ref NativeList<T20> d20)
            where T00 : unmanaged
            where T01 : unmanaged
            where T02 : unmanaged
            where T03 : unmanaged
            where T04 : unmanaged
            where T05 : unmanaged
            where T06 : unmanaged
            where T07 : unmanaged
            where T08 : unmanaged
            where T09 : unmanaged
            where T10 : unmanaged
            where T11 : unmanaged
            where T12 : unmanaged
            where T13 : unmanaged
            where T14 : unmanaged
            where T15 : unmanaged
            where T16 : unmanaged
            where T17 : unmanaged
            where T18 : unmanaged
            where T19 : unmanaged
            where T20 : unmanaged
        {
            SafeClear(ref d00);
            SafeClear(ref d01);
            SafeClear(ref d02);
            SafeClear(ref d03);
            SafeClear(ref d04);
            SafeClear(ref d05);
            SafeClear(ref d06);
            SafeClear(ref d07);
            SafeClear(ref d08);
            SafeClear(ref d09);
            SafeClear(ref d10);
            SafeClear(ref d11);
            SafeClear(ref d12);
            SafeClear(ref d13);
            SafeClear(ref d14);
            SafeClear(ref d15);
            SafeClear(ref d16);
            SafeClear(ref d17);
            SafeClear(ref d18);
            SafeClear(ref d19);
            SafeClear(ref d20);
        }

        public static void
            SafeClear<T00, T01, T02, T03, T04, T05, T06, T07, T08, T09, T10, T11, T12, T13, T14, T15, T16,
                      T17, T18, T19, T20, T21>(
                ref NativeList<T00> d00,
                ref NativeList<T01> d01,
                ref NativeList<T02> d02,
                ref NativeList<T03> d03,
                ref NativeList<T04> d04,
                ref NativeList<T05> d05,
                ref NativeList<T06> d06,
                ref NativeList<T07> d07,
                ref NativeList<T08> d08,
                ref NativeList<T09> d09,
                ref NativeList<T10> d10,
                ref NativeList<T11> d11,
                ref NativeList<T12> d12,
                ref NativeList<T13> d13,
                ref NativeList<T14> d14,
                ref NativeList<T15> d15,
                ref NativeList<T16> d16,
                ref NativeList<T17> d17,
                ref NativeList<T18> d18,
                ref NativeList<T19> d19,
                ref NativeList<T20> d20,
                ref NativeList<T21> d21)
            where T00 : unmanaged
            where T01 : unmanaged
            where T02 : unmanaged
            where T03 : unmanaged
            where T04 : unmanaged
            where T05 : unmanaged
            where T06 : unmanaged
            where T07 : unmanaged
            where T08 : unmanaged
            where T09 : unmanaged
            where T10 : unmanaged
            where T11 : unmanaged
            where T12 : unmanaged
            where T13 : unmanaged
            where T14 : unmanaged
            where T15 : unmanaged
            where T16 : unmanaged
            where T17 : unmanaged
            where T18 : unmanaged
            where T19 : unmanaged
            where T20 : unmanaged
            where T21 : unmanaged
        {
            SafeClear(ref d00);
            SafeClear(ref d01);
            SafeClear(ref d02);
            SafeClear(ref d03);
            SafeClear(ref d04);
            SafeClear(ref d05);
            SafeClear(ref d06);
            SafeClear(ref d07);
            SafeClear(ref d08);
            SafeClear(ref d09);
            SafeClear(ref d10);
            SafeClear(ref d11);
            SafeClear(ref d12);
            SafeClear(ref d13);
            SafeClear(ref d14);
            SafeClear(ref d15);
            SafeClear(ref d16);
            SafeClear(ref d17);
            SafeClear(ref d18);
            SafeClear(ref d19);
            SafeClear(ref d20);
            SafeClear(ref d21);
        }

        public static void
            SafeClear<T00, T01, T02, T03, T04, T05, T06, T07, T08, T09, T10, T11, T12, T13, T14, T15, T16,
                      T17, T18, T19, T20, T21, T22>(
                ref NativeList<T00> d00,
                ref NativeList<T01> d01,
                ref NativeList<T02> d02,
                ref NativeList<T03> d03,
                ref NativeList<T04> d04,
                ref NativeList<T05> d05,
                ref NativeList<T06> d06,
                ref NativeList<T07> d07,
                ref NativeList<T08> d08,
                ref NativeList<T09> d09,
                ref NativeList<T10> d10,
                ref NativeList<T11> d11,
                ref NativeList<T12> d12,
                ref NativeList<T13> d13,
                ref NativeList<T14> d14,
                ref NativeList<T15> d15,
                ref NativeList<T16> d16,
                ref NativeList<T17> d17,
                ref NativeList<T18> d18,
                ref NativeList<T19> d19,
                ref NativeList<T20> d20,
                ref NativeList<T21> d21,
                ref NativeList<T22> d22)
            where T00 : unmanaged
            where T01 : unmanaged
            where T02 : unmanaged
            where T03 : unmanaged
            where T04 : unmanaged
            where T05 : unmanaged
            where T06 : unmanaged
            where T07 : unmanaged
            where T08 : unmanaged
            where T09 : unmanaged
            where T10 : unmanaged
            where T11 : unmanaged
            where T12 : unmanaged
            where T13 : unmanaged
            where T14 : unmanaged
            where T15 : unmanaged
            where T16 : unmanaged
            where T17 : unmanaged
            where T18 : unmanaged
            where T19 : unmanaged
            where T20 : unmanaged
            where T21 : unmanaged
            where T22 : unmanaged
        {
            SafeClear(ref d00);
            SafeClear(ref d01);
            SafeClear(ref d02);
            SafeClear(ref d03);
            SafeClear(ref d04);
            SafeClear(ref d05);
            SafeClear(ref d06);
            SafeClear(ref d07);
            SafeClear(ref d08);
            SafeClear(ref d09);
            SafeClear(ref d10);
            SafeClear(ref d11);
            SafeClear(ref d12);
            SafeClear(ref d13);
            SafeClear(ref d14);
            SafeClear(ref d15);
            SafeClear(ref d16);
            SafeClear(ref d17);
            SafeClear(ref d18);
            SafeClear(ref d19);
            SafeClear(ref d20);
            SafeClear(ref d21);
            SafeClear(ref d22);
        }

        public static void
            SafeClear<T00, T01, T02, T03, T04, T05, T06, T07, T08, T09, T10, T11, T12, T13, T14, T15, T16,
                      T17, T18, T19, T20, T21, T22, T23>(
                ref NativeList<T00> d00,
                ref NativeList<T01> d01,
                ref NativeList<T02> d02,
                ref NativeList<T03> d03,
                ref NativeList<T04> d04,
                ref NativeList<T05> d05,
                ref NativeList<T06> d06,
                ref NativeList<T07> d07,
                ref NativeList<T08> d08,
                ref NativeList<T09> d09,
                ref NativeList<T10> d10,
                ref NativeList<T11> d11,
                ref NativeList<T12> d12,
                ref NativeList<T13> d13,
                ref NativeList<T14> d14,
                ref NativeList<T15> d15,
                ref NativeList<T16> d16,
                ref NativeList<T17> d17,
                ref NativeList<T18> d18,
                ref NativeList<T19> d19,
                ref NativeList<T20> d20,
                ref NativeList<T21> d21,
                ref NativeList<T22> d22,
                ref NativeList<T23> d23)
            where T00 : unmanaged
            where T01 : unmanaged
            where T02 : unmanaged
            where T03 : unmanaged
            where T04 : unmanaged
            where T05 : unmanaged
            where T06 : unmanaged
            where T07 : unmanaged
            where T08 : unmanaged
            where T09 : unmanaged
            where T10 : unmanaged
            where T11 : unmanaged
            where T12 : unmanaged
            where T13 : unmanaged
            where T14 : unmanaged
            where T15 : unmanaged
            where T16 : unmanaged
            where T17 : unmanaged
            where T18 : unmanaged
            where T19 : unmanaged
            where T20 : unmanaged
            where T21 : unmanaged
            where T22 : unmanaged
            where T23 : unmanaged
        {
            SafeClear(ref d00);
            SafeClear(ref d01);
            SafeClear(ref d02);
            SafeClear(ref d03);
            SafeClear(ref d04);
            SafeClear(ref d05);
            SafeClear(ref d06);
            SafeClear(ref d07);
            SafeClear(ref d08);
            SafeClear(ref d09);
            SafeClear(ref d10);
            SafeClear(ref d11);
            SafeClear(ref d12);
            SafeClear(ref d13);
            SafeClear(ref d14);
            SafeClear(ref d15);
            SafeClear(ref d16);
            SafeClear(ref d17);
            SafeClear(ref d18);
            SafeClear(ref d19);
            SafeClear(ref d20);
            SafeClear(ref d21);
            SafeClear(ref d22);
            SafeClear(ref d23);
        }

        public static void
            SafeClear<T00, T01, T02, T03, T04, T05, T06, T07, T08, T09, T10, T11, T12, T13, T14, T15, T16,
                      T17, T18, T19, T20, T21, T22, T23, T24>(
                ref NativeList<T00> d00,
                ref NativeList<T01> d01,
                ref NativeList<T02> d02,
                ref NativeList<T03> d03,
                ref NativeList<T04> d04,
                ref NativeList<T05> d05,
                ref NativeList<T06> d06,
                ref NativeList<T07> d07,
                ref NativeList<T08> d08,
                ref NativeList<T09> d09,
                ref NativeList<T10> d10,
                ref NativeList<T11> d11,
                ref NativeList<T12> d12,
                ref NativeList<T13> d13,
                ref NativeList<T14> d14,
                ref NativeList<T15> d15,
                ref NativeList<T16> d16,
                ref NativeList<T17> d17,
                ref NativeList<T18> d18,
                ref NativeList<T19> d19,
                ref NativeList<T20> d20,
                ref NativeList<T21> d21,
                ref NativeList<T22> d22,
                ref NativeList<T23> d23,
                ref NativeList<T24> d24)
            where T00 : unmanaged
            where T01 : unmanaged
            where T02 : unmanaged
            where T03 : unmanaged
            where T04 : unmanaged
            where T05 : unmanaged
            where T06 : unmanaged
            where T07 : unmanaged
            where T08 : unmanaged
            where T09 : unmanaged
            where T10 : unmanaged
            where T11 : unmanaged
            where T12 : unmanaged
            where T13 : unmanaged
            where T14 : unmanaged
            where T15 : unmanaged
            where T16 : unmanaged
            where T17 : unmanaged
            where T18 : unmanaged
            where T19 : unmanaged
            where T20 : unmanaged
            where T21 : unmanaged
            where T22 : unmanaged
            where T23 : unmanaged
            where T24 : unmanaged
        {
            SafeClear(ref d00);
            SafeClear(ref d01);
            SafeClear(ref d02);
            SafeClear(ref d03);
            SafeClear(ref d04);
            SafeClear(ref d05);
            SafeClear(ref d06);
            SafeClear(ref d07);
            SafeClear(ref d08);
            SafeClear(ref d09);
            SafeClear(ref d10);
            SafeClear(ref d11);
            SafeClear(ref d12);
            SafeClear(ref d13);
            SafeClear(ref d14);
            SafeClear(ref d15);
            SafeClear(ref d16);
            SafeClear(ref d17);
            SafeClear(ref d18);
            SafeClear(ref d19);
            SafeClear(ref d20);
            SafeClear(ref d21);
            SafeClear(ref d22);
            SafeClear(ref d23);
            SafeClear(ref d24);
        }

        public static void
            SafeClear<T00, T01, T02, T03, T04, T05, T06, T07, T08, T09, T10, T11, T12, T13, T14, T15, T16,
                      T17, T18, T19, T20, T21, T22, T23, T24, T25>(
                ref NativeList<T00> d00,
                ref NativeList<T01> d01,
                ref NativeList<T02> d02,
                ref NativeList<T03> d03,
                ref NativeList<T04> d04,
                ref NativeList<T05> d05,
                ref NativeList<T06> d06,
                ref NativeList<T07> d07,
                ref NativeList<T08> d08,
                ref NativeList<T09> d09,
                ref NativeList<T10> d10,
                ref NativeList<T11> d11,
                ref NativeList<T12> d12,
                ref NativeList<T13> d13,
                ref NativeList<T14> d14,
                ref NativeList<T15> d15,
                ref NativeList<T16> d16,
                ref NativeList<T17> d17,
                ref NativeList<T18> d18,
                ref NativeList<T19> d19,
                ref NativeList<T20> d20,
                ref NativeList<T21> d21,
                ref NativeList<T22> d22,
                ref NativeList<T23> d23,
                ref NativeList<T24> d24,
                ref NativeList<T25> d25)
            where T00 : unmanaged
            where T01 : unmanaged
            where T02 : unmanaged
            where T03 : unmanaged
            where T04 : unmanaged
            where T05 : unmanaged
            where T06 : unmanaged
            where T07 : unmanaged
            where T08 : unmanaged
            where T09 : unmanaged
            where T10 : unmanaged
            where T11 : unmanaged
            where T12 : unmanaged
            where T13 : unmanaged
            where T14 : unmanaged
            where T15 : unmanaged
            where T16 : unmanaged
            where T17 : unmanaged
            where T18 : unmanaged
            where T19 : unmanaged
            where T20 : unmanaged
            where T21 : unmanaged
            where T22 : unmanaged
            where T23 : unmanaged
            where T24 : unmanaged
            where T25 : unmanaged
        {
            SafeClear(ref d00);
            SafeClear(ref d01);
            SafeClear(ref d02);
            SafeClear(ref d03);
            SafeClear(ref d04);
            SafeClear(ref d05);
            SafeClear(ref d06);
            SafeClear(ref d07);
            SafeClear(ref d08);
            SafeClear(ref d09);
            SafeClear(ref d10);
            SafeClear(ref d11);
            SafeClear(ref d12);
            SafeClear(ref d13);
            SafeClear(ref d14);
            SafeClear(ref d15);
            SafeClear(ref d16);
            SafeClear(ref d17);
            SafeClear(ref d18);
            SafeClear(ref d19);
            SafeClear(ref d20);
            SafeClear(ref d21);
            SafeClear(ref d22);
            SafeClear(ref d23);
            SafeClear(ref d24);
            SafeClear(ref d25);
        }

        public static void
            SafeClear<T00, T01, T02, T03, T04, T05, T06, T07, T08, T09, T10, T11, T12, T13, T14, T15, T16,
                      T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(
                ref NativeList<T00> d00,
                ref NativeList<T01> d01,
                ref NativeList<T02> d02,
                ref NativeList<T03> d03,
                ref NativeList<T04> d04,
                ref NativeList<T05> d05,
                ref NativeList<T06> d06,
                ref NativeList<T07> d07,
                ref NativeList<T08> d08,
                ref NativeList<T09> d09,
                ref NativeList<T10> d10,
                ref NativeList<T11> d11,
                ref NativeList<T12> d12,
                ref NativeList<T13> d13,
                ref NativeList<T14> d14,
                ref NativeList<T15> d15,
                ref NativeList<T16> d16,
                ref NativeList<T17> d17,
                ref NativeList<T18> d18,
                ref NativeList<T19> d19,
                ref NativeList<T20> d20,
                ref NativeList<T21> d21,
                ref NativeList<T22> d22,
                ref NativeList<T23> d23,
                ref NativeList<T24> d24,
                ref NativeList<T25> d25,
                ref NativeList<T26> d26)
            where T00 : unmanaged
            where T01 : unmanaged
            where T02 : unmanaged
            where T03 : unmanaged
            where T04 : unmanaged
            where T05 : unmanaged
            where T06 : unmanaged
            where T07 : unmanaged
            where T08 : unmanaged
            where T09 : unmanaged
            where T10 : unmanaged
            where T11 : unmanaged
            where T12 : unmanaged
            where T13 : unmanaged
            where T14 : unmanaged
            where T15 : unmanaged
            where T16 : unmanaged
            where T17 : unmanaged
            where T18 : unmanaged
            where T19 : unmanaged
            where T20 : unmanaged
            where T21 : unmanaged
            where T22 : unmanaged
            where T23 : unmanaged
            where T24 : unmanaged
            where T25 : unmanaged
            where T26 : unmanaged
        {
            SafeClear(ref d00);
            SafeClear(ref d01);
            SafeClear(ref d02);
            SafeClear(ref d03);
            SafeClear(ref d04);
            SafeClear(ref d05);
            SafeClear(ref d06);
            SafeClear(ref d07);
            SafeClear(ref d08);
            SafeClear(ref d09);
            SafeClear(ref d10);
            SafeClear(ref d11);
            SafeClear(ref d12);
            SafeClear(ref d13);
            SafeClear(ref d14);
            SafeClear(ref d15);
            SafeClear(ref d16);
            SafeClear(ref d17);
            SafeClear(ref d18);
            SafeClear(ref d19);
            SafeClear(ref d20);
            SafeClear(ref d21);
            SafeClear(ref d22);
            SafeClear(ref d23);
            SafeClear(ref d24);
            SafeClear(ref d25);
            SafeClear(ref d26);
        }

        public static void
            SafeClear<T00, T01, T02, T03, T04, T05, T06, T07, T08, T09, T10, T11, T12, T13, T14, T15, T16,
                      T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(
                ref NativeList<T00> d00,
                ref NativeList<T01> d01,
                ref NativeList<T02> d02,
                ref NativeList<T03> d03,
                ref NativeList<T04> d04,
                ref NativeList<T05> d05,
                ref NativeList<T06> d06,
                ref NativeList<T07> d07,
                ref NativeList<T08> d08,
                ref NativeList<T09> d09,
                ref NativeList<T10> d10,
                ref NativeList<T11> d11,
                ref NativeList<T12> d12,
                ref NativeList<T13> d13,
                ref NativeList<T14> d14,
                ref NativeList<T15> d15,
                ref NativeList<T16> d16,
                ref NativeList<T17> d17,
                ref NativeList<T18> d18,
                ref NativeList<T19> d19,
                ref NativeList<T20> d20,
                ref NativeList<T21> d21,
                ref NativeList<T22> d22,
                ref NativeList<T23> d23,
                ref NativeList<T24> d24,
                ref NativeList<T25> d25,
                ref NativeList<T26> d26,
                ref NativeList<T27> d27)
            where T00 : unmanaged
            where T01 : unmanaged
            where T02 : unmanaged
            where T03 : unmanaged
            where T04 : unmanaged
            where T05 : unmanaged
            where T06 : unmanaged
            where T07 : unmanaged
            where T08 : unmanaged
            where T09 : unmanaged
            where T10 : unmanaged
            where T11 : unmanaged
            where T12 : unmanaged
            where T13 : unmanaged
            where T14 : unmanaged
            where T15 : unmanaged
            where T16 : unmanaged
            where T17 : unmanaged
            where T18 : unmanaged
            where T19 : unmanaged
            where T20 : unmanaged
            where T21 : unmanaged
            where T22 : unmanaged
            where T23 : unmanaged
            where T24 : unmanaged
            where T25 : unmanaged
            where T26 : unmanaged
            where T27 : unmanaged
        {
            SafeClear(ref d00);
            SafeClear(ref d01);
            SafeClear(ref d02);
            SafeClear(ref d03);
            SafeClear(ref d04);
            SafeClear(ref d05);
            SafeClear(ref d06);
            SafeClear(ref d07);
            SafeClear(ref d08);
            SafeClear(ref d09);
            SafeClear(ref d10);
            SafeClear(ref d11);
            SafeClear(ref d12);
            SafeClear(ref d13);
            SafeClear(ref d14);
            SafeClear(ref d15);
            SafeClear(ref d16);
            SafeClear(ref d17);
            SafeClear(ref d18);
            SafeClear(ref d19);
            SafeClear(ref d20);
            SafeClear(ref d21);
            SafeClear(ref d22);
            SafeClear(ref d23);
            SafeClear(ref d24);
            SafeClear(ref d25);
            SafeClear(ref d26);
            SafeClear(ref d27);
        }

        public static void
            SafeClear<T00, T01, T02, T03, T04, T05, T06, T07, T08, T09, T10, T11, T12, T13, T14, T15, T16,
                      T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(
                ref NativeList<T00> d00,
                ref NativeList<T01> d01,
                ref NativeList<T02> d02,
                ref NativeList<T03> d03,
                ref NativeList<T04> d04,
                ref NativeList<T05> d05,
                ref NativeList<T06> d06,
                ref NativeList<T07> d07,
                ref NativeList<T08> d08,
                ref NativeList<T09> d09,
                ref NativeList<T10> d10,
                ref NativeList<T11> d11,
                ref NativeList<T12> d12,
                ref NativeList<T13> d13,
                ref NativeList<T14> d14,
                ref NativeList<T15> d15,
                ref NativeList<T16> d16,
                ref NativeList<T17> d17,
                ref NativeList<T18> d18,
                ref NativeList<T19> d19,
                ref NativeList<T20> d20,
                ref NativeList<T21> d21,
                ref NativeList<T22> d22,
                ref NativeList<T23> d23,
                ref NativeList<T24> d24,
                ref NativeList<T25> d25,
                ref NativeList<T26> d26,
                ref NativeList<T27> d27,
                ref NativeList<T28> d28)
            where T00 : unmanaged
            where T01 : unmanaged
            where T02 : unmanaged
            where T03 : unmanaged
            where T04 : unmanaged
            where T05 : unmanaged
            where T06 : unmanaged
            where T07 : unmanaged
            where T08 : unmanaged
            where T09 : unmanaged
            where T10 : unmanaged
            where T11 : unmanaged
            where T12 : unmanaged
            where T13 : unmanaged
            where T14 : unmanaged
            where T15 : unmanaged
            where T16 : unmanaged
            where T17 : unmanaged
            where T18 : unmanaged
            where T19 : unmanaged
            where T20 : unmanaged
            where T21 : unmanaged
            where T22 : unmanaged
            where T23 : unmanaged
            where T24 : unmanaged
            where T25 : unmanaged
            where T26 : unmanaged
            where T27 : unmanaged
            where T28 : unmanaged
        {
            SafeClear(ref d00);
            SafeClear(ref d01);
            SafeClear(ref d02);
            SafeClear(ref d03);
            SafeClear(ref d04);
            SafeClear(ref d05);
            SafeClear(ref d06);
            SafeClear(ref d07);
            SafeClear(ref d08);
            SafeClear(ref d09);
            SafeClear(ref d10);
            SafeClear(ref d11);
            SafeClear(ref d12);
            SafeClear(ref d13);
            SafeClear(ref d14);
            SafeClear(ref d15);
            SafeClear(ref d16);
            SafeClear(ref d17);
            SafeClear(ref d18);
            SafeClear(ref d19);
            SafeClear(ref d20);
            SafeClear(ref d21);
            SafeClear(ref d22);
            SafeClear(ref d23);
            SafeClear(ref d24);
            SafeClear(ref d25);
            SafeClear(ref d26);
            SafeClear(ref d27);
            SafeClear(ref d28);
        }

        public static void
            SafeClear<T00, T01, T02, T03, T04, T05, T06, T07, T08, T09, T10, T11, T12, T13, T14, T15, T16,
                      T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(
                ref NativeList<T00> d00,
                ref NativeList<T01> d01,
                ref NativeList<T02> d02,
                ref NativeList<T03> d03,
                ref NativeList<T04> d04,
                ref NativeList<T05> d05,
                ref NativeList<T06> d06,
                ref NativeList<T07> d07,
                ref NativeList<T08> d08,
                ref NativeList<T09> d09,
                ref NativeList<T10> d10,
                ref NativeList<T11> d11,
                ref NativeList<T12> d12,
                ref NativeList<T13> d13,
                ref NativeList<T14> d14,
                ref NativeList<T15> d15,
                ref NativeList<T16> d16,
                ref NativeList<T17> d17,
                ref NativeList<T18> d18,
                ref NativeList<T19> d19,
                ref NativeList<T20> d20,
                ref NativeList<T21> d21,
                ref NativeList<T22> d22,
                ref NativeList<T23> d23,
                ref NativeList<T24> d24,
                ref NativeList<T25> d25,
                ref NativeList<T26> d26,
                ref NativeList<T27> d27,
                ref NativeList<T28> d28,
                ref NativeList<T29> d29)
            where T00 : unmanaged
            where T01 : unmanaged
            where T02 : unmanaged
            where T03 : unmanaged
            where T04 : unmanaged
            where T05 : unmanaged
            where T06 : unmanaged
            where T07 : unmanaged
            where T08 : unmanaged
            where T09 : unmanaged
            where T10 : unmanaged
            where T11 : unmanaged
            where T12 : unmanaged
            where T13 : unmanaged
            where T14 : unmanaged
            where T15 : unmanaged
            where T16 : unmanaged
            where T17 : unmanaged
            where T18 : unmanaged
            where T19 : unmanaged
            where T20 : unmanaged
            where T21 : unmanaged
            where T22 : unmanaged
            where T23 : unmanaged
            where T24 : unmanaged
            where T25 : unmanaged
            where T26 : unmanaged
            where T27 : unmanaged
            where T28 : unmanaged
            where T29 : unmanaged
        {
            SafeClear(ref d00);
            SafeClear(ref d01);
            SafeClear(ref d02);
            SafeClear(ref d03);
            SafeClear(ref d04);
            SafeClear(ref d05);
            SafeClear(ref d06);
            SafeClear(ref d07);
            SafeClear(ref d08);
            SafeClear(ref d09);
            SafeClear(ref d10);
            SafeClear(ref d11);
            SafeClear(ref d12);
            SafeClear(ref d13);
            SafeClear(ref d14);
            SafeClear(ref d15);
            SafeClear(ref d16);
            SafeClear(ref d17);
            SafeClear(ref d18);
            SafeClear(ref d19);
            SafeClear(ref d20);
            SafeClear(ref d21);
            SafeClear(ref d22);
            SafeClear(ref d23);
            SafeClear(ref d24);
            SafeClear(ref d25);
            SafeClear(ref d26);
            SafeClear(ref d27);
            SafeClear(ref d28);
            SafeClear(ref d29);
        }

        #endregion
    }
}
