#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Attributes;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Core.Scriptables
{
    [Critical]
    [Serializable]
    [InitializeOnLoadAlways]
    public sealed class SingletonAppalachiaObjectLookup : AppalachiaObject
    {
        static SingletonAppalachiaObjectLookup()
        {
        }

        #region Fields and Autoproperties

        public List<ScriptableObject> singletons = new();
        private Dictionary<Assembly, string> _assemblyNames;

        public List<UnityEditorInternal.AssemblyDefinitionAsset> excludedAssemblies = new();

        #endregion

#if UNITY_EDITOR

        #region Event Functions

        protected override void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                base.OnEnable();

                Scan();
            }
        }

        #endregion

        public static void ScanExternal()
        {
            using (_PRF_ScanExternal.Auto())
            {
                var path = AssetDatabaseManager.FindAssetPaths("t: SingletonScriptableObjectLookup")
                                               .FirstOrDefault();

                if (path == null)
                {
                    return;
                }

                var instance = AssetDatabaseManager.LoadAssetAtPath<SingletonAppalachiaObjectLookup>(path);

                if (instance == null)
                {
                    return;
                }

                instance.Scan();
            }
        }

        private static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            using (_PRF_IsAssignableToGenericType.Auto())
            {
                var interfaceTypes = givenType.GetInterfaces();

                for (var index = 0; index < interfaceTypes.Length; index++)
                {
                    var it = interfaceTypes[index];
                    if (it.IsGenericType && (it.GetGenericTypeDefinition() == genericType))
                    {
                        return true;
                    }
                }

                if (givenType.IsGenericType && (givenType.GetGenericTypeDefinition() == genericType))
                {
                    return true;
                }

                var baseType = givenType.BaseType;
                if (baseType == null)
                {
                    return false;
                }

                var result = IsAssignableToGenericType(baseType, genericType);

                return result;
            }
        }

        [Button]
        public void Scan()
        {
            using (_PRF_Scan.Auto())
            {
                if (singletons == null)
                {
                    singletons = new List<ScriptableObject>();
                }

                singletons.Clear();

                var genericType = typeof(SingletonAppalachiaObject<>);

                HashSet<string> excludedAssemblyLookup;
                string[] guids;

                using (_PRF_Scan_AssemblyLookup.Auto())
                {
                    excludedAssemblyLookup = excludedAssemblies.Select(ea => ea.name).ToHashSet();
                }

                using (_PRF_Scan_Search.Auto())
                {
                    guids = AssetDatabaseManager.FindAssets("t: AppalachiaObject");
                }

                for (var index = 0; index < guids.Length; index++)
                {
                    var guid = guids[index];
                    var path = AssetDatabaseManager.GUIDToAssetPath(guid);

                    Type type;

                    using (_PRF_Scan_CheckExcluded.Auto())
                    {
                        if (string.IsNullOrWhiteSpace(path))
                        {
                            continue;
                        }

                        type = AssetDatabaseManager.GetMainAssetTypeAtPath(path);

                        if (type == null)
                        {
                            continue;
                        }

                        var assembly = type.Assembly;

                        if (_assemblyNames == null)
                        {
                            _assemblyNames = new Dictionary<Assembly, string>();
                        }

                        using (_PRF_Scan_AssemblyCheck.Auto())
                        {
                            string assemblyName;

                            if (_assemblyNames.ContainsKey(assembly))
                            {
                                assemblyName = _assemblyNames[assembly];
                            }
                            else
                            {
                                assemblyName = assembly.GetName().Name;
                                _assemblyNames.Add(assembly, assemblyName);
                            }

                            if (excludedAssemblyLookup.Contains(assemblyName))
                            {
                                continue;
                            }
                        }
                    }

                    if (IsAssignableToGenericType(type, genericType))
                    {
                        ScriptableObject instance;

                        using (_PRF_Scan_LoadInstance.Auto())
                        {
                            instance = AssetDatabaseManager.LoadAssetAtPath<ScriptableObject>(path);
                        }

                        if (instance == null)
                        {
                            continue;
                        }

                        using (_PRF_Scan_AddToList.Auto())
                        {
                            singletons.Add(instance);
                        }

                        /*var cast = instance as ISingletonScriptableObject;
                        
                        using (_PRF_Scan_SetInstance.Auto())
                        {
                            cast.SetInstance(cast);
                        }*/
                    }
                }

                this.MarkAsModified();
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(SingletonAppalachiaObjectLookup) + ".";
        private static readonly ProfilerMarker _PRF_Awake = new(_PRF_PFX + "Awake");

        private static readonly ProfilerMarker _PRF_Start = new(_PRF_PFX + "Start");
        private static readonly ProfilerMarker _PRF_OnEnable = new(_PRF_PFX + "OnEnable");
        private static readonly ProfilerMarker _PRF_Update = new(_PRF_PFX + "Update");
        private static readonly ProfilerMarker _PRF_LateUpdate = new(_PRF_PFX + "LateUpdate");
        private static readonly ProfilerMarker _PRF_OnDisable = new(_PRF_PFX + "OnDisable");
        private static readonly ProfilerMarker _PRF_OnDestroy = new(_PRF_PFX + "OnDestroy");
        private static readonly ProfilerMarker _PRF_Scan = new(_PRF_PFX + nameof(Scan));
        private static readonly ProfilerMarker _PRF_Scan_Search = new(_PRF_PFX + nameof(Scan) + ".Search");

        private static readonly ProfilerMarker _PRF_Scan_AssemblyLookup =
            new(_PRF_PFX + nameof(Scan) + ".AssemblyLookup");

        private static readonly ProfilerMarker _PRF_Scan_CheckExcluded =
            new(_PRF_PFX + nameof(Scan) + ".CheckExcluded");

        private static readonly ProfilerMarker _PRF_Scan_LoadInstance =
            new(_PRF_PFX + nameof(Scan) + ".LoadInstance");

        private static readonly ProfilerMarker _PRF_Scan_AddToList =
            new(_PRF_PFX + nameof(Scan) + ".AddToList");

        private static readonly ProfilerMarker _PRF_Scan_SetInstance =
            new(_PRF_PFX + nameof(Scan) + ".SetInstance");

        private static readonly ProfilerMarker _PRF_Scan_SetDirty =
            new(_PRF_PFX + nameof(Scan) + ".SetDirty");

        private static readonly ProfilerMarker _PRF_Scan_AssemblyCheck =
            new(_PRF_PFX + nameof(Scan) + ".AssemblyCheck");

        private static readonly ProfilerMarker _PRF_IsAssignableToGenericType =
            new(_PRF_PFX + nameof(IsAssignableToGenericType));

        private static readonly ProfilerMarker _PRF_ScanExternal = new(_PRF_PFX + nameof(ScanExternal));

        #endregion
#endif
    }
}
