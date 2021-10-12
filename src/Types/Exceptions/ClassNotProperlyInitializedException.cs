#region

using System;

#endregion

namespace Appalachia.Core.Types.Exceptions
{
    public class ClassNotProperlyInitializedException : Exception
    {
        public ClassNotProperlyInitializedException(string className) : base(
            $"Static class {className} was not initialized properly."
        )
        {
        }

        public ClassNotProperlyInitializedException(string className, string fieldName) : base(
            $"Static class {className} was not initialized properly: Issue with field {fieldName}."
        )
        {
        }
    }
}
