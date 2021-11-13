#region

using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Preferences.API.PlayerPrefs
{
    public struct float4_PPAPI : IPAPI<float4>
    {
        public float4 Get(string key, float4 defaultValue, float4 low, float4 high)
        {
            var result = float4.zero;
            result.x = UnityEngine.PlayerPrefs.GetFloat($"{key}.x", defaultValue.x);
            result.y = UnityEngine.PlayerPrefs.GetFloat($"{key}.y", defaultValue.y);
            result.z = UnityEngine.PlayerPrefs.GetFloat($"{key}.z", defaultValue.z);
            result.w = UnityEngine.PlayerPrefs.GetFloat($"{key}.w", defaultValue.w);
            return result;
        }

        public void Save(string key, float4 value, float4 low, float4 high)
        {
            UnityEngine.PlayerPrefs.SetFloat($"{key}.x", value.x);
            UnityEngine.PlayerPrefs.SetFloat($"{key}.y", value.y);
            UnityEngine.PlayerPrefs.SetFloat($"{key}.z", value.z);
            UnityEngine.PlayerPrefs.SetFloat($"{key}.w", value.w);
        }

        public float4 Draw(string key, string label, float4 value, float4 low, float4 high)
        {            
            return value;
            /*return EditorGUILayout.Vector4Field(label, value);*/
        }
    }
}
