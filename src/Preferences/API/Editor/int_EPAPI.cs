#if UNITY_EDITOR

#region

using Unity.Mathematics;
using UnityEditor;

#endregion

namespace Appalachia.Core.Preferences.API.Editor
{
    public struct int_EPAPI : IPAPI<int>
    {
        #region IPAPI<int> Members

        public int Get(string key, int defaultValue, int low, int high)
        {
            var val = EditorPrefs.GetInt(key, defaultValue);
            return low == high ? val : math.clamp(val, low, high);
        }

        public void Save(string key, int value, int low, int high)
        {
            EditorPrefs.SetInt(key, low == high ? value : math.clamp(value, low, high));
        }

        public int Draw(string key, string label, int value, int low, int high)
        {
            var val = low != high
                ? EditorGUILayout.IntSlider(label, value, low, high)
                : EditorGUILayout.IntField(label, value);

            return val;
        }

        #endregion
    }
}

#endif
