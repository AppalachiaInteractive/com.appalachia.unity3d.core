#region

using Appalachia.Utility.Strings;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Preferences.API.PlayerPrefs
{
    public struct float2_PPAPI : IPAPI<float2>
    {
        #region IPAPI<float2> Members

        public float2 Get(string key, float2 defaultValue, float2 low, float2 high)
        {
            var result = float2.zero;
            result.x = UnityEngine.PlayerPrefs.GetFloat(ZString.Format("{0}.x", key), defaultValue.x);
            result.y = UnityEngine.PlayerPrefs.GetFloat(ZString.Format("{0}.y", key), defaultValue.y);
            return result;
        }

        public void Save(string key, float2 value, float2 low, float2 high)
        {
            UnityEngine.PlayerPrefs.SetFloat(ZString.Format("{0}.x", key), value.x);
            UnityEngine.PlayerPrefs.SetFloat(ZString.Format("{0}.y", key), value.y);
        }

        public float2 Draw(string key, string label, float2 value, float2 low, float2 high)
        {
            return value;

            //return EditorGUILayout.Vector2Field(label, value);
        }

        #endregion
    }
}
