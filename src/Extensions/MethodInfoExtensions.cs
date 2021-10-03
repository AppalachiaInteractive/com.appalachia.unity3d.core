#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Appalachia.Utility.Reflection.Extensions;
using Sirenix.Utilities;

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
            var parameterInfoArray = method.IsExtensionMethod() ? method.GetParameters().Skip(1).ToArray() : method.GetParameters();
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

        /// <summary>Determines whether the specified method is an alias.</summary>
        /// <param name="methodInfo">The method to check.</param>
        /// <returns>
        ///     <c>true</c> if the specified method is an alias; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAliasMethod(this MethodInfo methodInfo)
        {
            return methodInfo is MemberAliasMethodInfo;
        }

        /// <summary>
        ///     Returns the original, backing method of an alias method if the method is an alias.
        /// </summary>
        /// <param name="methodInfo">The method to check.</param>
        /// ///
        /// <param name="throwOnNotAliased">if set to <c>true</c> an exception will be thrown if the method is not aliased.</param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentException">The method was not aliased; this only occurs if throwOnNotAliased is true.</exception>
        public static MethodInfo DeAliasMethod(this MethodInfo methodInfo, bool throwOnNotAliased = false)
        {
            if (methodInfo is MemberAliasMethodInfo memberAliasMethodInfo)
            {
                while (memberAliasMethodInfo.AliasedMethod is MemberAliasMethodInfo)
                {
                    memberAliasMethodInfo = memberAliasMethodInfo.AliasedMethod as MemberAliasMethodInfo;
                }

                return memberAliasMethodInfo.AliasedMethod;
            }

            if (throwOnNotAliased)
            {
                throw new ArgumentException("The method " + methodInfo.GetNiceName() + " was not aliased.");
            }

            return methodInfo;
        }
    }
}
