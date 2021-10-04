#region

using System;
using System.Runtime.Serialization;

#endregion

namespace Appalachia.Core.Exceptions
{
    public class MissingRequiredInstanceAssignmentException<T> : MissingRequiredInstanceAssignmentException
    {
        public MissingRequiredInstanceAssignmentException(string fieldName) : base(
            $"Failed to initialize instance of {typeof(T).Name}: [{fieldName}] was not initialized properly."
        )
        {
        }
    }

    public class MissingRequiredInstanceAssignmentException : Exception
    {
        protected MissingRequiredInstanceAssignmentException(string message) : base(message)
        {
        }

        public MissingRequiredInstanceAssignmentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MissingRequiredInstanceAssignmentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
