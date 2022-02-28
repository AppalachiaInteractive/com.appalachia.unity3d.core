#region

using System;

#endregion

namespace Appalachia.Core.Collections.Extensions
{
    public static class ArrayExtensions
    {
        public static TComponentSet ToAppaSet<T, TComponentSet, TList>(this T[] array)
            where T : IEquatable<T>
            where TComponentSet : AppaSet<T, TList>, new()
            where TList : AppaList<T>, new()
        {
            var hash = new TComponentSet();

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
