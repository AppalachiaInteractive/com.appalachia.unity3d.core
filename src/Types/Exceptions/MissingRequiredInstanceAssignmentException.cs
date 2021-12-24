#region

using System;
using System.Runtime.Serialization;
using Appalachia.Utility.Strings;

#endregion

namespace Appalachia.Core.Types.Exceptions
{
    public class MissingRequiredInstanceAssignmentException<T> : MissingRequiredInstanceAssignmentException
    {
        public MissingRequiredInstanceAssignmentException(string fieldName) : base(
            ZString.Format(
                "Failed to initialize instance of {0}: [{1}] was not initialized properly.",
                typeof(T).Name,
                fieldName
            )
        )
        {
        }
    }

    public class MissingRequiredInstanceAssignmentException : Exception
    {
        public MissingRequiredInstanceAssignmentException(string message, Exception innerException) : base(
            message,
            innerException
        )
        {
        }

        protected MissingRequiredInstanceAssignmentException(string message) : base(message)
        {
        }

        protected MissingRequiredInstanceAssignmentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
