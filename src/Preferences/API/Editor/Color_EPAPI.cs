#if UNITY_EDITOR

#region

using UnityEditor;
using UnityEngine;

#endregion

namespace Appalachia.Core.Preferences.API.Editor
{
    public struct Color_EPAPI : IPAPI<Color>
    {
        public static uint ToHex(Color c)
        {
            return ((uint)(c.a * 255) << 24) |
                   ((uint)(c.r * 255) << 16) |
                   ((uint)(c.g * 255) << 8) |
                   (uint)(c.b * 255);
        }

        public static Color ToRGBA(uint hex)
        {
            return new(
                ((hex >> 16) & 0xff) / 255f, // r
                ((hex >> 8) & 0xff) / 255f,  // g
                (hex & 0xff) / 255f,         // b
                ((hex >> 24) & 0xff) / 255f  // a
            );
        }

        private static Color GetColor(string key, Color defaultValue)
        {
            var dv = (int)ToHex(defaultValue);
            var value = EditorPrefs.GetInt(key, dv);
            var converted = ToRGBA((uint)value);
            return converted;
        }

        #region IPAPI<Color> Members

        public Color Get(string key, Color defaultValue, Color low, Color high)
        {
            return GetColor(key, defaultValue);
        }

        public void Save(string key, Color value, Color low, Color high)
        {
            var hex = (int)ToHex(value);
            EditorPrefs.SetInt(key, hex);
        }

        public Color Draw(string key, string label, Color value, Color low, Color high)
        {
            var color = EditorGUILayout.ColorField(label, value);

            return color;
        }

        #endregion
    }
}

#endif
