#region

using Appalachia.Core.Extensions;
using Unity.Mathematics;
using UnityEditor;

#endregion

namespace Appalachia.Core.Preferences.API
{
    public struct quaternion_EPAPI : IEditorPreferenceAPI<quaternion>
    {
        public quaternion Get(string key, quaternion defaultValue, quaternion low, quaternion high)
        {
            var result = quaternion.identity;
            var value = result.value;
            value.x = EditorPrefs.GetFloat($"{key}.x", defaultValue.value.x);
            value.y = EditorPrefs.GetFloat($"{key}.y", defaultValue.value.y);
            value.z = EditorPrefs.GetFloat($"{key}.z", defaultValue.value.z);
            value.w = EditorPrefs.GetFloat($"{key}.w", defaultValue.value.w);
            result.value = value;
            return result;
        }

        public void Save(string key, quaternion value, quaternion low, quaternion high)
        {
            EditorPrefs.SetFloat($"{key}.x", value.value.x);
            EditorPrefs.SetFloat($"{key}.y", value.value.y);
            EditorPrefs.SetFloat($"{key}.z", value.value.z);
            EditorPrefs.SetFloat($"{key}.w", value.value.w);
        }

        public quaternion Draw(string label, quaternion value, quaternion low, quaternion high)
        {
            var euler = value.ToEuler();
            euler = EditorGUILayout.Vector3Field(label, euler);

            return quaternion.Euler(euler.x, euler.y, euler.z);
        }
    }
}
