namespace Appalachia.Core.Preferences.API.PlayerPrefs
{
    internal static class Helpers
    {
        public static void SetBool(string key, bool value)
        {
            UnityEngine.PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        public static bool GetBool(string key, bool defaultValue)
        {
            if (!UnityEngine.PlayerPrefs.HasKey(key))
            {
                return defaultValue;
            }

            var result = UnityEngine.PlayerPrefs.GetInt(key);

            return result >= 1;
        }
    }
}