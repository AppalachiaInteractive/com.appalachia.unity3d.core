#region

using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Attributes;
using Appalachia.Utility.Reflection.Extensions;
using Unity.Profiling;

#endregion

namespace Appalachia.Core.Execution
{
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
    public class ExecutionOrderManager
    {
        private const string _PRF_PFX = nameof(ExecutionOrderManager) + ".";

        private static readonly ProfilerMarker _PRF_ExecutionOrderManager =
            new(_PRF_PFX + nameof(ExecutionOrderManager));

        static ExecutionOrderManager()
        {
            using (_PRF_ExecutionOrderManager.Auto())
            {
                var scripts = AssetDatabaseManager.GetAllRuntimeMonoScripts();

                for (var i = 0; i < scripts.Count; i++)
                {
                    var monoScript = scripts[i];
                    var typeClass = monoScript.GetClass();

                    if (typeClass == null)
                    {
                        continue;
                    }

                    var atties = typeClass.GetAttributes_CACHE<ExecutionOrderAttribute>();

                    for (var index = 0; index < atties.Length; index++)
                    {
                        var a = atties[index];
                        var currentOrder = UnityEditor.MonoImporter.GetExecutionOrder(monoScript);
                        var newOrder = a.Order;
                        if (currentOrder != newOrder)
                        {
                            UnityEditor.MonoImporter.SetExecutionOrder(monoScript, newOrder);
                        }
                    }
                }
            }
        }
    }
#endif
}
