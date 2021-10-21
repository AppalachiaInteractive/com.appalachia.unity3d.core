#region

using UnityEditor;

#endregion

namespace Appalachia.Core.Preferences.API
{
    public struct string_EPAPI : IEditorPreferenceAPI<string>
    {
        public void Save(PREF<string> pref)
        {
            Save(pref.Key, pref.Value, pref.Low, pref.High);
        }

        public string Draw(PREF<string> pref)
        {
            return Draw(pref.Key, pref.Value, pref.Low, pref.High);
        }

        public string Get(string key, string defaultValue, string low, string high)
        {
            return EditorPrefs.GetString(key, defaultValue);
        }

        public void Save(string key, string value, string low, string high)
        {
            EditorPrefs.SetString(key, value);
        }

        public string Draw(string label, string value, string low, string high)
        {
            return EditorGUILayout.TextField(label, value);
        }
    }
}
