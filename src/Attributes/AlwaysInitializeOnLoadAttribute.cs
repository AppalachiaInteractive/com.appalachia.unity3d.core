using UnityEditor;

namespace Appalachia.Core.Attributes
{
    public class AlwaysInitializeOnLoadAttribute
#if UNITY_EDITOR
        : InitializeOnLoadAttribute
#else
        : Attribute
#endif
    {
    }
}
