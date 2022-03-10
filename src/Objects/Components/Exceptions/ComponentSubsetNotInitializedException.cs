using System;

namespace Appalachia.Core.Objects.Components.Exceptions
{
    public class ComponentSubsetNotInitializedException : ComponentInitializationException
    {
        public ComponentSubsetNotInitializedException(Type issueType, string message) : base(issueType, message)
        {
        }
    }
}
