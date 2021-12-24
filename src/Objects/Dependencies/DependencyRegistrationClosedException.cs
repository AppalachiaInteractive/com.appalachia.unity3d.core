using System;
using Appalachia.Utility.Constants;

namespace Appalachia.Core.Objects.Dependencies
{
    public sealed class DependencyRegistrationClosedException : Exception
    {
        public DependencyRegistrationClosedException(Type t) : base(
            $"Dependency registration has already been closed.  The type {t.FormatForLogging()} must invoke its dependency registration before the first frame executes."
        )
        {
        }
    }
}
