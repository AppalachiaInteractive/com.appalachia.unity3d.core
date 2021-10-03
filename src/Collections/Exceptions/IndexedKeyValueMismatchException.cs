#region

using System;

#endregion

namespace Appalachia.Core.Collections.Exceptions
{
    public class IndexedKeyValueMismatchException : Exception
    {
        public IndexedKeyValueMismatchException(int keyCount, int valueCount) : base(
            $"Count mismatch between indexed keys [{keyCount}] and values [{valueCount}]!"
        )
        {
        }
    }
}
