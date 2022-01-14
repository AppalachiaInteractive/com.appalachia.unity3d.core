using System;
using UnityEngine;

namespace Appalachia.Core.Attributes.Editing
{
    public class SerializedEnumAttribute : PropertyAttribute
    {
        public SerializedEnumAttribute(Type type)
        {
            this.type = type;
        }

        #region Fields and Autoproperties

        public readonly Type type;

        #endregion
    }
}
