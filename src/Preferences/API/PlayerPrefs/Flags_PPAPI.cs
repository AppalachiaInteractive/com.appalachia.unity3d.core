#region

using System;
using System.Globalization;

#endregion

namespace Appalachia.Core.Preferences.API.PlayerPrefs
{
    public struct Flags_PPAPI<T> : IPAPI<T>
    {
        public T Get(string key, T defaultValue, T low, T high)
        {
            var intDefaultValue = Convert.ToInt32(defaultValue);

            var intValue = UnityEngine.PlayerPrefs.GetInt(key, intDefaultValue);

            var enumType = typeof(T);
            var underlyingType = Enum.GetUnderlyingType(enumType);
            var obj = Convert.ChangeType(intValue, underlyingType, CultureInfo.InvariantCulture);
            var result = Enum.ToObject(enumType, obj);

            return (T) result;
        }

        public void Save(string key, T value, T low, T high)
        {
            var intValue = Convert.ToInt32(value);

            UnityEngine.PlayerPrefs.SetInt(key, intValue);
        }

        public T Draw(string key, string label, T value, T low, T high)
        {
            return value;
            /*var valueObj = EditorGUILayout.EnumFlagsField(label, (Enum) (object) value);

            return (T) (object) valueObj;*/
        }
    }
}
