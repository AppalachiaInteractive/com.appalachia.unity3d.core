namespace Appalachia.Core.Attributes
{
    public class InitializeOnLoadMethodAlwaysAttribute
#if UNITY_EDITOR
        : UnityEditor.InitializeOnLoadMethodAttribute
#else
        : UnityEngine.RuntimeInitializeOnLoadMethodAttribute
#endif
    {
    }
}
