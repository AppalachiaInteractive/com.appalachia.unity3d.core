#region

using System;

#endregion

namespace Appalachia.Core.Attributes
{
    public class ExecutionOrderAttribute : Attribute
    {
        public ExecutionOrderAttribute(short order)
        {
            Order = order;
        }

        #region Fields and Autoproperties

        public short Order;

        #endregion
    }
}
