namespace Appalachia.Core.Attributes.Editing
{
    public class EditorOnlyInitializeOnLoadAttribute
#if UNITY_EDITOR
        : UnityEditor.InitializeOnLoadAttribute
#else
        : System.Attribute
#endif
    {
    }
}
