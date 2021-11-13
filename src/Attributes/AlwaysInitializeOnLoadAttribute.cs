
namespace Appalachia.Core.Attributes
{
    public class AlwaysInitializeOnLoadAttribute
#if UNITY_EDITOR
        : UnityEditor.InitializeOnLoadAttribute
#else
        : System.Attribute
#endif
    {
    }
}