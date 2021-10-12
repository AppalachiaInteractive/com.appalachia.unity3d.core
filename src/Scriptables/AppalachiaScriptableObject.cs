using System;
using System.Collections.Generic;
using System.IO;
using Appalachia.Core.Assets;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;

#region

using Object = UnityEngine.Object;

#endregion

#if UNITY_EDITOR

#endif

namespace Appalachia.Core.Scriptables
{
    public abstract class AppalachiaScriptableObject<T> : ScriptableObject /*, IResponsive*/
        where T : AppalachiaScriptableObject<T>
    {
        private const string _PRF_PFX = nameof(AppalachiaScriptableObject<T>) + ".";

        private static readonly ProfilerMarker _PRF_SetDirty = new(_PRF_PFX + nameof(SetDirty));

        private static readonly ProfilerMarker _PRF_SetDirtyAndSave =
            new(_PRF_PFX + nameof(SetDirtyAndSave));

        //private static AspectSets _aspects;

        [SerializeField]
        [HideInInspector]
        private string _niceName;

        public new void SetDirty()
        {
            using (_PRF_SetDirty.Auto())
            {
#if UNITY_EDITOR
                EditorUtility.SetDirty(this);
#endif
            }
        }

        public void SetDirtyAndSave()
        {
            using (_PRF_SetDirtyAndSave.Auto())
            {
                SetDirty();
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    AssetDatabaseManager.SaveAssetsNextFrame();
                }
#endif
            }
        }

#if UNITY_EDITOR

        [SerializeField]
        [HideInInspector]
        private string _cachedName;

        public string NiceName
        {
            get
            {
                if ((_niceName == null) || (name != _cachedName))
                {
                    _cachedName = name;
                    _niceName = ObjectNames.NicifyVariableName(name);
                    SetDirty();
                }

                return _niceName;
            }
            set => _niceName = value;
        }

        private static readonly ProfilerMarker _PRF_Ping = new(_PRF_PFX + nameof(Ping));

        public void Ping()
        {
            using (_PRF_Ping.Auto())
            {
                EditorGUIUtility.PingObject(this);
            }
        }

        private static readonly ProfilerMarker _PRF_Select = new(_PRF_PFX + nameof(Select));

        [ShowIfGroup("$ShowWorkflow")]
        [FoldoutGroup("$ShowWorkflow/Workflow", Order = -50000)]
        [HorizontalGroup("$ShowWorkflow/Workflow/Productivity")]
        [Button]
        [PropertyOrder(-40000)]
        [ShowIf(nameof(ShowWorkflow))]
        public void Select()
        {
            using (_PRF_Select.Auto())
            {                
                AssetDatabaseManager.SetSelection(this);
            }
        }

        private static readonly ProfilerMarker _PRF_Duplicate = new(_PRF_PFX + nameof(Duplicate));

        [HorizontalGroup("$ShowWorkflow/Workflow/Productivity")]
        [Button]
        [PropertyOrder(-40000)]
        [ShowIf(nameof(ShowWorkflow))]
        public void Duplicate()
        {
            using (_PRF_Duplicate.Auto())
            {
                var path = AssetDatabase.GenerateUniqueAssetPath(AssetDatabase.GetAssetPath(this));
                var newInstance = Instantiate(this);
                AssetDatabase.CreateAsset(newInstance, path);
                Selection.activeObject = newInstance;
            }
        }

        private static readonly ProfilerMarker _PRF_AssetPath = new(_PRF_PFX + nameof(AssetPath));

        public string AssetPath
        {
            get
            {
                using (_PRF_AssetPath.Auto())
                {
                    return AssetDatabase.GetAssetPath(this);
                }
            }
        }

        private static readonly ProfilerMarker _PRF_DirectoryPath =
            new(_PRF_PFX + nameof(DirectoryPath));

        public string DirectoryPath
        {
            get
            {
                using (_PRF_DirectoryPath.Auto())
                {
                    return Path.GetDirectoryName(AssetPath);
                }
            }
        }

        private static readonly ProfilerMarker _PRF_HasAssetPath =
            new(_PRF_PFX + nameof(HasAssetPath));

        public bool HasAssetPath(out string path)
        {
            using (_PRF_HasAssetPath.Auto())
            {
                path = AssetPath;

                if (string.IsNullOrWhiteSpace(path))
                {
                    return false;
                }

                return true;
            }
        }

        private static readonly ProfilerMarker _PRF_HasSubAssets =
            new(_PRF_PFX + nameof(HasSubAssets));

        public bool HasSubAssets(out Object[] subAssets)
        {
            using (_PRF_HasSubAssets.Auto())
            {
                subAssets = null;

                if (HasAssetPath(out var path))
                {
                    subAssets = AssetDatabase.LoadAllAssetsAtPath(path);

                    if ((subAssets == null) || (subAssets.Length == 0))
                    {
                        return false;
                    }

                    return true;
                }

                return false;
            }
        }

        private static readonly ProfilerMarker _PRF_UpdateNameAndMove =
            new(_PRF_PFX + nameof(UpdateNameAndMove));

        public bool UpdateNameAndMove(string newName)
        {
            using (_PRF_UpdateNameAndMove.Auto())
            {
                var assetPath = AssetDatabase.GetAssetPath(this).Replace("\\", "/");
                var basePath = Path.GetDirectoryName(assetPath);

                var newPath = Path.Combine(basePath, newName);

                var newPath_name = Path.GetFileNameWithoutExtension(newPath);
                var newPath_extension = Path.GetExtension(newPath);

                newPath_name = newPath_name.TrimEnd('.', '-', '_', ',');

                if (string.IsNullOrWhiteSpace(newPath_extension))
                {
                    newPath_extension = ".asset";
                }

                var finalPath = Path.Combine(basePath, $"{newPath_name}{newPath_extension}")
                                    .Replace("\\", "/");

                name = newPath_name;

                var successful = true;

                if (finalPath != assetPath)
                {
                    var landedAt = AssetDatabase.MoveAsset(assetPath, finalPath);

                    if (landedAt != finalPath)
                    {
                        successful = false;
                    }

                    AssetDatabase.Refresh();
                }

                return successful;
            }
        }

        public virtual bool ShowWorkflow => false;

        internal virtual void OnCreate()
        {
        }

        private static readonly ProfilerMarker _PRF_GetAllOfType =
            new(_PRF_PFX + nameof(GetAllOfType));

        public static T[] GetAllOfType()
        {
            using (_PRF_GetAllOfType.Auto())
            {
                var all = AssetDatabase.FindAssets($"t: {typeof(T).Name}");

                var results = new T[all.Length];

                for (var i = 0; i < all.Length; i++)
                {
                    var path = AssetDatabase.GUIDToAssetPath(all[i]);

                    results[i] = AssetDatabase.LoadAssetAtPath<T>(path);
                }

                return results;
            }
        }

        public static List<T> GetAllOfType(Predicate<T> where)
        {
            using (_PRF_GetAllOfType.Auto())
            {
                var all = AssetDatabase.FindAssets($"t: {typeof(T).Name}");

                var results = new List<T>(all.Length);

                for (var i = 0; i < all.Length; i++)
                {
                    var path = AssetDatabase.GUIDToAssetPath(all[i]);

                    var loaded = AssetDatabase.LoadAssetAtPath<T>(path);

                    if (where(loaded))
                    {
                        results.Add(loaded);
                    }
                }

                return results;
            }
        }

#endif
    }
}
