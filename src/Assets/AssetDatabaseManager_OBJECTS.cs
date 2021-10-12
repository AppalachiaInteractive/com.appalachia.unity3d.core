using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Appalachia.Core.Constants;
using Appalachia.Utility.Reflection.Extensions;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;

namespace Appalachia.Core.Assets
{
    public static partial class AssetDatabaseManager
    {
        
        private static Dictionary<string, string[]> _guidsByTypeName;
        private static Dictionary<string, string[]> _pathsByTypeName;
        private static Dictionary<string, Type[]> _typesByTypeName;

        private const string _PRF_PFX = nameof(AssetDatabaseManager) + ".";
        
        private static readonly ProfilerMarker _PRF_GetAllMonoScripts = new ProfilerMarker(_PRF_PFX + nameof(GetAllMonoScripts));

        private static readonly ProfilerMarker _PRF_InitializeTypeData = new ProfilerMarker(_PRF_PFX + nameof(InitializeTypeData));
        
        private static void InitializeTypeData(string typeName)
        {
            using (_PRF_InitializeTypeData.Auto())
            {
                _typesByTypeName ??= new Dictionary<string, Type[]>();
                _pathsByTypeName ??= new Dictionary<string, string[]>();
                _guidsByTypeName ??= new Dictionary<string, string[]>();

                if (!_typesByTypeName.ContainsKey(typeName))
                {
                    var guids = AssetDatabase.FindAssets($"t: {typeName}");

                    var sglength = guids.Length;
                
                    var paths = new string[sglength];
                    var types = new Type[sglength];

                    for (var i = 0; i < sglength; i++)
                    {
                        var guid = guids[i];
                        var path = AssetDatabase.GUIDToAssetPath(guid);
                        var type = AssetDatabase.GetMainAssetTypeAtPath(path);
                    
                        paths[i] = path;
                        types[i] = type;
                    }
                    
                    _typesByTypeName.Add(typeName, types);
                    _pathsByTypeName.Add(typeName, paths);
                    _guidsByTypeName.Add(typeName, guids);
                }
            }
        }
        
  
        private static readonly ProfilerMarker _PRF_GetAllAssetGuids = new ProfilerMarker(_PRF_PFX + nameof(GetAllAssetGuids));
        public static string[] GetAllAssetGuids(Type type = null)
        {
            using (_PRF_GetAllAssetGuids.Auto())
            {
                var typeName = type?.Name ?? nameof(UnityEngine.Object);
                InitializeTypeData(typeName);

                return _guidsByTypeName[typeName];
            }
        }

        private static readonly ProfilerMarker _PRF_GetAllAssetPaths = new ProfilerMarker(_PRF_PFX + nameof(GetAllAssetPaths));
        public static string[] GetAllAssetPaths(Type type = null)
        {
            using (_PRF_GetAllAssetPaths.Auto())
            {
                var typeName = type?.Name ?? nameof(UnityEngine.Object);
                InitializeTypeData(typeName);

                return _pathsByTypeName[typeName];
            }
        }

        private static readonly ProfilerMarker _PRF_GetProjectAssetPaths = new ProfilerMarker(_PRF_PFX + nameof(GetProjectAssetPaths));
        public static string[] GetProjectAssetPaths(Type type = null)
        {
            using (_PRF_GetProjectAssetPaths.Auto())
            {
                var assetPaths = GetAllAssetPaths(type);

                var filteredAssetPaths = assetPaths
                                        .Where(
                                             p => p.StartsWith("Assets/") &&
                                                  p.Contains("Appalachia")
                                         )
                                        .ToArray();

                Array.Sort(filteredAssetPaths);

                return filteredAssetPaths;
            }
        }
        
        

        private static readonly ProfilerMarker _PRF_GetAllAssetTypes = new ProfilerMarker(_PRF_PFX + nameof(GetAllAssetTypes));
        public static Type[] GetAllAssetTypes(Type type = null)
        {
            using (_PRF_GetAllAssetTypes.Auto())
            {
                var typeName = type?.Name ?? nameof(UnityEngine.Object);
                InitializeTypeData(typeName);

                return _typesByTypeName[typeName];
            }
        }


        private static string[] _assemblyLogs;
        private static StringBuilder _assemblyLogBuilder;
        private static string _assemblyLog;
        
        [MenuItem(APPA_MENU.BASE_AppalachiaTools + APPA_MENU.ASM_AppalachiaEditingCore + nameof(LogAssemblies))]
        public static void LogAssemblies()
        {
            var assemblies = ReflectionExtensions.GetAssemblies();

            if (_assemblyLogs == null)
            {
                _assemblyLogs = new string[assemblies.Length];
                
                for (var index = 0; index < assemblies.Length; index++)
                {
                    var assembly = assemblies[index];
                    var partialAssemblyName = assembly.FullName.Split(',')[0];

                    _assemblyLogs[index] = partialAssemblyName;
                }

                Array.Sort(_assemblyLogs);

                if (_assemblyLogBuilder == null)
                {
                    _assemblyLogBuilder = new StringBuilder();
                }
                  
                for (var index = 0; index < _assemblyLogs.Length; index++)
                {
                    _assemblyLogBuilder.AppendLine(_assemblyLogs[index]);
                }

                _assemblyLog = _assemblyLogBuilder.ToString();
            }

          

            Debug.Log(_assemblyLog);
        }
    }
}
