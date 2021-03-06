#if UNITY_EDITOR

#region

using UnityEditor;

#endregion

namespace Appalachia.Core.Preferences.API.Editor
{
    public struct bool_EPAPI : IPAPI<bool>
    {
        #region IPAPI<bool> Members

        public bool Get(string key, bool defaultValue, bool low, bool high)
        {
            return EditorPrefs.GetBool(key, defaultValue);
        }

        public void Save(string key, bool value, bool low, bool high)
        {
            EditorPrefs.SetBool(key, value);
        }

        public bool Draw(string key, string label, bool value, bool low, bool high)
        {
            return EditorGUILayout.ToggleLeft(label, value);
        }

        #endregion
    }
}

#endif
