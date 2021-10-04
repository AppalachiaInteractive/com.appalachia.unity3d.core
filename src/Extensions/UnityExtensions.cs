#region

using System;

#endregion

namespace Appalachia.Core.Extensions
{
    public static class UnityExtensions
    {
        public static TO ifnull<TI, TO>(this TI input, Func<TI, TO> retriever, TO defaultOutput)
            where TI : UnityEngine.Object
        {
            if (input == null)
            {
                return defaultOutput;
            }

            return retriever(input);

        }
        
        public static TO notnull<TI, TO>(this TI input, Func<TI, TO> retriever, TO defaultOutput)
            where TI : UnityEngine.Object
        {
            if (input != null)
            {
                return defaultOutput;
            }
            
            return retriever(input);
        }
        
    }
}