using System;

namespace Appalachia.Core.Objects.Components.Exceptions
{
    public abstract class ComponentInitializationException : Exception
    {
        protected ComponentInitializationException(Type issueType, string message) : base(message)
        {
            this.issueType = issueType;
        }

        private ComponentInitializationException()
        {
        }

        #region Fields and Autoproperties

        public readonly Type issueType;

        #endregion
    }
}
