#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Appalachia.Utility.Reflection.Extensions;


#endregion

namespace Appalachia.Core.Extensions
{
    /// <summary>Various extensions for MethodInfo.</summary>
    public static class MethodInfoExtensions
    {
        private static Dictionary<MethodBase, string> _cache;

        /// <summary>
        ///     Returns the specified method's full name "methodName(argType1 arg1, argType2 arg2, etc)"
        ///     Uses the specified gauntlet to replaces type names, ex: "int" instead of "Int32"
        /// </summary>
        public static string GetReadableName(this MethodBase method, string extensionMethodPrefix)
        {
            if (_cache == null)
            {
                _cache = new Dictionary<MethodBase, string>();
            }

            if (_cache.ContainsKey(method))
            {
                return _cache[method];
            }

            var stringBuilder = new StringBuilder();
            if (method.IsExtensionMethod())
            {
                stringBuilder.Append(extensionMethodPrefix);
            }

            stringBuilder.Append(method.Name);
            if (method.IsGenericMethod)
            {
                var genericArguments = method.GetGenericArguments();
                stringBuilder.Append("<");
                for (var index = 0; index < genericArguments.Length; ++index)
                {
                    if (index != 0)
                    {
                        stringBuilder.Append(", ");
                    }

                    stringBuilder.Append(genericArguments[index].GetReadableName());
                }

                stringBuilder.Append(">");
            }

            stringBuilder.Append("(");
            stringBuilder.Append(method.GetParamsNames());
            stringBuilder.Append(")");
            var result = stringBuilder.ToString();

            _cache.Add(method, result);

            return result;
        }

        /// <summary>
        ///     Returns a string representing the passed method parameters names. Ex "int num, float damage, Transform target"
        /// </summary>
        public static string GetParamsNames(this MethodBase method)
        {
            var parameterInfoArray = method.IsExtensionMethod()
                ? method.GetParameters().Skip(1).ToArray()
                : method.GetParameters();
            var stringBuilder = new StringBuilder();
            var index = 0;
            for (var length = parameterInfoArray.Length; index < length; ++index)
            {
                var parameterInfo = parameterInfoArray[index];
                var niceName = parameterInfo.ParameterType.GetReadableName();
                stringBuilder.Append(niceName);
                stringBuilder.Append(" ");
                stringBuilder.Append(parameterInfo.Name);
                if (index < (length - 1))
                {
                    stringBuilder.Append(", ");
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>Returns the specified method's full name.</summary>
        public static string GetReadableName(this MethodBase method)
        {
            return method.GetReadableName("[ext] ");
        }

        /// <summary>Tests if a method is an extension method.</summary>
        public static bool IsExtensionMethod(this MethodBase method)
        {
            var declaringType = method.DeclaringType;
            return declaringType.IsSealed &&
                   !declaringType.IsGenericType &&
                   !declaringType.IsNested &&
                   method.IsDefined(typeof(ExtensionAttribute), false);
        }


      
    }
}
