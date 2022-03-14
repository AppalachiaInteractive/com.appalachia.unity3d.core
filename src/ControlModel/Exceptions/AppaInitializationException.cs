using System;

namespace Appalachia.Core.ControlModel.Exceptions
{
    public abstract class AppaInitializationException : Exception
    {
        protected AppaInitializationException(Type issueType, string message) : base(message)
        {
            this.issueType = issueType;
        }

        private AppaInitializationException()
        {
        }

        #region Fields and Autoproperties

        public readonly Type issueType;

        #endregion
    }
}
