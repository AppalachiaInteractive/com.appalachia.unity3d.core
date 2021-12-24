using System;

namespace Appalachia.Core.Objects.Initialization
{
    public class InstanceNotReadyException : Exception
    {
        public InstanceNotReadyException() : base(
            "The instance must be available before you access this property."
        )
        {
        }
    }
}
