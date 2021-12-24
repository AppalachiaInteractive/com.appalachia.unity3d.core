#region

using Appalachia.Utility.Strings;
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

            center.x = UnityEngine.PlayerPrefs.GetFloat(
                ZString.Format("{0}.center.x", key),
                defaultValue.center.x
            );
            center.y = UnityEngine.PlayerPrefs.GetFloat(
                ZString.Format("{0}.center.y", key),
                defaultValue.center.y
            );
            center.z = UnityEngine.PlayerPrefs.GetFloat(
                ZString.Format("{0}.center.z", key),
                defaultValue.center.z
            );
            size.x = UnityEngine.PlayerPrefs.GetFloat(ZString.Format("{0}.size.x", key), defaultValue.size.x);
            size.y = UnityEngine.PlayerPrefs.GetFloat(ZString.Format("{0}.size.y", key), defaultValue.size.y);
            size.z = UnityEngine.PlayerPrefs.GetFloat(ZString.Format("{0}.size.z", key), defaultValue.size.z);

            result.center = center;
            result.size = size;
            return result;
        }

        public void Save(string key, Bounds value, Bounds low, Bounds high)
        {
            UnityEngine.PlayerPrefs.SetFloat(ZString.Format("{0}.center.x", key), value.center.x);
            UnityEngine.PlayerPrefs.SetFloat(ZString.Format("{0}.center.y", key), value.center.y);
            UnityEngine.PlayerPrefs.SetFloat(ZString.Format("{0}.center.z", key), value.center.z);
            UnityEngine.PlayerPrefs.SetFloat(ZString.Format("{0}.size.x",   key), value.size.x);
            UnityEngine.PlayerPrefs.SetFloat(ZString.Format("{0}.size.y",   key), value.size.y);
            UnityEngine.PlayerPrefs.SetFloat(ZString.Format("{0}.size.z",   key), value.size.z);
        }

        public Bounds Draw(string key, string label, Bounds value, Bounds low, Bounds high)
        {
            return value;
            /*return EditorGUILayout.BoundsField(label, value);*/
        }
    }
}
