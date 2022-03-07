using System;

namespace Appalachia.Core.Objects.Sets2.Exceptions
{
    public class ComponentSetNotInitializedException : ComponentInitializationException
    {
        public ComponentSetNotInitializedException(Type issueType, string message) : base(issueType, message)
        {
        }
    }
}
