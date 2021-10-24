using System;
using System.Linq;

namespace Appalachia.Core.Extensions
{
    public static class EnumExtension
    {
        public static T Parse<T>(string name)
            where T : struct, IConvertible
        {
#if NET_4_6
    T e;
    Enum.TryParse(name, out e);
    return e;
#else
            try
            {
                return (T) Enum.Parse(typeof(T), name);
            }
            catch
            {
                return default;
            }
#endif
        }

        public static T[] GetValuesAsInstances<T>(Type type)
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
            /*var names = Enum.GetNames(type);
            var array = (T[]) Array.CreateInstance(typeof(T), names.Length);
            var assembly = type.Assembly.FullName;
            var space = type.Namespace;
            for (int i = 0, n = names.Length; i < n; ++i) {
                var fullName = space != null ? space + "." + names[i] : names[i];
                array[i] = (T) Activator.CreateInstance(assembly, fullName).Unwrap();
            }
            return array;*/
        }
    }
}
