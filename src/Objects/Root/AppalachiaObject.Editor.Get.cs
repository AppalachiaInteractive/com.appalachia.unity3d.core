#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Appalachia.CI.Integration.Assets;
using Appalachia.CI.Integration.Core;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Root
{
    public abstract partial class AppalachiaObject
    {
        public static ScriptableObject[] GetAllOfType(Type t)
        {
            using (_PRF_GetAllOfType.Auto())
            {
                var searchTerm = SearchStringBuilder.Build.AddType(t).Finish();
                var all = AssetDatabaseManager.FindAssets(searchTerm);

                var results = new ScriptableObject[all.Length];

                for (var i = 0; i < all.Length; i++)
                {
                    var path = AssetDatabaseManager.GUIDToAssetPath(all[i]);

                    results[i] = AssetDatabaseManager.LoadAssetAtPath(path, t) as ScriptableObject;
                }

                return results;
            }
        }

        public static List<ScriptableObject> GetAllOfType(Type t, Predicate<ScriptableObject> where)
        {
            using (_PRF_GetAllOfType.Auto())
            {
                return GetAllOfType(t).Where(v => where(v)).ToList();
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetAllOfType =
            new ProfilerMarker(_PRF_PFX + nameof(GetAllOfType));

        #endregion
    }

    public abstract partial class AppalachiaObject<T>
    {
        #region Constants and Static Readonly

        private static readonly string _PRF_PFX2 = typeof(T).Name + ".";

        #endregion

        public static T[] GetAllOfType()
        {
            using (_PRF_GetAllOfType.Auto())
            {
                return GetAllOfType(typeof(T)).Cast<T>().ToArray();
            }
        }

        public static List<T> GetAllOfType(Predicate<T> where)
        {
            using (_PRF_GetAllOfType.Auto())
            {
                var predicate = new Predicate<ScriptableObject>(obj => where(obj as T));

                return GetAllOfType(typeof(T), predicate).Cast<T>().ToList();
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetAllOfType =
            new ProfilerMarker(_PRF_PFX2 + nameof(GetAllOfType));

        #endregion
    }
}

#endif
