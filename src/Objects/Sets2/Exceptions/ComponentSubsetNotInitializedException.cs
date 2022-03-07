using System;

namespace Appalachia.Core.Objects.Sets2.Exceptions
{
    public class ComponentSubsetNotInitializedException : ComponentInitializationException
    {
        public ComponentSubsetNotInitializedException(Type issueType, string message) : base(
            issueType,
            message
        )
        {
        }
    }
}
