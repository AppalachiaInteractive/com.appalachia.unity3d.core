using System;

namespace Appalachia.Core.ControlModel.Exceptions
{
    public class AppaComponentNotInitializedException : AppaInitializationException
    {
        public AppaComponentNotInitializedException(Type issueType, string message) : base(issueType, message)
        {
        }
    }
}
