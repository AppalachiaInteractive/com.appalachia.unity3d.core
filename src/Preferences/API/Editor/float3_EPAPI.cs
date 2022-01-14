#if UNITY_EDITOR

#region

using Appalachia.Utility.Strings;
using Unity.Mathematics;
using UnityEditor;

#endregion

namespace Appalachia.Core.Preferences.API.Editor
{
    public struct float3_EPAPI : IPAPI<float3>
    {
        #region IPAPI<float3> Members

        public float3 Get(string key, float3 defaultValue, float3 low, float3 high)
        {
            var result = float3.zero;
            result.x = EditorPrefs.GetFloat(ZString.Format("{0}.x", key), defaultValue.x);
            result.y = EditorPrefs.GetFloat(ZString.Format("{0}.y", key), defaultValue.y);
            result.z = EditorPrefs.GetFloat(ZString.Format("{0}.z", key), defaultValue.z);
            return result;
        }

        public void Save(string key, float3 value, float3 low, float3 high)
        {
            EditorPrefs.SetFloat(ZString.Format("{0}.x", key), value.x);
            EditorPrefs.SetFloat(ZString.Format("{0}.y", key), value.y);
            EditorPrefs.SetFloat(ZString.Format("{0}.z", key), value.z);
        }

        public float3 Draw(string key, string label, float3 value, float3 low, float3 high)
        {
            return EditorGUILayout.Vector3Field(label, value);
        }

        #endregion
    }
}

#endif
