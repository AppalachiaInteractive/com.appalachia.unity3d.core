#region

using Unity.Mathematics;
using UnityEditor;

#endregion

namespace Appalachia.Core.Preferences.API
{
    public struct float3_EPAPI : IEditorPreferenceAPI<float3>
    {
        public float3 Get(string key, float3 defaultValue, float3 low, float3 high)
        {
            var result = float3.zero;
            result.x = EditorPrefs.GetFloat($"{key}.x", defaultValue.x);
            result.y = EditorPrefs.GetFloat($"{key}.y", defaultValue.y);
            result.z = EditorPrefs.GetFloat($"{key}.z", defaultValue.z);
            return result;
        }

        public void Save(string key, float3 value, float3 low, float3 high)
        {
            EditorPrefs.SetFloat($"{key}.x", value.x);
            EditorPrefs.SetFloat($"{key}.y", value.y);
            EditorPrefs.SetFloat($"{key}.z", value.z);
        }

        public float3 Draw(string label, float3 value, float3 low, float3 high)
        {
            return EditorGUILayout.Vector3Field(label, value);
        }
    }
}
