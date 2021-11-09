#region

using System;
using System.Diagnostics;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Reflection.Extensions;
using UnityEditor;
using Debug = UnityEngine.Debug;

#endregion

namespace Appalachia.Core.Extensions.Helpers
{
    public static class DebugHelper
    {
        [UnityEditor.MenuItem(PKG.Menu.Appalachia.Tools.Base + "Clear Log Entries" + SHC.CTRL_ALT_SHFT_C)]
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

            [DebuggerStepThrough] public override string ToString()
            {
                return Message;
            }
        }
    }
}
