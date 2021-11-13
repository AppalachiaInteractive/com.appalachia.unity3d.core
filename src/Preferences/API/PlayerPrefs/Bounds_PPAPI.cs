#region

using UnityEngine;

#endregion

namespace Appalachia.Core.Preferences.API.PlayerPrefs
{
    public struct Bounds_PPAPI : IPAPI<Bounds>
    {
        public Bounds Get(string key, Bounds defaultValue, Bounds low, Bounds high)
        {
            var result = default(Bounds);

            var center = result.center;
            var size = result.size;

            center.x = UnityEngine.PlayerPrefs.GetFloat($"{key}.center.x", defaultValue.center.x);
            center.y = UnityEngine.PlayerPrefs.GetFloat($"{key}.center.y", defaultValue.center.y);
            center.z = UnityEngine.PlayerPrefs.GetFloat($"{key}.center.z", defaultValue.center.z);
            size.x = UnityEngine.PlayerPrefs.GetFloat($"{key}.size.x",     defaultValue.size.x);
            size.y = UnityEngine.PlayerPrefs.GetFloat($"{key}.size.y",     defaultValue.size.y);
            size.z = UnityEngine.PlayerPrefs.GetFloat($"{key}.size.z",     defaultValue.size.z);

            result.center = center;
            result.size = size;
            return result;
        }

        public void Save(string key, Bounds value, Bounds low, Bounds high)
        {
            UnityEngine.PlayerPrefs.SetFloat($"{key}.center.x", value.center.x);
            UnityEngine.PlayerPrefs.SetFloat($"{key}.center.y", value.center.y);
            UnityEngine.PlayerPrefs.SetFloat($"{key}.center.z", value.center.z);
            UnityEngine.PlayerPrefs.SetFloat($"{key}.size.x",   value.size.x);
            UnityEngine.PlayerPrefs.SetFloat($"{key}.size.y",   value.size.y);
            UnityEngine.PlayerPrefs.SetFloat($"{key}.size.z",   value.size.z);
        }

        public Bounds Draw(string key, string label, Bounds value, Bounds low, Bounds high)
        {
            return value;
            /*return EditorGUILayout.BoundsField(label, value);*/
        }
    }
}
