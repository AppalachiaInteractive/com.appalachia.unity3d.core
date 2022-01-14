#region

using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Preferences.API.PlayerPrefs
{
    public struct int_PPAPI : IPAPI<int>
    {
        #region IPAPI<int> Members

        public int Get(string key, int defaultValue, int low, int high)
        {
            var val = UnityEngine.PlayerPrefs.GetInt(key, defaultValue);
            return low == high ? val : math.clamp(val, low, high);
        }

        public void Save(string key, int value, int low, int high)
        {
            UnityEngine.PlayerPrefs.SetInt(key, low == high ? value : math.clamp(value, low, high));
        }

        public int Draw(string key, string label, int value, int low, int high)
        {
            return value;
            /*var val = low != high
                ? EditorGUILayout.IntSlider(label, value, low, high)
                : EditorGUILayout.IntField(label, value);

            return val;*/
        }

        #endregion
    }
}
