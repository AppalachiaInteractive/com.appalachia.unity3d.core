#region

using System;
using Appalachia.Core.Attributes;
using Unity.Profiling;
using UnityEditor;

#endregion

namespace Appalachia.Core.Execution
{
#if UNITY_EDITOR
    [InitializeOnLoad]
    public class ExecutionOrderManager
    {
        private const string _PRF_PFX = nameof(ExecutionOrderManager) + ".";

        private static readonly ProfilerMarker _PRF_ScriptOrderManager =
            new ProfilerMarker(_PRF_PFX + nameof(ExecutionOrderManager));

        static ExecutionOrderManager()
        {
            using (_PRF_ScriptOrderManager.Auto())
            {
                var scripts = MonoImporter.GetAllRuntimeMonoScripts();
                for (var i = 0; i < scripts.Length; i++)
                {
                    var monoScript = scripts[i];
                    if (monoScript.GetClass() != null)
                    {
                        var @as = Attribute.GetCustomAttributes(monoScript.GetClass(), typeof(ExecutionOrderAttribute));
                        for (var index = 0; index < @as.Length; index++)
                        {
                            var a = @as[index];
                            var currentOrder = MonoImporter.GetExecutionOrder(monoScript);
                            var newOrder = ((ExecutionOrderAttribute) a).Order;
                            if (currentOrder != newOrder)
                            {
                                MonoImporter.SetExecutionOrder(monoScript, newOrder);
                            }
                        }
                    }
                }
            }
        }
    }
#endif
}
