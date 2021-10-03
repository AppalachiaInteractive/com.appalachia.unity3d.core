#region

using System;
using Appalachia.Core.Collections;

#endregion

namespace Appalachia.Core.Collections.Extensions
{
    public static class ArrayExtensions
    {
        public static TSet ToAppaSet<T, TSet, TList>(this T[] array)
            where T : IEquatable<T>
            where TSet : AppaSet<T, TList>, new()
            where TList : AppaList<T>, new()
        {
            var hash = new TSet();

            if ((array == null) || (array.Length == 0))
            {
                return hash;
            }

            for (var i = 0; i < array.Length; i++)
            {
                var val = array[i];

                if (val != null)
                {
                    hash.Add(val);
                }
            }

            return hash;
        }
        
        
        
    }
}
