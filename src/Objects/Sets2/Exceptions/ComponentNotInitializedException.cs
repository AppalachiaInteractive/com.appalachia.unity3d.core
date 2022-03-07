using System;

namespace Appalachia.Core.Objects.Sets2.Exceptions
{
    public class ComponentNotInitializedException : ComponentInitializationException
    {
        public ComponentNotInitializedException(Type issueType, string message) : base(issueType, message)
        {
        }
    }
}
