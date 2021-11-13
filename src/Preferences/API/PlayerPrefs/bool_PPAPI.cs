#region

#endregion

namespace Appalachia.Core.Preferences.API.PlayerPrefs
{
    public struct bool_PPAPI : IPAPI<bool>
    {
        public bool Get(string key, bool defaultValue, bool low, bool high)
        {
            return Helpers.GetBool(key, defaultValue);
        }

        public void Save(string key, bool value, bool low, bool high)
        {
            Helpers.SetBool(key, value);
        }

        public bool Draw(string key, string label, bool value, bool low, bool high)
        {            
            return value;
            /*return EditorGUILayout.ToggleLeft(label, value);*/
        }
    }
}
