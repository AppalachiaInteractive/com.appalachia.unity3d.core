using System;

namespace Appalachia.Core.Attributes
{
    /// <summary>
    ///     Allows the specification of a method that should be executed when OnPreCull fires, even
    ///     if the class is not derived from MonoBehaviour.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ExecuteOnPreCullAttribute : ExecuteEventBaseAttribute
    {
    }
}
