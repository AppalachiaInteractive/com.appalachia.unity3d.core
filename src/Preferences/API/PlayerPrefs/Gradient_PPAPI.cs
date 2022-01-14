#region

using Appalachia.Utility.Strings;
using UnityEngine;

#endregion

namespace Appalachia.Core.Preferences.API.PlayerPrefs
{
    public struct Gradient_PPAPI : IPAPI<Gradient>
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

        private static Gradient GetGradient(string key, Gradient defaultValue)
        {
            var gradient = new Gradient();

            var modeKey = ZString.Format("{0}.mode", key);
            gradient.mode = Helpers.GetBool(modeKey, true) ? GradientMode.Blend : GradientMode.Fixed;

            var colorBaseKey = ZString.Format("{0}.color", key);
            var alphaBaseKey = ZString.Format("{0}.alpha", key);

            var colorCount = UnityEngine.PlayerPrefs.GetInt(colorBaseKey, 2);
            var alphaCount = UnityEngine.PlayerPrefs.GetInt(alphaBaseKey, 2);

            var colorKeys = new GradientColorKey[colorCount];
            var alphaKeys = new GradientAlphaKey[alphaCount];

            for (var i = 0; i < colorCount; i++)
            {
                var colorKey = ZString.Format("{0}.{1}.value", colorBaseKey, i);
                var timeKey = ZString.Format("{0}.{1}.time",   colorBaseKey, i);

                var color = UnityEngine.PlayerPrefs.GetInt(
                    colorKey,
                    (int)ToHex(i == 0 ? Color.black : Color.white)
                );
                var time = UnityEngine.PlayerPrefs.GetFloat(timeKey, i);

                colorKeys[i] = new GradientColorKey(ToRGBA((uint)color), time);
            }

            for (var i = 0; i < alphaCount; i++)
            {
                var alphaKey = ZString.Format("{0}.{1}.value", alphaBaseKey, i);
                var timeKey = ZString.Format("{0}.{1}.time",   alphaBaseKey, i);

                var alpha = UnityEngine.PlayerPrefs.GetFloat(alphaKey, 1.0f);
                var time = UnityEngine.PlayerPrefs.GetFloat(timeKey,   i);

                alphaKeys[i] = new GradientAlphaKey(alpha, time);
            }

            gradient.SetKeys(colorKeys, alphaKeys);

            return gradient;
        }

        #region IPAPI<Gradient> Members

        public Gradient Get(string key, Gradient defaultValue, Gradient low, Gradient high)
        {
            return GetGradient(key, defaultValue);
        }

        public void Save(string key, Gradient value, Gradient low, Gradient high)
        {
            var modeKey = ZString.Format("{0}.mode", key);
            Helpers.SetBool(modeKey, value.mode == GradientMode.Blend);

            var colorBaseKey = ZString.Format("{0}.color", key);
            var alphaBaseKey = ZString.Format("{0}.alpha", key);

            UnityEngine.PlayerPrefs.SetInt(colorBaseKey, value.colorKeys.Length);
            UnityEngine.PlayerPrefs.SetInt(alphaBaseKey, value.alphaKeys.Length);

            for (var i = 0; i < value.colorKeys.Length; i++)
            {
                var colorKey = ZString.Format("{0}.{1}.value", colorBaseKey, i);
                var timeKey = ZString.Format("{0}.{1}.time",   colorBaseKey, i);

                UnityEngine.PlayerPrefs.SetInt(colorKey, (int)ToHex(value.colorKeys[i].color));
                UnityEngine.PlayerPrefs.SetFloat(timeKey, value.colorKeys[i].time);
            }

            for (var i = 0; i < value.alphaKeys.Length; i++)
            {
                var alphaKey = ZString.Format("{0}.{1}.value", alphaBaseKey, i);
                var timeKey = ZString.Format("{0}.{1}.time",   alphaBaseKey, i);

                UnityEngine.PlayerPrefs.SetFloat(alphaKey, value.alphaKeys[i].alpha);
                UnityEngine.PlayerPrefs.SetFloat(timeKey,  value.alphaKeys[i].time);
            }
        }

        public Gradient Draw(string key, string label, Gradient value, Gradient low, Gradient high)
        {
            return value;
            /*var gradient = EditorGUILayout.GradientField(label, value);

            return gradient;*/
        }

        #endregion
    }
}
