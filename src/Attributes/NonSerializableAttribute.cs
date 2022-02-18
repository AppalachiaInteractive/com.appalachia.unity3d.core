using System;

namespace Appalachia.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NonSerializableAttribute : Attribute
    {
    }
}
