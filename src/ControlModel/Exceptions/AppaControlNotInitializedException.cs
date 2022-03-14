using System;

namespace Appalachia.Core.ControlModel.Exceptions
{
    public class AppaControlNotInitializedException : AppaInitializationException
    {
        public AppaControlNotInitializedException(Type issueType, string message) : base(issueType, message)
        {
        }
    }
}
