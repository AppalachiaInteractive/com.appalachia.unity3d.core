#if UNITY_EDITOR

#region

using UnityEditor;

#endregion

namespace Appalachia.Core.Preferences.API.Editor
{
    public struct string_EPAPI : IPAPI<string>
    {
        #region IPAPI<string> Members

        public string Get(string key, string defaultValue, string low, string high)
        {
            return EditorPrefs.GetString(key, defaultValue);
        }

        public void Save(string key, string value, string low, string high)
        {
            EditorPrefs.SetString(key, value);
        }

        public string Draw(string key, string label, string value, string low, string high)
        {
            return EditorGUILayout.TextField(label, value);
        }

        #endregion
    }
}

#endif
