#region

using System;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Reflection.Extensions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

namespace Appalachia.Core.Extensions.Helpers
{
    public static class DebugHelper
    {
        [MenuItem("Tools/Clear Log Entries" + SHC.CTRL_ALT_SHFT_C)]
        private static void ClearLogEntries()
        {
            Debug.ClearDeveloperConsole();
        }

        public static void Log(this GameObject context, string log)
        {
            Log(log, context);
        }

        public static void Log(string log)
        {
#if UNITY_EDITOR
            Debug.Log(log);
#endif
        }

        public static void Log(string log, Object context)
        {
#if UNITY_EDITOR
            Debug.Log(log, context);
#endif
        }

        public static void LogWarning(string log)
        {
#if UNITY_EDITOR
            Debug.LogWarning(log);
#endif
        }

        public static void LogWarning(string log, Object context)
        {
#if UNITY_EDITOR
            Debug.LogWarning(log, context);
#endif
        }

        public static void LogWarning(string log, Exception ex)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"{log}\n{ex}");
#endif
        }

        public static void LogWarning(string log, Exception ex, Object context)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"{log}\n{ex}", context);
#endif
        }

        public static void LogError(string log)
        {
#if UNITY_EDITOR
            Debug.LogError(log);
#endif
        }

        public static void LogError(string log, Object context)
        {
#if UNITY_EDITOR
            Debug.LogError(log, context);
#endif
        }

        public static void LogException<T>(string log, Exception ex)
        {
#if UNITY_EDITOR
            var wrapper = new WrapperException(log, ex);
            Debug.LogException(wrapper);
#endif
        }

        public static void LogException(string log, Exception ex, Object context)
        {
#if UNITY_EDITOR
            var wrapper = new WrapperException(log, ex);
            Debug.LogException(wrapper, context);
#endif
        }

        public static void LogException(this Exception ex, string log)
        {
#if UNITY_EDITOR
            var wrapper = new WrapperException(log, ex);
            Debug.LogException(wrapper);
#endif
        }

        public static void LogException(this Exception ex, string log, Object context)
        {
#if UNITY_EDITOR
            var wrapper = new WrapperException(log, ex);
            Debug.LogException(wrapper, context);
#endif
        }

        private class WrapperException : Exception
        {
            public WrapperException(string message, Exception inner) : base(
                $"{message}: [{inner.GetType().GetReadableName()}] {inner.Message}",
                inner
            )
            {
            }

            public override string ToString()
            {
                return Message;
            }
        }
    }
}
