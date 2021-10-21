using System;
using System.Text;
using Appalachia.CI.Constants;
using Appalachia.Utility.Reflection.Extensions;
using UnityEditor;
using UnityEngine;

namespace Appalachia.Core.Assets
{
    public class AssetAssemblyUtilities
    {
        

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
