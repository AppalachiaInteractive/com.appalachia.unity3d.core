#region

using System;

#endregion

namespace Appalachia.Core.Exceptions
{
    public class StaticClassNotProperlyInitializedException : Exception
    {
        public StaticClassNotProperlyInitializedException(string className) : base(
            $"Static class {className} was not initialized properly."
        )
        {
        }

        public StaticClassNotProperlyInitializedException(string className, string fieldName) :
            base(
                $"Static class {className} was not initialized properly: Issue with field {fieldName}."
            )
        {
        }
    }
}
