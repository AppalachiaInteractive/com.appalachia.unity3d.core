#if UNITY_EDITOR

#region

using Appalachia.Utility.Strings;
using UnityEditor;
using UnityEngine;

#endregion

namespace Appalachia.Core.Preferences.API.Editor
{
    public struct Bounds_EPAPI : IPAPI<Bounds>
    {
        public Bounds Get(string key, Bounds defaultValue, Bounds low, Bounds high)
        {
            var result = default(Bounds);

            var center = result.center;
            var size = result.size;

            center.x = EditorPrefs.GetFloat(ZString.Format("{0}.center.x", key), defaultValue.center.x);
            center.y = EditorPrefs.GetFloat(ZString.Format("{0}.center.y", key), defaultValue.center.y);
            center.z = EditorPrefs.GetFloat(ZString.Format("{0}.center.z", key), defaultValue.center.z);
            size.x = EditorPrefs.GetFloat(ZString.Format("{0}.size.x",     key), defaultValue.size.x);
            size.y = EditorPrefs.GetFloat(ZString.Format("{0}.size.y",     key), defaultValue.size.y);
            size.z = EditorPrefs.GetFloat(ZString.Format("{0}.size.z",     key), defaultValue.size.z);

            result.center = center;
            result.size = size;
            return result;
        }

        public void Save(string key, Bounds value, Bounds low, Bounds high)
        {
            EditorPrefs.SetFloat(ZString.Format("{0}.center.x", key), value.center.x);
            EditorPrefs.SetFloat(ZString.Format("{0}.center.y", key), value.center.y);
            EditorPrefs.SetFloat(ZString.Format("{0}.center.z", key), value.center.z);
            EditorPrefs.SetFloat(ZString.Format("{0}.size.x",   key), value.size.x);
            EditorPrefs.SetFloat(ZString.Format("{0}.size.y",   key), value.size.y);
            EditorPrefs.SetFloat(ZString.Format("{0}.size.z",   key), value.size.z);
        }

        public Bounds Draw(string key, string label, Bounds value, Bounds low, Bounds high)
        {
            return EditorGUILayout.BoundsField(label, value);
        }
    }
}

#endif