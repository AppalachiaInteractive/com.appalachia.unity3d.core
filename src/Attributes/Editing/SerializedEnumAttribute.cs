using System;
using UnityEngine;

namespace Appalachia.Core.Attributes.Editing
{
    public class SerializedEnumAttribute : PropertyAttribute
    {
        public readonly Type type;

        public SerializedEnumAttribute(Type type)
        {
            this.type = type;
        }
    }
}
