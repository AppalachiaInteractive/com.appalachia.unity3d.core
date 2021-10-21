#region

using Unity.Mathematics;
using UnityEditor;

#endregion

namespace Appalachia.Core.Preferences.API
{
    public struct float2_EPAPI : IEditorPreferenceAPI<float2>
    {
        public void Save(PREF<float2> pref)
        {
            Save(pref.Key, pref.Value, pref.Low, pref.High);
        }

        public float2 Draw(PREF<float2> pref)
        {
            return Draw(pref.Key, pref.Value, pref.Low, pref.High);
        }

        public float2 Get(string key, float2 defaultValue, float2 low, float2 high)
        {
            var result = float2.zero;
            result.x = EditorPrefs.GetFloat($"{key}.x", defaultValue.x);
            result.y = EditorPrefs.GetFloat($"{key}.y", defaultValue.y);
            return result;
        }

        public void Save(string key, float2 value, float2 low, float2 high)
        {
            EditorPrefs.SetFloat($"{key}.x", value.x);
            EditorPrefs.SetFloat($"{key}.y", value.y);
        }

        public float2 Draw(string label, float2 value, float2 low, float2 high)
        {
            return EditorGUILayout.Vector2Field(label, value);
        }
    }
}
