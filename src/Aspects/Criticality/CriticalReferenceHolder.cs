#region

using System;
using System.Collections.Generic;
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Attributes;
using Appalachia.Core.Behaviours;
using Appalachia.Utility.Reflection.Extensions;
using Unity.Profiling;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#endif

#endregion

#if UNITY_EDITOR

#endif

namespace Appalachia.Core.Aspects.Criticality
{
    public class CriticalReferenceHolder : AppalachiaMonoBehaviour
    {
        private const string _PRF_PFX = nameof(CriticalReferenceHolder) + ".";
        public List<ScriptableObject> references = new();

#if UNITY_EDITOR

        private static readonly ProfilerMarker _PRF_OnEnable = new(_PRF_PFX + nameof(OnEnable));

        private void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                Scan();
            }
        }

        private static readonly ProfilerMarker _PRF_Scan = new(_PRF_PFX + nameof(Scan));

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

                var foundTypes = new HashSet<Type>();

                var allTypes = ReflectionExtensions.GetAllTypes();

                for (var i = 0; i < allTypes.Length; i++)
                {
                    var type = allTypes[i];

                    if (type.GetAttributes_CACHE<CriticalAttribute>(true).Length > 0)
                    {
                        foundTypes.Add(type);
                    }
                }

                var soPaths = AssetDatabaseManager.GetAllAssetGuids(typeof(ScriptableObject));

                for (var i = 0; i < soPaths.Length; i++)
                {
                    var path = soPaths[i];

                    var instance = AssetDatabaseManager.LoadAssetAtPath<ScriptableObject>(path);

                    if (instance == null)
                    {
                        continue;
                    }

                    var type = instance.GetType();

                    if (foundTypes.Contains(type))
                    {
                        references.Add(instance);
                    }
                }
            }
        }
#endif
    }
}
