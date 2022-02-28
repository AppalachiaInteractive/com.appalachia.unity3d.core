using System;

namespace Appalachia.Core.Objects.Sets.Exceptions
{
    public class ComponentSetNotInitializedException : ComponentInitializationException
    {
        public ComponentSetNotInitializedException(Type issueType, string message) : base(issueType, message)
        {
        }
    }
}
