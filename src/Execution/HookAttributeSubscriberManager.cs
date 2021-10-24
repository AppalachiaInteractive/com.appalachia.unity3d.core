using Appalachia.Core.Attributes;
using Appalachia.Core.Execution.Hooks;
using Appalachia.Utility.Reflection.Delegated;
using Appalachia.Utility.Reflection.Extensions;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Execution
{
    [AlwaysInitializeOnLoad]
    public class HookAttributeSubscriberManager
    {
        private const string _PRF_PFX = nameof(HookAttributeSubscriberManager) + ".";

        private static readonly ProfilerMarker _PRF_HookAttributeSubscriberManager =
            new(_PRF_PFX + nameof(HookAttributeSubscriberManager));

        private static readonly ProfilerMarker _PRF_HookAttributeSubscriberManager_ProcessType =
            new(_PRF_PFX + nameof(HookAttributeSubscriberManager) + ".ProcessType");

        private static readonly ProfilerMarker _PRF_HookAttributeSubscriberManager_ProcessType_ProcessMethod =
            new(_PRF_PFX + nameof(HookAttributeSubscriberManager) + ".ProcessType.ProcessMethod");

        private static readonly ProfilerMarker
            _PRF_HookAttributeSubscriberManager_ProcessType_ProcessMethod_ProcessAttribute = new(_PRF_PFX +
                nameof(HookAttributeSubscriberManager) +
                ".ProcessType.ProcessMethod.ProcessAttribute");

        static HookAttributeSubscriberManager()
        {
            using var scope0 = _PRF_HookAttributeSubscriberManager.Auto();

            var allTypes = ReflectionExtensions.GetAllTypes();

            for (var typeIndex = 0; typeIndex < allTypes.Length; typeIndex++)
            {
                var type = allTypes[typeIndex];
                using var scope2 = _PRF_HookAttributeSubscriberManager_ProcessType.Auto();

                var methods = type.GetMethods_CACHE();

                for (var methodIndex = 0; methodIndex < methods.Length; methodIndex++)
                {
                    var method = methods[methodIndex];
                    using var scope3 = _PRF_HookAttributeSubscriberManager_ProcessType_ProcessMethod.Auto();

                    if (!method.IsStatic_CACHE())
                    {
                        continue;
                    }

                    var attributes = method.GetAttributes_CACHE(false);

                    for (var attributeIndex = 0; attributeIndex < attributes.Length; attributeIndex++)
                    {
                        var attribute = attributes[attributeIndex];
                        using var scope4 =
                            _PRF_HookAttributeSubscriberManager_ProcessType_ProcessMethod_ProcessAttribute
                               .Auto();

                        var attributeType = attribute.GetType();

                        if (!attributeType.InheritsFrom<ExecuteEventBaseAttribute>())
                        {
                            continue;
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
                            FrameStart.EventDelegates.fixedUpdate +=
                                StaticRoutine.CreateDelegate(method).Invoke;
                        }
                        else if (attribute is ExecuteOnPreCullAttribute)
                        {
                            FrameStart.EventDelegates.onPreCull +=
                                StaticRoutine.CreateDelegate<Camera>(method).Invoke;
                        }
                        else if (attribute is ExecuteOnDisableAttribute)
                        {
                            FrameStart.EventDelegates.onDisable +=
                                StaticRoutine.CreateDelegate(method).Invoke;
                        }
                        else if (attribute is ExecuteOnDestroyAttribute)
                        {
                            FrameStart.EventDelegates.onDestroy +=
                                StaticRoutine.CreateDelegate(method).Invoke;
                        }
                        else if (attribute is ExecuteOnApplicationQuitAttribute)
                        {
                            FrameStart.EventDelegates.onApplicationQuit +=
                                StaticRoutine.CreateDelegate(method).Invoke;
                        }
                    }
                }
            }
        }
    }
}
