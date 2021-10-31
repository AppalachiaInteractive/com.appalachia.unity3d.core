#region

using System;
using System.Diagnostics;
using Appalachia.CI.Integration.Assets;
using Unity.Profiling;
using UnityEngine;

#if UNITY_EDITOR

#endif

#endregion

namespace Appalachia.Core.Scriptables
{
    [Serializable]
    public abstract class SingletonAppalachiaObject<T> : AppalachiaObject<T>,
                                                         ICrossAssemblySerializable,
                                                         ISingletonAppalachiaObject
        where T : SingletonAppalachiaObject<T>
    {
        #region Profiling And Tracing Markers

        private const string _PRF_PFX = nameof(SingletonAppalachiaObject<T>) + ".";
        private static T _instance;
        private static readonly ProfilerMarker _PRF_instance = new(_PRF_PFX + nameof(instance));
        private static readonly ProfilerMarker _PRF_GetSerializable = new(_PRF_PFX + nameof(GetSerializable));

        private static readonly ProfilerMarker _PRF_SetInstance = new(_PRF_PFX + nameof(SetInstance));
        private static readonly ProfilerMarker _PRF_OnEnable = new(_PRF_PFX + nameof(OnEnable));

        #endregion

        public static T instance
        {
            get
            {
                using (_PRF_instance.Auto())
                {
#if UNITY_EDITOR
                    if (_instance == null)
                    {
                        InitializeSingletonUsage();
                    }
#endif
                    return _instance;
                }
            }
        }

        protected virtual void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                _instance = this as T;

                WhenEnabled();
            }
        }

        protected virtual void WhenEnabled()
        {
        }

        public ScriptableObject GetSerializable()
        {
            using (_PRF_GetSerializable.Auto())
            {
                return _instance;
            }
        }

        public void SetInstance(ISingletonAppalachiaObject i)
        {
            using (_PRF_SetInstance.Auto())
            {
                _instance = i as T;
            }
        }

#if UNITY_EDITOR

        private static readonly ProfilerMarker _PRF_InitializeSingletonUsage =
            new(_PRF_PFX + nameof(InitializeSingletonUsage));

        private static void InitializeSingletonUsage()
        {
            using (_PRF_InitializeSingletonUsage.Auto())
            {
                var stack = new StackTrace();

                if (stack.FrameCount > 255)
                {
                    throw new NotSupportedException("Stack too deep!");
                }

                _instance = FindExistingInstance();

                if (_instance != null)
                {
                    return;
                }

                _instance = CreateAndSaveSingleton();

                SingletonAppalachiaObjectLookup.ScanExternal();
            }
        }

        private static T CreateAndSaveSingleton()
        {
            using (_PRF_CreateAndSaveSingleton.Auto())
            {
                return CreateAndSaveSingleton($"{typeof(T).Name}_{DateTime.Now:yyyyMMdd-hhmmssfff}.asset");
            }
        }

        private static readonly ProfilerMarker _PRF_CreateAndSaveSingleton =
            new(_PRF_PFX + nameof(CreateAndSaveSingleton));

        private static T CreateAndSaveSingleton(string name)
        {
            using (_PRF_CreateAndSaveSingleton.Auto())
            {
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = FindExistingInstance();

                if (_instance != null)
                {
                    return _instance;
                }

                var inst = CreateInstance(typeof(T)) as T;

                var i = CreateNew(name, inst);

                AssetDatabaseManager.SaveAssets();
                AssetDatabaseManager.Refresh();

                return i;
            }
        }

        private static T FindExistingInstance()
        {
            var searchTerm = $"t:{typeof(T).Name}";
            var guids = AssetDatabaseManager.FindAssets(searchTerm);

            T firstFound = null;

            for (var index = 0; index < guids.Length; index++)
            {
                var guid = guids[index];
                var path = AssetDatabaseManager.GUIDToAssetPath(guid);
                var type = AssetDatabaseManager.GetMainAssetTypeAtPath(path);

                if (typeof(T).IsAssignableFrom(type))
                {
                    var i = AssetDatabaseManager.LoadAssetAtPath<ScriptableObject>(path);

                    if (i == null)
                    {
                        continue;
                    }

                    var cast = i as T;

                    // prefer assets to packages
                    if (path.StartsWith("Assets"))
                    {
                        return cast;
                    }

                    if (firstFound == null)
                    {
                        firstFound = cast;
                    }
                }
            }

            if (firstFound != null)
            {
                return firstFound;
            }

            return null;
        }

#endif
    }
}