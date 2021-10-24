#region

using System;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Reflection.Extensions;
using UnityEditor;
using UnityEngine;

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
