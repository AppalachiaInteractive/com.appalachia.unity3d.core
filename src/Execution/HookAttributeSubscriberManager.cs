using System.Reflection;
using Appalachia.Core.Attributes;
using Appalachia.Core.Execution.Hooks;
using Appalachia.Utility.Reflection.Delegated;
using Appalachia.Utility.Reflection.Extensions;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Execution
{
    [CallStaticConstructorInEditor]
    public class HookAttributeSubscriberManager
    {
        static HookAttributeSubscriberManager()
        {
            using var scope0 = _PRF_HookAttributeSubscriberManager.Auto();

            var appalachiaTypes = ReflectionExtensions.GetAppalachiaTypes_CACHED();

            for (var typeIndex = 0; typeIndex < appalachiaTypes.Length; typeIndex++)
            {
                var type = appalachiaTypes[typeIndex];

                using var scope5 = _PRF_HookAttributeSubscriberManager_ProcessType.Auto();

                var methods = type.GetMethods_CACHE(
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
                );

                for (var methodIndex = 0; methodIndex < methods.Length; methodIndex++)
                {
                    var method = methods[methodIndex];
                    using var scope20 = _PRF_HookAttributeSubscriberManager_ProcessType_ProcessMethod.Auto();

                    /*if (!method.IsStatic_CACHE())
                    {
                        continue;
                    }*/

                    var attribute = method.GetAttribute_CACHE<ExecuteEventBaseAttribute>(true);

                    if (attribute == null)
                    {
                        continue;
                    }

                    using var scope4 =
                        _PRF_HookAttributeSubscriberManager_ProcessType_ProcessMethod_ProcessAttribute.Auto();

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
                    else if (attribute is ExecuteOnPreCullAttribute)
                    {
                        FrameStart.EventDelegates.onPreCull +=
                            StaticRoutine.CreateDelegate<Camera>(method).Invoke;
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
                        FrameStart.EventDelegates.onApplicationQuit +=
                            StaticRoutine.CreateDelegate(method).Invoke;
                    }
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(HookAttributeSubscriberManager) + ".";

        private static readonly ProfilerMarker _PRF_HookAttributeSubscriberManager =
            new(_PRF_PFX + nameof(HookAttributeSubscriberManager));

        private static readonly ProfilerMarker _PRF_HookAttributeSubscriberManager_ProcessType =
            new(_PRF_PFX + nameof(HookAttributeSubscriberManager) + ".ProcessType");

        private static readonly ProfilerMarker _PRF_HookAttributeSubscriberManager_ProcessType_ProcessMethod =
            new(_PRF_PFX + nameof(HookAttributeSubscriberManager) + ".ProcessType.ProcessMethod");

        private static readonly ProfilerMarker
            _PRF_HookAttributeSubscriberManager_ProcessType_ProcessMethod_ProcessAttribute = new(
                _PRF_PFX +
                nameof(HookAttributeSubscriberManager) +
                ".ProcessType.ProcessMethod.ProcessAttribute"
            );

        #endregion
    }
}
