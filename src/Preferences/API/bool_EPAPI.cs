#region

using UnityEditor;

#endregion

namespace Appalachia.Core.Preferences.API
{
    public struct bool_EPAPI : IEditorPreferenceAPI<bool>
    {
        public void Save(PREF<bool> pref)
        {
            Save(pref.Key, pref.Value, pref.Low, pref.High);
        }

        public bool Draw(PREF<bool> pref)
        {
            return Draw(pref.Key, pref.Value, pref.Low, pref.High);
        }

        public bool Get(string key, bool defaultValue, bool low, bool high)
        {
            return EditorPrefs.GetBool(key, defaultValue);
        }

        public void Save(string key, bool value, bool low, bool high)
        {
            EditorPrefs.SetBool(key, value);
        }

        public bool Draw(string label, bool value, bool low, bool high)
        {
            return EditorGUILayout.ToggleLeft(label, value);
        }
    }
}
