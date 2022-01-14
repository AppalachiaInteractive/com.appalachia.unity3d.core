#region

using System;

#endregion

namespace Appalachia.Core.Preferences.API.PlayerPrefs
{
    public struct double_PPAPI : IPAPI<double>
    {
        private static double Round(float d)
        {
            return Math.Round(d, 4, MidpointRounding.AwayFromZero);
        }

        private static float Round(double d)
        {
            return (float)Math.Round(d, 4, MidpointRounding.AwayFromZero);
        }

        #region IPAPI<double> Members

        public double Get(string key, double defaultValue, double low, double high)
        {
            return Round(UnityEngine.PlayerPrefs.GetFloat(key, Round(defaultValue)));
        }

        public void Save(string key, double value, double low, double high)
        {
            UnityEngine.PlayerPrefs.SetFloat(key, Round(value));
        }

        public double Draw(string key, string label, double value, double low, double high)
        {
            return value;
            /*var val = Math.Abs(low - high) > float.Epsilon
                ? EditorGUILayout.Slider(label, Round(value), (float) low, (float) high)
                : EditorGUILayout.DoubleField(label, Round(value));

            return Round(val);*/
        }

        #endregion
    }
}
