using Unity.Collections;

namespace Appalachia.Core.Collections.Extensions
{
    public static class NativeBitArrayExtensions
    {
        public static void SetAllBits(this NativeBitArray array, bool value)
        {
            array.SetBits(0, value, array.Length - 1);
        }

        public static int Sum(this NativeBitArray array)
        {
            return array.CountAllBits();
        }
        
        public static int CountAllBits(this NativeBitArray array)
        {
            return array.CountBits(0, array.Length - 1);
        }

        public static bool[] ToArray(this NativeBitArray array)
        {
            var outArray = new bool[array.Length];
            
            for (int i = 0; i < array.Length; ++i)
            {
                outArray[i] = array.IsSet(i);
            }
            
            return outArray;
        }

        public static NativeBitArray FromArray(this bool[] values)
        {
            var outArray = new NativeBitArray(values.Length, Allocator.Persistent);
            
            for (var i = 0; i < values.Length; i++)
            {
                outArray.Set(i, values[i]);
            }
            
            return outArray;
        }
    }
}
