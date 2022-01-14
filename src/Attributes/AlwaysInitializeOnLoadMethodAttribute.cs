using System;

namespace Appalachia.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AlwaysInitializeOnLoadMethodAttribute
#if UNITY_EDITOR
        : UnityEditor.InitializeOnLoadMethodAttribute
#else
        : UnityEngine.RuntimeInitializeOnLoadMethodAttribute
#endif
    {
    }
}
