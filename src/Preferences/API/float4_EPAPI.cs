#region

using Unity.Mathematics;
using UnityEditor;

#endregion

namespace Appalachia.Core.Preferences.API
{
    public struct float4_EPAPI : IEditorPreferenceAPI<float4>
    {
        public void Save(PREF<float4> pref)
        {
            Save(pref.Key, pref.Value, pref.Low, pref.High);
        }

        public float4 Draw(PREF<float4> pref)
        {
            return Draw(pref.Key, pref.Value, pref.Low, pref.High);
        }

        public float4 Get(string key, float4 defaultValue, float4 low, float4 high)
        {
            var result = float4.zero;
            result.x = EditorPrefs.GetFloat($"{key}.x", defaultValue.x);
            result.y = EditorPrefs.GetFloat($"{key}.y", defaultValue.y);
            result.z = EditorPrefs.GetFloat($"{key}.z", defaultValue.z);
            result.w = EditorPrefs.GetFloat($"{key}.w", defaultValue.w);
            return result;
        }

        public void Save(string key, float4 value, float4 low, float4 high)
        {
            EditorPrefs.SetFloat($"{key}.x", value.x);
            EditorPrefs.SetFloat($"{key}.y", value.y);
            EditorPrefs.SetFloat($"{key}.z", value.z);
            EditorPrefs.SetFloat($"{key}.w", value.w);
        }

        public float4 Draw(string label, float4 value, float4 low, float4 high)
        {
            return EditorGUILayout.Vector4Field(label, value);
        }
    }
}
