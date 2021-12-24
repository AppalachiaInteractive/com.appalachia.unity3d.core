#region

using System;
using Appalachia.Utility.Strings;

#endregion

namespace Appalachia.Core.Collections.Exceptions
{
    public class IndexedKeyValueMismatchException : Exception
    {
        public IndexedKeyValueMismatchException(int keyCount, int valueCount) : base(
            ZString.Format(
                "Count mismatch between indexed keys [{0}] and values [{1}]!",
                keyCount,
                valueCount
            )
        )
        {
        }
    }
}
