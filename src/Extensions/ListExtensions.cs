#region

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.Profiling;
using Object = UnityEngine.Object;

#endregion

#if !ENABLE_IL2CPP && !NET_STANDARD_2_0
using System.Reflection.Emit;
#endif

namespace Appalachia.Core.Collections.Extensions
{
    public static class ListExtensions
    {

        private const string _PRF_PFX = nameof(ListExtensions) + ".";

        private static readonly ProfilerMarker _PRF_GetInternalArray = new ProfilerMarker(_PRF_PFX + nameof(GetInternalArray));
        public static T[] GetInternalArray<T>(this List<T> list)
        {
            using (_PRF_GetInternalArray.Auto())
            {

#if ENABLE_IL2CPP || NET_STANDARD_2_0
                return (T[]) ArrayAccessor<T>.AotGetter(list);
#else
            return ArrayAccessor<T>.Getter(list);
#endif
}
        }

        private static readonly ProfilerMarker _PRF_RemoveNulls = new ProfilerMarker(_PRF_PFX + nameof(RemoveNulls));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool RemoveNulls<T>(this IList<T> list)
        {
            using (_PRF_RemoveNulls.Auto())
            {
                var hadNulls = false;

                for (var i = list.Count - 1; i >= 0; i--)
                {
                    var obj = list[i];

                    if (obj == null)
                    {
                        hadNulls = true;
                        list.RemoveAt(i);
                    }
                }

                return hadNulls;
            }
        }

        private static readonly ProfilerMarker _PRF_RemoveUnityNulls = new ProfilerMarker(_PRF_PFX + nameof(RemoveUnityNulls));
        public static bool RemoveUnityNulls<T>(this IList<T> list)
            where T : Object
        {
            using (_PRF_RemoveUnityNulls.Auto())
            {
                var hadNulls = false;

                for (var i = list.Count - 1; i >= 0; i--)
                {
                    Object obj = list[i];

                    if (obj == null)
                    {
                        hadNulls = true;
                        list.RemoveAt(i);
                    }
                }

                return hadNulls;
            }
        }

        private static readonly ProfilerMarker _PRF_RemoveWhere = new ProfilerMarker(_PRF_PFX + nameof(RemoveWhere));
        public static void RemoveWhere<T>(this IList<T> list, Predicate<T> func)
        {
            using (_PRF_RemoveWhere.Auto())
            {
                for (var i = list.Count - 1; i >= 0; i--)
                {
                    if (func(list[i]))
                    {
                        list.RemoveAt(i);
                    }
                }
            }
        }

        private static class ArrayAccessor<T>
        {
#if ENABLE_IL2CPP || NET_STANDARD_2_0
            public static readonly FieldInfo FieldInfo;
            public static readonly Func<List<T>, object> AotGetter;
#else
            public static readonly Func<List<T>, T[]> Getter;
#endif
            private static readonly ProfilerMarker _PRF_ArrayAccessor = new ProfilerMarker(_PRF_PFX + nameof(ArrayAccessor<T>));

            static ArrayAccessor()
            {
                using (_PRF_ArrayAccessor.Auto())
                {
#if ENABLE_IL2CPP || NET_STANDARD_2_0
                    FieldInfo = typeof(List<T>).GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);
                    AotGetter = FieldInfo.GetValue;
#else
                // ReSharper disable once RedundantExplicitArrayCreation
                var dm = new DynamicMethod("get", MethodAttributes.Static | MethodAttributes.Public,
                    CallingConventions.Standard, typeof(T[]), new Type[] { typeof(List<T>) }, typeof(ArrayAccessor<T>),
                    true);
                var il = dm.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0); // Load List<T> argument
                // ReSharper disable once AssignNullToNotNullAttribute
                il.Emit(OpCodes.Ldfld,
                    typeof(List<T>).GetField("_items",
                        BindingFlags.NonPublic | BindingFlags.Instance)); // Replace argument by field
                il.Emit(OpCodes.Ret); // Return field
                Getter = (Func<List<T>, T[]>)dm.CreateDelegate(typeof(Func<List<T>, T[]>));
#endif
                }
            }
        }
    }
}
