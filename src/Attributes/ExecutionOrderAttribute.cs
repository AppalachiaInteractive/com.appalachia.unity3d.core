#region

using System;

#endregion

namespace Appalachia.Core.Attributes
{
    public class ExecutionOrderAttribute : Attribute
    {
        public short Order;

        public ExecutionOrderAttribute(short order)
        {
            Order = order;
        }
    }
}
