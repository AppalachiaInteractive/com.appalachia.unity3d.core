#region

using Appalachia.Core.Assets;
using Appalachia.Core.Attributes;
using Appalachia.Utility.Reflection.Extensions;
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

        private static readonly ProfilerMarker _PRF_ExecutionOrderManager =
            new(_PRF_PFX + nameof(ExecutionOrderManager));

        static ExecutionOrderManager()
        {
            using (_PRF_ExecutionOrderManager.Auto())
            {
                var scripts = AssetDatabaseManager.GetAllMonoScripts();

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
                        var currentOrder = MonoImporter.GetExecutionOrder(monoScript);
                        var newOrder = a.Order;
                        if (currentOrder != newOrder)
                        {
                            MonoImporter.SetExecutionOrder(monoScript, newOrder);
                        }
                    }
                }
            }
        }
    }
#endif
}
