namespace Appalachia.Core.Preferences.API.PlayerPrefs
{
    public struct string_PPAPI : IPAPI<string>
    {
        #region IPAPI<string> Members

        public string Get(string key, string defaultValue, string low, string high)
        {
            return UnityEngine.PlayerPrefs.GetString(key, defaultValue);
        }

        public void Save(string key, string value, string low, string high)
        {
            UnityEngine.PlayerPrefs.SetString(key, value);
        }

        public string Draw(string key, string label, string value, string low, string high)
        {
            //return EditorGUILayout.TextField(label, value);
            return value;
        }

        #endregion
    }
}
