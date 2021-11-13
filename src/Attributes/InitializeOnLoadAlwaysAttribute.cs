namespace Appalachia.Core.Attributes
{
    public class InitializeOnLoadAlwaysAttribute
#if UNITY_EDITOR
        : UnityEditor.InitializeOnLoadAttribute
#else
        : System.Attribute
#endif
    {
    }
}