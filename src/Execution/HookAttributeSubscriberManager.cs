using System;
using System.Reflection;
using Appalachia.Core.Attributes;
using Appalachia.Core.Execution.Hooks;
using Appalachia.Utility.Reflection.Delegated;
using Appalachia.Utility.Reflection.Extensions;
using Unity.Profiling;
using UnityEditor;

namespace Appalachia.Core.Execution
{
    [AlwaysInitializeOnLoad]
    public class HookAttributeSubscriberManager
    {
        private const string _PRF_PFX = nameof(HookAttributeSubscriberManager) + ".";

        private static readonly ProfilerMarker _PRF_HookAttributeSubscriberManager =
            new ProfilerMarker(_PRF_PFX + nameof(HookAttributeSubscriberManager));

        static HookAttributeSubscriberManager()
        {
            using (_PRF_HookAttributeSubscriberManager.Auto())
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();

                foreach (var assembly in assemblies)
                {
                    var types = assembly.GetTypes();

                    foreach (var type in types)
                    {
                        var methods = type.GetMethods();

                        foreach (var method in methods)
                        {
                            var attributes = method.GetCustomAttributes();

                            foreach (var attribute in attributes)
                            {
                                var attributeType = attribute.GetType();

                                if (attributeType.InheritsFrom<ExecuteEventBaseAttribute>() && !method.IsStatic())
                                {
                                    throw new NotSupportedException(
                                        $"Cannot use attribute [{attribute.GetType().GetReadableFullName()}] on class [{type.GetReadableFullName()}] non-static method [{method.Name}]."
                                    );
                                }
                                
                                if (attribute is ExecuteOnAwakeAttribute)
                                {
                                    FrameStart.EventDelegates.awake += StaticRoutine.CreateDelegate(method).Invoke;
                                }
                                else if (attribute is ExecuteOnStartAttribute)
                                {
                                    FrameStart.EventDelegates.start += StaticRoutine.CreateDelegate(method).Invoke;
                                }
                                else if (attribute is ExecuteOnEnableAttribute)
                                {
                                    FrameStart.EventDelegates.onEnable += StaticRoutine.CreateDelegate(method).Invoke;
                                }
                                else if (attribute is ExecuteOnUpdateAttribute)
                                {
                                    FrameStart.EventDelegates.update += StaticRoutine.CreateDelegate(method).Invoke;
                                }
                                else if (attribute is ExecuteOnFixedUpdateAttribute)
                                {
                                    FrameStart.EventDelegates.fixedUpdate += StaticRoutine.CreateDelegate(method).Invoke;
                                }
                                else if (attribute is ExecuteOnDisableAttribute)
                                {
                                    FrameStart.EventDelegates.onDisable += StaticRoutine.CreateDelegate(method).Invoke;
                                }
                                else if (attribute is ExecuteOnDestroyAttribute)
                                {
                                    FrameStart.EventDelegates.onDestroy += StaticRoutine.CreateDelegate(method).Invoke;
                                }
                                else if (attribute is ExecuteOnApplicationQuitAttribute)
                                {
                                    FrameStart.EventDelegates.onApplicationQuit += StaticRoutine.CreateDelegate(method).Invoke;
                                }
                            }
                        }
                    }
                }

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
}
