#region

using Appalachia.Utility.Strings;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Preferences.API.PlayerPrefs
{
    public struct float3_PPAPI : IPAPI<float3>
    {
        #region IPAPI<float3> Members

        public float3 Get(string key, float3 defaultValue, float3 low, float3 high)
        {
            var result = float3.zero;
            result.x = UnityEngine.PlayerPrefs.GetFloat(ZString.Format("{0}.x", key), defaultValue.x);
            result.y = UnityEngine.PlayerPrefs.GetFloat(ZString.Format("{0}.y", key), defaultValue.y);
            result.z = UnityEngine.PlayerPrefs.GetFloat(ZString.Format("{0}.z", key), defaultValue.z);
            return result;
        }

        public void Save(string key, float3 value, float3 low, float3 high)
        {
            UnityEngine.PlayerPrefs.SetFloat(ZString.Format("{0}.x", key), value.x);
            UnityEngine.PlayerPrefs.SetFloat(ZString.Format("{0}.y", key), value.y);
            UnityEngine.PlayerPrefs.SetFloat(ZString.Format("{0}.z", key), value.z);
        }

        public float3 Draw(string key, string label, float3 value, float3 low, float3 high)
        {
            return value;
            /*return EditorGUILayout.Vector3Field(label, value);*/
        }

        #endregion
    }
}
