#region

using Unity.Mathematics;
using UnityEditor;

#endregion

namespace Appalachia.Core.Preferences.API
{
    public struct float_EPAPI : IEditorPreferenceAPI<float>
    {
        public float Get(string key, float defaultValue, float low, float high)
        {
            var val = EditorPrefs.GetFloat(key, defaultValue);
            return low == high ? val : math.clamp(val, low, high);
        }

        public void Save(string key, float value, float low, float high)
        {
            EditorPrefs.SetFloat(key, low == high ? value : math.clamp(value, low, high));
        }

        public float Draw(string label, float value, float low, float high)
        {
            var val = low != high
                ? EditorGUILayout.Slider(label, value, low, high)
                : EditorGUILayout.FloatField(label, value);

            return val;
        }
    }
}
