#region

using Appalachia.Utility.Strings;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Preferences.API.PlayerPrefs
{
    public struct quaternion_PPAPI : IPAPI<quaternion>
    {
        private static float3 ToEuler(quaternion quat)
        {
            var q = quat.value;
            float3 angles;

            // roll (x-axis rotation)
            var sinr_cosp = 2.0 * ((q.w * q.x) + (q.y * q.z));
            var cosr_cosp = 1.0 - (2.0 * ((q.x * q.x) + (q.y * q.y)));
            angles.x = (float)math.atan2(sinr_cosp, cosr_cosp);

            // pitch (y-axis rotation)
            var sinp = 2.0 * ((q.w * q.y) - (q.z * q.x));
            if (math.abs(sinp) >= 1.0)
            {
                const double val = math.PI / 2.0;
                angles.y = (float)(math.sign(sinp) >= 0.0 ? val : -val);
            }
            else
            {
                angles.y = (float)math.asin(sinp);
            }

            // yaw (z-axis rotation)
            var siny_cosp = 2.0 * ((q.w * q.z) + (q.x * q.y));
            var cosy_cosp = 1.0 - (2.0 * ((q.y * q.y) + (q.z * q.z)));
            angles.z = (float)math.atan2(siny_cosp, cosy_cosp);

            return angles;
        }

        #region IPAPI<quaternion> Members

        public quaternion Get(string key, quaternion defaultValue, quaternion low, quaternion high)
        {
            var result = quaternion.identity;
            var value = result.value;
            value.x = UnityEngine.PlayerPrefs.GetFloat(ZString.Format("{0}.x", key), defaultValue.value.x);
            value.y = UnityEngine.PlayerPrefs.GetFloat(ZString.Format("{0}.y", key), defaultValue.value.y);
            value.z = UnityEngine.PlayerPrefs.GetFloat(ZString.Format("{0}.z", key), defaultValue.value.z);
            value.w = UnityEngine.PlayerPrefs.GetFloat(ZString.Format("{0}.w", key), defaultValue.value.w);
            result.value = value;
            return result;
        }

        public void Save(string key, quaternion value, quaternion low, quaternion high)
        {
            UnityEngine.PlayerPrefs.SetFloat(ZString.Format("{0}.x", key), value.value.x);
            UnityEngine.PlayerPrefs.SetFloat(ZString.Format("{0}.y", key), value.value.y);
            UnityEngine.PlayerPrefs.SetFloat(ZString.Format("{0}.z", key), value.value.z);
            UnityEngine.PlayerPrefs.SetFloat(ZString.Format("{0}.w", key), value.value.w);
        }

        public quaternion Draw(string key, string label, quaternion value, quaternion low, quaternion high)
        {
            return value;
            /*var euler = ToEuler(value);
            euler = EditorGUILayout.Vector3Field(label, euler);

            return quaternion.Euler(euler.x, euler.y, euler.z);*/
        }

        #endregion
    }
}
