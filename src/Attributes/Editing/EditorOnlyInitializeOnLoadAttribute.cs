#region

using UnityEditor;

#endregion

namespace Appalachia.Core.Attributes.Editing
{
    public class EditorOnlyInitializeOnLoadAttribute
#if UNITY_EDITOR
        : InitializeOnLoadAttribute
#else
        : Attribute
#endif
    {
    }
}
