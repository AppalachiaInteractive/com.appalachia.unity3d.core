#if UNITY_EDITOR

#region

using Appalachia.Utility.Strings;
using Unity.Mathematics;
using UnityEditor;

#endregion

namespace Appalachia.Core.Preferences.API.Editor
{
    public struct float4_EPAPI : IPAPI<float4>
    {
        #region IPAPI<float4> Members

        public float4 Get(string key, float4 defaultValue, float4 low, float4 high)
        {
            var result = float4.zero;
            result.x = EditorPrefs.GetFloat(ZString.Format("{0}.x", key), defaultValue.x);
            result.y = EditorPrefs.GetFloat(ZString.Format("{0}.y", key), defaultValue.y);
            result.z = EditorPrefs.GetFloat(ZString.Format("{0}.z", key), defaultValue.z);
            result.w = EditorPrefs.GetFloat(ZString.Format("{0}.w", key), defaultValue.w);
            return result;
        }

        public void Save(string key, float4 value, float4 low, float4 high)
        {
            EditorPrefs.SetFloat(ZString.Format("{0}.x", key), value.x);
            EditorPrefs.SetFloat(ZString.Format("{0}.y", key), value.y);
            EditorPrefs.SetFloat(ZString.Format("{0}.z", key), value.z);
            EditorPrefs.SetFloat(ZString.Format("{0}.w", key), value.w);
        }

        public float4 Draw(string key, string label, float4 value, float4 low, float4 high)
        {
            return EditorGUILayout.Vector4Field(label, value);
        }

        #endregion
    }
}

#endif
