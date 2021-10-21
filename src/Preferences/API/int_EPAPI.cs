#region

using Unity.Mathematics;
using UnityEditor;

#endregion

namespace Appalachia.Core.Preferences.API
{
    public struct int_EPAPI : IEditorPreferenceAPI<int>
    {
        public void Save(PREF<int> pref)
        {
            Save(pref.Key, pref.Value, pref.Low, pref.High);
        }

        public int Draw(PREF<int> pref)
        {
            return Draw(pref.Key, pref.Value, pref.Low, pref.High);
        }

        public int Get(string key, int defaultValue, int low, int high)
        {
            var val = EditorPrefs.GetInt(key, defaultValue);
            return low == high ? val : math.clamp(val, low, high);
        }

        public void Save(string key, int value, int low, int high)
        {
            EditorPrefs.SetInt(key, low == high ? value : math.clamp(value, low, high));
        }

        public int Draw(string label, int value, int low, int high)
        {
            var val = low != high
                ? EditorGUILayout.IntSlider(label, value, low, high)
                : EditorGUILayout.IntField(label, value);

            return val;
        }
    }
}
