#region

using Appalachia.Utility.Strings;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Preferences.API.PlayerPrefs
{
    public struct float4_PPAPI : IPAPI<float4>
    {
        #region IPAPI<float4> Members

        public float4 Get(string key, float4 defaultValue, float4 low, float4 high)
        {
            var result = float4.zero;
            result.x = UnityEngine.PlayerPrefs.GetFloat(ZString.Format("{0}.x", key), defaultValue.x);
            result.y = UnityEngine.PlayerPrefs.GetFloat(ZString.Format("{0}.y", key), defaultValue.y);
            result.z = UnityEngine.PlayerPrefs.GetFloat(ZString.Format("{0}.z", key), defaultValue.z);
            result.w = UnityEngine.PlayerPrefs.GetFloat(ZString.Format("{0}.w", key), defaultValue.w);
            return result;
        }

        public void Save(string key, float4 value, float4 low, float4 high)
        {
            UnityEngine.PlayerPrefs.SetFloat(ZString.Format("{0}.x", key), value.x);
            UnityEngine.PlayerPrefs.SetFloat(ZString.Format("{0}.y", key), value.y);
            UnityEngine.PlayerPrefs.SetFloat(ZString.Format("{0}.z", key), value.z);
            UnityEngine.PlayerPrefs.SetFloat(ZString.Format("{0}.w", key), value.w);
        }

        public float4 Draw(string key, string label, float4 value, float4 low, float4 high)
        {
            return value;
            /*return EditorGUILayout.Vector4Field(label, value);*/
        }

        #endregion
    }
}
