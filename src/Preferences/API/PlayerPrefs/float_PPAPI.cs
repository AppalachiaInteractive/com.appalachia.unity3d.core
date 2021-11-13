#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Preferences.API.PlayerPrefs
{
    public struct float_PPAPI : IPAPI<float>
    {
        public float Get(string key, float defaultValue, float low, float high)
        {
            var val = UnityEngine.PlayerPrefs.GetFloat(key, defaultValue);
            return Math.Abs(low - high) < float.Epsilon ? val : math.clamp(val, low, high);
        }

        public void Save(string key, float value, float low, float high)
        {
            UnityEngine.PlayerPrefs.SetFloat(
                key,
                Math.Abs(low - high) < float.Epsilon ? value : math.clamp(value, low, high)
            );
        }

        public float Draw(string key, string label, float value, float low, float high)
        {
            return value;
            /*var val = Math.Abs(low - high) > float.Epsilon
                ? EditorGUILayout.Slider(label, value, low, high)
                : EditorGUILayout.FloatField(label, value);

            return val;*/
        }
    }
}
