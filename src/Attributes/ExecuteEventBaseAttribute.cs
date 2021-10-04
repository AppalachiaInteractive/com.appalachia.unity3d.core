using System;

namespace Appalachia.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public abstract class ExecuteEventBaseAttribute : Attribute
    {
    }
}
