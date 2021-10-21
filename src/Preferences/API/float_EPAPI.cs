#region

using System;
using Unity.Mathematics;
using UnityEditor;

#endregion

namespace Appalachia.Core.Preferences.API
{
    public struct float_EPAPI : IEditorPreferenceAPI<float>
    {
        public void Save(PREF<float> pref)
        {
            Save(pref.Key, pref.Value, pref.Low, pref.High);
        }

        public float Draw(PREF<float> pref)
        {
            return Draw(pref.Key, pref.Value, pref.Low, pref.High);
        }

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

        public float Draw(string label, float value, float low, float high)
        {
            var val = Math.Abs(low - high) > float.Epsilon
                ? EditorGUILayout.Slider(label, value, low, high)
                : EditorGUILayout.FloatField(label, value);

            return val;
        }
    }
}
