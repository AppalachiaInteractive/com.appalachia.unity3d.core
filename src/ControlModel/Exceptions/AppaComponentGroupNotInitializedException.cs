using System;

namespace Appalachia.Core.ControlModel.Exceptions
{
    public class AppaComponentGroupNotInitializedException : AppaInitializationException
    {
        public AppaComponentGroupNotInitializedException(Type issueType, string message) : base(issueType, message)
        {
        }
    }
}
