#region

using System;
using System.Globalization;
using UnityEditor;

#endregion

namespace Appalachia.Core.Preferences.API
{
    public struct Flags_EPAPI<T> : IEditorPreferenceAPI<T>
    {
        public void Save(PREF<T> pref)
        {
            Save(pref.Key, pref.Value, pref.Low, pref.High);
        }

        public T Draw(PREF<T> pref)
        {
            return Draw(pref.Key, pref.Value, pref.Low, pref.High);
        }

        public T Get(string key, T defaultValue, T low, T high)
        {
            var intDefaultValue = Convert.ToInt32(defaultValue);

            var intValue = EditorPrefs.GetInt(key, intDefaultValue);

            var enumType = typeof(T);
            var underlyingType = Enum.GetUnderlyingType(enumType);
            var obj = Convert.ChangeType(intValue, underlyingType, CultureInfo.InvariantCulture);
            var result = Enum.ToObject(enumType, obj);

            return (T) result;
        }

        public void Save(string key, T value, T low, T high)
        {
            var intValue = Convert.ToInt32(value);

            EditorPrefs.SetInt(key, intValue);
        }

        public T Draw(string label, T value, T low, T high)
        {
            var valueObj = EditorGUILayout.EnumFlagsField(label, (Enum) (object) value);

            return (T) (object) valueObj;
        }
    }
}
