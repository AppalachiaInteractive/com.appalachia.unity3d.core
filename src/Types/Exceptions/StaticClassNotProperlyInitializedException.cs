#region

using System;
using Appalachia.Utility.Strings;

#endregion

namespace Appalachia.Core.Types.Exceptions
{
    public class StaticClassNotProperlyInitializedException : Exception
    {
        public StaticClassNotProperlyInitializedException(string className) : base(
            ZString.Format("Static class {0} was not initialized properly.", className)
        )
        {
        }

        public StaticClassNotProperlyInitializedException(string className, string fieldName) : base(
            ZString.Format(
                "Static class {0} was not initialized properly: Issue with field {1}.",
                className,
                fieldName
            )
        )
        {
        }
    }
}
