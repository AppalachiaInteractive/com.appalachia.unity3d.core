using System;

namespace Appalachia.Core.Objects.Sets.Exceptions
{
    public class ComponentNotInitializedException : ComponentInitializationException
    {
        public ComponentNotInitializedException(Type issueType, string message) : base(issueType, message)
        {
        }
    }
}
