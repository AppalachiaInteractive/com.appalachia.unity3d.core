#region

using System;
using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Core.Behaviours;
using Unity.Profiling;
using UnityEngine;
#if ODIN_INSPECTOR

using Sirenix.OdinInspector;
#endif
#endregion

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Appalachia.Core.Aspects.Criticality
{
    public class CriticalReferenceHolder: InternalMonoBehaviour
    {
        private const string _PRF_PFX = nameof(CriticalReferenceHolder) + ".";
        public List<ScriptableObject> references = new List<ScriptableObject>();

#if UNITY_EDITOR

        private static readonly ProfilerMarker _PRF_OnEnable = new ProfilerMarker(_PRF_PFX + nameof(OnEnable));
        
        private void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                Scan();
            }
        }

        private static readonly ProfilerMarker _PRF_Scan = new ProfilerMarker(_PRF_PFX + nameof(Scan));
        
#if ODIN_INSPECTOR
        [Button]
#endif
        private void Scan()
        {
            using (_PRF_Scan.Auto())
            {
                if (references == null)
                {
                    references = new List<ScriptableObject>();
                }

                references.Clear();

                var types = new HashSet<Type>();

                for (var index = 0; index < AppDomain.CurrentDomain.GetAssemblies().Length; index++)
                {
                    var assembly = AppDomain.CurrentDomain.GetAssemblies()[index];
                    var types1 = assembly.GetTypes();
                    for (var i = 0; i < types1.Length; i++)
                    {
                        var type = types1[i];
                        if (type.GetCustomAttributes(typeof(CriticalAttribute), true).Length > 0)
                        {
                            types.Add(type);
                        }
                    }
                }

                var guids = AssetDatabase.FindAssets("t: ScriptableObject");

                for(var i = 0; i < guids.Length; i++)
                {
                    var guid = guids[i];
                    var path = AssetDatabase.GUIDToAssetPath(guid);

                    var instance = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

                    if (instance == null)
                    {
                        continue;
                    }

                    var type = instance.GetType();

                    if (types.Contains(type))
                    {
                        references.Add(instance);
                    }
                }
            }
        }
#endif
    }
}
