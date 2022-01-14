#if UNITY_EDITOR

#region

using System;
using Unity.Mathematics;
using UnityEditor;

#endregion

namespace Appalachia.Core.Preferences.API.Editor
{
    public struct float_EPAPI : IPAPI<float>
    {
        #region IPAPI<float> Members

        public float Get(string key, float defaultValue, float low, float high)
        {
            var val = EditorPrefs.GetFloat(key, defaultValue);
            return Math.Abs(low - high) < float.Epsilon ? val : math.clamp(val, low, high);
        }

        public void Save(string key, float value, float low, float high)
        {
            EditorPrefs.SetFloat(
                key,
                Math.Abs(low - high) < float.Epsilon ? value : math.clamp(value, low, high)
            );
        }

        public float Draw(string key, string label, float value, float low, float high)
        {
            var val = Math.Abs(low - high) > float.Epsilon
                ? EditorGUILayout.Slider(label, value, low, high)
                : EditorGUILayout.FloatField(label, value);

            return val;
        }

        #endregion
    }
}

#endif
