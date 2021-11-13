#region

using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Preferences.API.PlayerPrefs
{
    public struct float3_PPAPI : IPAPI<float3>
    {
        public float3 Get(string key, float3 defaultValue, float3 low, float3 high)
        {
            var result = float3.zero;
            result.x = UnityEngine.PlayerPrefs.GetFloat($"{key}.x", defaultValue.x);
            result.y = UnityEngine.PlayerPrefs.GetFloat($"{key}.y", defaultValue.y);
            result.z = UnityEngine.PlayerPrefs.GetFloat($"{key}.z", defaultValue.z);
            return result;
        }

        public void Save(string key, float3 value, float3 low, float3 high)
        {
            UnityEngine.PlayerPrefs.SetFloat($"{key}.x", value.x);
            UnityEngine.PlayerPrefs.SetFloat($"{key}.y", value.y);
            UnityEngine.PlayerPrefs.SetFloat($"{key}.z", value.z);
        }

        public float3 Draw(string key, string label, float3 value, float3 low, float3 high)
        {
            return value;
            /*return EditorGUILayout.Vector3Field(label, value);*/
        }
    }
}
