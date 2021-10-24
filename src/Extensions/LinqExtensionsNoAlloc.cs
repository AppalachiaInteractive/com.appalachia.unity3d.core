#region

using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

#endregion

namespace Appalachia.Core.Extensions
{
    public static partial class LinqExtensionsNoAlloc
    {
        private static readonly Vector2 Vector2_max = new(float.MaxValue, float.MaxValue);
        private static readonly Vector2 Vector2_min = new(float.MinValue, float.MinValue);

        private static readonly Vector3 Vector3_max = new(float.MaxValue, float.MaxValue, float.MaxValue);

        private static readonly Vector3 Vector3_min = new(float.MinValue, float.MinValue, float.MinValue);

        private static readonly Vector4 Vector4_max =
            new(float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue);

        private static readonly Vector4 Vector4_min =
            new(float.MinValue, float.MinValue, float.MinValue, float.MinValue);

        public static bool All_NoAlloc<T>(this T[] array, Predicate<T> pred)
        {
            var length = array.Length;
            for (var i = 0; i < length; i++)
            {
                var val = array[i];

                if (!pred(val))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Any_NoAlloc<T>(this T[] array, Predicate<T> pred)
        {
            var length = array.Length;
            for (var i = 0; i < length; i++)
            {
                var val = array[i];

                if (pred(val))
                {
                    return true;
                }
            }

            return true;
        }

        public static bool AtLeast_NoAlloc<T>(this T[] array, Predicate<T> pred, int atLeast)
        {
            var length = array.Length;
            var found = 0;

            for (var i = 0; i < length; i++)
            {
                var val = array[i];

                if (pred(val))
                {
                    found += 1;

                    if (found >= atLeast)
                    {
                        return true;
                    }
                }
            }

            return true;
        }

        public static bool AtMost_NoAlloc<T>(this T[] array, Predicate<T> pred, int atMost)
        {
            var length = array.Length;
            var found = 0;

            for (var i = 0; i < length; i++)
            {
                var val = array[i];

                if (pred(val))
                {
                    found += 1;

                    if (found >= atMost)
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public static bool None_NoAlloc<T>(this T[] array, Predicate<T> pred)
        {
            var length = array.Length;
            for (var i = 0; i < length; i++)
            {
                var val = array[i];

                if (pred(val))
                {
                    return false;
                }
            }

            return true;
        }

        public static Bounds Bounds_NoAlloc(this Vector3[] array)
        {
            var bounds = new Bounds
            {
                center = array.Center_NoAlloc(), min = array.Min_NoAlloc(), max = array.Max_NoAlloc()
            };

            return bounds;
        }

        public static double Average_NoAlloc<T>(this T[] array, Func<T, double> selector)
        {
            var sum = 0.0;

            for (var i = 0; i < array.Length; i++)
            {
                sum += selector(array[i]);
            }

            return sum / array.Length;
        }

        public static double Max_NoAlloc<T>(this T[] array, Func<T, double> selector)
        {
            var max = double.MinValue;

            for (var i = 0; i < array.Length; i++)
            {
                max = math.max(max, selector(array[i]));
            }

            return max;
        }

        public static double Min_NoAlloc<T>(this T[] array, Func<T, double> selector)
        {
            var min = double.MaxValue;

            for (var i = 0; i < array.Length; i++)
            {
                min = math.min(min, selector(array[i]));
            }

            return min;
        }

        public static double Sum_NoAlloc<T>(this T[] array, Func<T, double> selector)
        {
            var sum = 0.0;

            for (var i = 0; i < array.Length; i++)
            {
                sum += selector(array[i]);
            }

            return sum;
        }

        public static float Average_NoAlloc<T>(this T[] array, Func<T, float> selector)
        {
            var sum = 0f;

            for (var i = 0; i < array.Length; i++)
            {
                sum += selector(array[i]);
            }

            return sum / array.Length;
        }

        public static float Max_NoAlloc<T>(this T[] array, Func<T, float> selector)
        {
            var max = float.MinValue;

            for (var i = 0; i < array.Length; i++)
            {
                max = math.max(max, selector(array[i]));
            }

            return max;
        }

        public static float Min_NoAlloc<T>(this T[] array, Func<T, float> selector)
        {
            var min = float.MaxValue;

            for (var i = 0; i < array.Length; i++)
            {
                min = math.min(min, selector(array[i]));
            }

            return min;
        }

        public static float Sum_NoAlloc<T>(this T[] array, Func<T, float> selector)
        {
            var sum = 0f;

            for (var i = 0; i < array.Length; i++)
            {
                sum += selector(array[i]);
            }

            return sum;
        }

        public static int Average_NoAlloc<T>(this T[] array, Func<T, int> selector)
        {
            var sum = 0;

            for (var i = 0; i < array.Length; i++)
            {
                sum += selector(array[i]);
            }

            return sum / array.Length;
        }

        public static int Count_NoAlloc<T>(this T[] array, Predicate<T> pred)
        {
            var sum = 0;

            for (var i = 0; i < array.Length; i++)
            {
                sum += pred(array[i]) ? 1 : 0;
            }

            return sum;
        }

        public static int Max_NoAlloc<T>(this T[] array, Func<T, int> selector)
        {
            var max = int.MinValue;

            for (var i = 0; i < array.Length; i++)
            {
                max = math.max(max, selector(array[i]));
            }

            return max;
        }

        public static int Min_NoAlloc<T>(this T[] array, Func<T, int> selector)
        {
            var min = int.MaxValue;

            for (var i = 0; i < array.Length; i++)
            {
                min = math.min(min, selector(array[i]));
            }

            return min;
        }

        public static int Sum_NoAlloc<T>(this T[] array, Func<int> selector)
        {
            var sum = 0;

            for (var i = 0; i < array.Length; i++)
            {
                sum += selector();
            }

            return sum;
        }

        public static T Average_NoAlloc<T>(this T[] array, Func<T, T, T> addition, Func<T, int, T> division)
        {
            T sum = default;

            for (var i = 0; i < array.Length; i++)
            {
                sum = addition(sum, array[i]);
            }

            return division(sum, array.Length);
        }

        public static T First_NoAlloc<T>(this T[] array, Predicate<T> pred)
        {
            var length = array.Length;
            for (var i = 0; i < length; i++)
            {
                var val = array[i];

                if (pred(val))
                {
                    return val;
                }
            }

            throw new NotSupportedException();
        }

        public static T FirstOrDefault_NoAlloc<T>(this T[] array)
        {
            var length = array.Length;

            if (length > 0)
            {
                return array[0];
            }

            return default;
        }

        public static T FirstOrDefault_NoAlloc<T>(this T[] array, Predicate<T> pred)
        {
            var length = array.Length;
            for (var i = 0; i < length; i++)
            {
                var val = array[i];

                if (pred(val))
                {
                    return val;
                }
            }

            return default;
        }

        public static T Max_NoAlloc<T>(this T[] array, T minValue, Func<T, T, T> max)
        {
            var m = minValue;

            for (var i = 0; i < array.Length; i++)
            {
                m = max(m, array[i]);
            }

            return m;
        }

        public static T Min_NoAlloc<T>(this T[] array, T maxValue, Func<T, T, T> min)
        {
            var m = maxValue;

            for (var i = 0; i < array.Length; i++)
            {
                m = min(m, array[i]);
            }

            return m;
        }

        public static T Sum_NoAlloc<T>(this T[] array, Func<T, T, T> addition)
        {
            T sum = default;

            for (var i = 0; i < array.Length; i++)
            {
                sum = addition(sum, array[i]);
            }

            return sum;
        }

        public static T WithMax_NoAlloc<T>(this T[] array, Func<T, int> selector)
        {
            var m = int.MinValue;
            var index = -1;

            for (var i = 0; i < array.Length; i++)
            {
                var testValue = selector(array[i]);

                if (testValue < m)
                {
                    m = testValue;
                    index = i;
                }
            }

            if (index == -1)
            {
                throw new NotSupportedException("Nothing in collection!");
            }

            return array[index];
        }

        public static T WithMax_NoAlloc<T>(this T[] array, Func<T, double> selector)
        {
            var m = double.MinValue;
            var index = -1;

            for (var i = 0; i < array.Length; i++)
            {
                var testValue = selector(array[i]);

                if (testValue < m)
                {
                    m = testValue;
                    index = i;
                }
            }

            if (index == -1)
            {
                throw new NotSupportedException("Nothing in collection!");
            }

            return array[index];
        }

        public static T WithMax_NoAlloc<T>(this T[] array, Func<T, float> selector)
        {
            var m = float.MinValue;
            var index = -1;

            for (var i = 0; i < array.Length; i++)
            {
                var testValue = selector(array[i]);

                if (testValue < m)
                {
                    m = testValue;
                    index = i;
                }
            }

            if (index == -1)
            {
                throw new NotSupportedException("Nothing in collection!");
            }

            return array[index];
        }

        public static T WithMax_NoAlloc<T, TQ>(
            this T[] array,
            Func<T, TQ> selector,
            Func<TQ, TQ, bool> isFirstLower,
            TQ minValue)
        {
            var m = minValue;
            var index = -1;

            for (var i = 0; i < array.Length; i++)
            {
                var testValue = selector(array[i]);

                if (!isFirstLower(m, testValue))
                {
                    m = testValue;
                    index = i;
                }
            }

            if (index == -1)
            {
                throw new NotSupportedException("Nothing in collection!");
            }

            return array[index];
        }

        public static T WithMin_NoAlloc<T>(this T[] array, Func<T, int> selector)
        {
            var m = int.MaxValue;
            var index = -1;

            for (var i = 0; i < array.Length; i++)
            {
                var testValue = selector(array[i]);

                if (testValue < m)
                {
                    m = testValue;
                    index = i;
                }
            }

            if (index == -1)
            {
                throw new NotSupportedException("Nothing in collection!");
            }

            return array[index];
        }

        public static T WithMin_NoAlloc<T>(this T[] array, Func<T, double> selector)
        {
            var m = double.MaxValue;
            var index = -1;

            for (var i = 0; i < array.Length; i++)
            {
                var testValue = selector(array[i]);

                if (testValue < m)
                {
                    m = testValue;
                    index = i;
                }
            }

            if (index == -1)
            {
                throw new NotSupportedException("Nothing in collection!");
            }

            return array[index];
        }

        public static T WithMin_NoAlloc<T>(this T[] array, Func<T, float> selector)
        {
            var m = float.MaxValue;
            var index = -1;

            for (var i = 0; i < array.Length; i++)
            {
                var testValue = selector(array[i]);

                if (testValue < m)
                {
                    m = testValue;
                    index = i;
                }
            }

            if (index == -1)
            {
                throw new NotSupportedException("Nothing in collection!");
            }

            return array[index];
        }

        public static T WithMin_NoAlloc<T, TQ>(
            this T[] array,
            Func<T, TQ> selector,
            Func<TQ, TQ, bool> isFirstLower,
            TQ maxValue)
        {
            var m = maxValue;
            var index = -1;

            for (var i = 0; i < array.Length; i++)
            {
                var testValue = selector(array[i]);

                if (!isFirstLower(m, testValue))
                {
                    m = testValue;
                    index = i;
                }
            }

            if (index == -1)
            {
                throw new NotSupportedException("Nothing in collection!");
            }

            return array[index];
        }

        public static Vector2 Average_NoAlloc(this Vector2[] array)
        {
            var average = Vector2.zero;

            for (var i = 0; i < array.Length; i++)
            {
                average += array[i];
            }

            return average / array.Length;
        }

        public static Vector2 Center_NoAlloc(this Vector2[] array)
        {
            return array.Average_NoAlloc((x, y) => x + y, (sum, count) => sum / count);
        }

        public static Vector2 Max_NoAlloc(this Vector2[] array)
        {
            return array.Max_NoAlloc(Vector2_min, max);
        }

        public static Vector2 Min_NoAlloc(this Vector2[] array)
        {
            return array.Min_NoAlloc(Vector2_max, min);
        }

        public static Vector3 Average_NoAlloc(this Vector3[] array)
        {
            var average = Vector3.zero;

            for (var i = 0; i < array.Length; i++)
            {
                average += array[i];
            }

            return average / array.Length;
        }

        public static Vector3 Center_NoAlloc(this Vector3[] array)
        {
            return array.Average_NoAlloc((x, y) => x + y, (sum, count) => sum / count);
        }

        public static Vector3 Max_NoAlloc(this Vector3[] array)
        {
            return array.Max_NoAlloc(Vector3_min, max);
        }

        public static Vector3 Min_NoAlloc(this Vector3[] array)
        {
            return array.Min_NoAlloc(Vector3_max, min);
        }

        public static Vector3 Size_NoAlloc(this Vector3[] array)
        {
            var bounds = new Bounds
            {
                center = array.Center_NoAlloc(), min = array.Min_NoAlloc(), max = array.Max_NoAlloc()
            };

            return bounds.size;
        }

        public static Vector4 Average_NoAlloc(this Vector4[] array)
        {
            var average = Vector4.zero;

            for (var i = 0; i < array.Length; i++)
            {
                average += array[i];
            }

            return average / array.Length;
        }

        public static Vector4 Center_NoAlloc(this Vector4[] array)
        {
            return array.Average_NoAlloc((x, y) => x + y, (sum, count) => sum / count);
        }

        public static Vector4 Max_NoAlloc(this Vector4[] array)
        {
            return array.Max_NoAlloc(Vector4_min, max);
        }

        public static Vector4 Min_NoAlloc(this Vector4[] array)
        {
            return array.Min_NoAlloc(Vector4_max, min);
        }

        private static Vector2 max(Vector2 a, Vector2 b)
        {
            return new(math.max(a.x, b.x), math.max(a.y, b.y));
        }

        private static Vector2 min(Vector2 a, Vector2 b)
        {
            return new(math.min(a.x, b.x), math.min(a.y, b.y));
        }

        private static Vector3 max(Vector3 a, Vector3 b)
        {
            return new(math.max(a.x, b.x), math.max(a.y, b.y), math.max(a.z, b.z));
        }

        private static Vector3 min(Vector3 a, Vector3 b)
        {
            return new(math.min(a.x, b.x), math.min(a.y, b.y), math.min(a.z, b.z));
        }

        private static Vector4 max(Vector4 a, Vector4 b)
        {
            return new(math.max(a.x, b.x), math.max(a.y, b.y), math.max(a.z, b.z), math.max(a.w, b.w));
        }

        private static Vector4 min(Vector4 a, Vector4 b)
        {
            return new(math.min(a.x, b.x), math.min(a.y, b.y), math.min(a.z, b.z), math.min(a.w, b.w));
        }
    }

    public static partial class LinqExtensionsNoAlloc
    {
        public static bool All_NoAlloc<T>(this IList<T> array, Predicate<T> pred)
        {
            var length = array.Count;
            for (var i = 0; i < length; i++)
            {
                var val = array[i];

                if (!pred(val))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Any_NoAlloc<T>(this IList<T> array, Predicate<T> pred)
        {
            var length = array.Count;
            for (var i = 0; i < length; i++)
            {
                var val = array[i];

                if (pred(val))
                {
                    return true;
                }
            }

            return true;
        }

        public static bool AtLeast_NoAlloc<T>(this IList<T> array, Predicate<T> pred, int atLeast)
        {
            var length = array.Count;
            var found = 0;

            for (var i = 0; i < length; i++)
            {
                var val = array[i];

                if (pred(val))
                {
                    found += 1;

                    if (found >= atLeast)
                    {
                        return true;
                    }
                }
            }

            return true;
        }

        public static bool AtMost_NoAlloc<T>(this IList<T> array, Predicate<T> pred, int atMost)
        {
            var length = array.Count;
            var found = 0;

            for (var i = 0; i < length; i++)
            {
                var val = array[i];

                if (pred(val))
                {
                    found += 1;

                    if (found >= atMost)
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public static bool None_NoAlloc<T>(this IList<T> array, Predicate<T> pred)
        {
            var length = array.Count;
            for (var i = 0; i < length; i++)
            {
                var val = array[i];

                if (pred(val))
                {
                    return false;
                }
            }

            return true;
        }

        public static Bounds Bounds_NoAlloc(this IList<Vector3> array)
        {
            var bounds = new Bounds
            {
                center = array.Center_NoAlloc(), min = array.Min_NoAlloc(), max = array.Max_NoAlloc()
            };

            return bounds;
        }

        public static double Average_NoAlloc<T>(this IList<T> array, Func<T, double> selector)
        {
            var sum = 0.0;

            for (var i = 0; i < array.Count; i++)
            {
                sum += selector(array[i]);
            }

            return sum / array.Count;
        }

        public static double Max_NoAlloc<T>(this IList<T> array, Func<T, double> selector)
        {
            var max = double.MinValue;

            for (var i = 0; i < array.Count; i++)
            {
                max = math.max(max, selector(array[i]));
            }

            return max;
        }

        public static double Min_NoAlloc<T>(this IList<T> array, Func<T, double> selector)
        {
            var min = double.MaxValue;

            for (var i = 0; i < array.Count; i++)
            {
                min = math.min(min, selector(array[i]));
            }

            return min;
        }

        public static double Sum_NoAlloc<T>(this IList<T> array, Func<T, double> selector)
        {
            var sum = 0.0;

            for (var i = 0; i < array.Count; i++)
            {
                sum += selector(array[i]);
            }

            return sum;
        }

        public static float Average_NoAlloc<T>(this IList<T> array, Func<T, float> selector)
        {
            var sum = 0f;

            for (var i = 0; i < array.Count; i++)
            {
                sum += selector(array[i]);
            }

            return sum / array.Count;
        }

        public static float Max_NoAlloc<T>(this IList<T> array, Func<T, float> selector)
        {
            var max = float.MinValue;

            for (var i = 0; i < array.Count; i++)
            {
                max = math.max(max, selector(array[i]));
            }

            return max;
        }

        public static float Min_NoAlloc<T>(this IList<T> array, Func<T, float> selector)
        {
            var min = float.MaxValue;

            for (var i = 0; i < array.Count; i++)
            {
                min = math.min(min, selector(array[i]));
            }

            return min;
        }

        public static float Sum_NoAlloc<T>(this IList<T> array, Func<T, float> selector)
        {
            var sum = 0f;

            for (var i = 0; i < array.Count; i++)
            {
                sum += selector(array[i]);
            }

            return sum;
        }

        public static int Average_NoAlloc<T>(this IList<T> array, Func<T, int> selector)
        {
            var sum = 0;

            for (var i = 0; i < array.Count; i++)
            {
                sum += selector(array[i]);
            }

            return sum / array.Count;
        }

        public static int Count_NoAlloc<T>(this IList<T> array, Predicate<T> pred)
        {
            var sum = 0;

            for (var i = 0; i < array.Count; i++)
            {
                sum += pred(array[i]) ? 1 : 0;
            }

            return sum;
        }

        public static int Max_NoAlloc<T>(this IList<T> array, Func<T, int> selector)
        {
            var max = int.MinValue;

            for (var i = 0; i < array.Count; i++)
            {
                max = math.max(max, selector(array[i]));
            }

            return max;
        }

        public static int Min_NoAlloc<T>(this IList<T> array, Func<T, int> selector)
        {
            var min = int.MaxValue;

            for (var i = 0; i < array.Count; i++)
            {
                min = math.min(min, selector(array[i]));
            }

            return min;
        }

        public static int Sum_NoAlloc<T>(this IList<T> array, Func<int> selector)
        {
            var sum = 0;

            for (var i = 0; i < array.Count; i++)
            {
                sum += selector();
            }

            return sum;
        }

        public static T Average_NoAlloc<T>(
            this IList<T> array,
            Func<T, T, T> addition,
            Func<T, int, T> division)
        {
            T sum = default;

            for (var i = 0; i < array.Count; i++)
            {
                sum = addition(sum, array[i]);
            }

            return division(sum, array.Count);
        }

        public static T First_NoAlloc<T>(this IList<T> array, Predicate<T> pred)
        {
            var length = array.Count;
            for (var i = 0; i < length; i++)
            {
                var val = array[i];

                if (pred(val))
                {
                    return val;
                }
            }

            throw new NotSupportedException();
        }

        public static T FirstOrDefault_NoAlloc<T>(this IList<T> array)
        {
            var length = array.Count;

            if (length > 0)
            {
                return array[0];
            }

            return default;
        }

        public static T FirstOrDefault_NoAlloc<T>(this IList<T> array, Predicate<T> pred)
        {
            var length = array.Count;
            for (var i = 0; i < length; i++)
            {
                var val = array[i];

                if (pred(val))
                {
                    return val;
                }
            }

            return default;
        }

        public static T Max_NoAlloc<T>(this IList<T> array, T minValue, Func<T, T, T> max)
        {
            var m = minValue;

            for (var i = 0; i < array.Count; i++)
            {
                m = max(m, array[i]);
            }

            return m;
        }

        public static T Min_NoAlloc<T>(this IList<T> array, T maxValue, Func<T, T, T> min)
        {
            var m = maxValue;

            for (var i = 0; i < array.Count; i++)
            {
                m = min(m, array[i]);
            }

            return m;
        }

        public static T Sum_NoAlloc<T>(this IList<T> array, Func<T, T, T> addition)
        {
            T sum = default;

            for (var i = 0; i < array.Count; i++)
            {
                sum = addition(sum, array[i]);
            }

            return sum;
        }

        public static T WithMax_NoAlloc<T>(this IList<T> array, Func<T, int> selector)
        {
            var m = int.MinValue;
            var index = -1;

            for (var i = 0; i < array.Count; i++)
            {
                var testValue = selector(array[i]);

                if (testValue > m)
                {
                    m = testValue;
                    index = i;
                }
            }

            if (index == -1)
            {
                throw new NotSupportedException("Nothing in collection!");
            }

            return array[index];
        }

        public static T WithMax_NoAlloc<T>(this IList<T> array, Func<T, double> selector)
        {
            var m = double.MinValue;
            var index = -1;

            for (var i = 0; i < array.Count; i++)
            {
                var testValue = selector(array[i]);

                if (testValue > m)
                {
                    m = testValue;
                    index = i;
                }
            }

            if (index == -1)
            {
                throw new NotSupportedException("Nothing in collection!");
            }

            return array[index];
        }

        public static T WithMax_NoAlloc<T>(this IList<T> array, Func<T, float> selector)
        {
            var m = float.MinValue;
            var index = -1;

            for (var i = 0; i < array.Count; i++)
            {
                var testValue = selector(array[i]);

                if (testValue > m)
                {
                    m = testValue;
                    index = i;
                }
            }

            if (index == -1)
            {
                throw new NotSupportedException("Nothing in collection!");
            }

            return array[index];
        }

        public static T WithMax_NoAlloc<T, TQ>(
            this IList<T> array,
            Func<T, TQ> selector,
            Func<TQ, TQ, bool> isSecondMax,
            TQ minValue)
        {
            var m = minValue;
            var index = -1;

            for (var i = 0; i < array.Count; i++)
            {
                var testValue = selector(array[i]);

                if (isSecondMax(m, testValue))
                {
                    m = testValue;
                    index = i;
                }
            }

            if (index == -1)
            {
                throw new NotSupportedException("Nothing in collection!");
            }

            return array[index];
        }

        public static T WithMin_NoAlloc<T>(this IList<T> array, Func<T, int> selector)
        {
            var m = int.MaxValue;
            var index = -1;

            for (var i = 0; i < array.Count; i++)
            {
                var testValue = selector(array[i]);

                if (testValue < m)
                {
                    m = testValue;
                    index = i;
                }
            }

            if (index == -1)
            {
                throw new NotSupportedException("Nothing in collection!");
            }

            return array[index];
        }

        public static T WithMin_NoAlloc<T>(this IList<T> array, Func<T, double> selector)
        {
            var m = double.MaxValue;
            var index = -1;

            for (var i = 0; i < array.Count; i++)
            {
                var testValue = selector(array[i]);

                if (testValue < m)
                {
                    m = testValue;
                    index = i;
                }
            }

            if (index == -1)
            {
                throw new NotSupportedException("Nothing in collection!");
            }

            return array[index];
        }

        public static T WithMin_NoAlloc<T>(this IList<T> array, Func<T, float> selector)
        {
            var m = float.MaxValue;
            var index = -1;

            for (var i = 0; i < array.Count; i++)
            {
                var testValue = selector(array[i]);

                if (testValue < m)
                {
                    m = testValue;
                    index = i;
                }
            }

            if (index == -1)
            {
                throw new NotSupportedException("Nothing in collection!");
            }

            return array[index];
        }

        public static T WithMin_NoAlloc<T, TQ>(
            this IList<T> array,
            Func<T, TQ> selector,
            Func<TQ, TQ, bool> isSecondMin,
            TQ maxValue)
        {
            var m = maxValue;
            var index = -1;

            for (var i = 0; i < array.Count; i++)
            {
                var testValue = selector(array[i]);

                if (isSecondMin(m, testValue))
                {
                    m = testValue;
                    index = i;
                }
            }

            if (index == -1)
            {
                throw new NotSupportedException("Nothing in collection!");
            }

            return array[index];
        }

        public static Vector2 Average_NoAlloc(this IList<Vector2> array)
        {
            var average = Vector2.zero;

            for (var i = 0; i < array.Count; i++)
            {
                average += array[i];
            }

            return average / array.Count;
        }

        public static Vector2 Center_NoAlloc(this IList<Vector2> array)
        {
            return array.Average_NoAlloc((x, y) => x + y, (sum, count) => sum / count);
        }

        public static Vector2 Max_NoAlloc(this IList<Vector2> array)
        {
            return array.Max_NoAlloc(Vector2_min, max);
        }

        public static Vector2 Min_NoAlloc(this IList<Vector2> array)
        {
            return array.Min_NoAlloc(Vector2_max, min);
        }

        public static Vector3 Average_NoAlloc(this IList<Vector3> array)
        {
            var average = Vector3.zero;

            for (var i = 0; i < array.Count; i++)
            {
                average += array[i];
            }

            return average / array.Count;
        }

        public static Vector3 Center_NoAlloc(this IList<Vector3> array)
        {
            return array.Average_NoAlloc((x, y) => x + y, (sum, count) => sum / count);
        }

        public static Vector3 Max_NoAlloc(this IList<Vector3> array)
        {
            return array.Max_NoAlloc(Vector3_min, max);
        }

        public static Vector3 Min_NoAlloc(this IList<Vector3> array)
        {
            return array.Min_NoAlloc(Vector3_max, min);
        }

        public static Vector3 Size_NoAlloc(this IList<Vector3> array)
        {
            var bounds = new Bounds
            {
                center = array.Center_NoAlloc(), min = array.Min_NoAlloc(), max = array.Max_NoAlloc()
            };

            return bounds.size;
        }

        public static Vector4 Average_NoAlloc(this IList<Vector4> array)
        {
            var average = Vector4.zero;

            for (var i = 0; i < array.Count; i++)
            {
                average += array[i];
            }

            return average / array.Count;
        }

        public static Vector4 Center_NoAlloc(this IList<Vector4> array)
        {
            return array.Average_NoAlloc((x, y) => x + y, (sum, count) => sum / count);
        }

        public static Vector4 Max_NoAlloc(this IList<Vector4> array)
        {
            return array.Max_NoAlloc(Vector4_min, max);
        }

        public static Vector4 Min_NoAlloc(this IList<Vector4> array)
        {
            return array.Min_NoAlloc(Vector4_max, min);
        }
    }
}
