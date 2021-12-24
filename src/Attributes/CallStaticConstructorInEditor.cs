using System;

namespace Appalachia.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CallStaticConstructorInEditor
#if UNITY_EDITOR
        : UnityEditor.InitializeOnLoadAttribute
#else
        : System.Attribute
#endif
    {
    }
}