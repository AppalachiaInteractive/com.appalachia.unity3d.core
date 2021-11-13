#region

using System;
using System.Collections.Generic;
using Appalachia.CI.Integration.Assets;
using Appalachia.CI.Integration.Attributes;
using Appalachia.CI.Integration.Core;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Core.Assets;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

namespace Appalachia.Core.Scriptables
{
    [DoNotReorderFields]
    public abstract class AppalachiaObject<T> : ScriptableObject /*, IResponsive*/, IAppalachiaObject<T>
        where T : ScriptableObject
    {
        #region Profiling And Tracing Markers

        private const string _PRF_PFX = nameof(AppalachiaObject<T>) + ".";

        #endregion
        
        protected virtual bool ShowMetadata => true;
        protected virtual bool ShowWorkflow => false;

#if UNITY_EDITOR

        #region Profiling And Tracing Markers

        private static readonly ProfilerMarker _PRF_SetDirty = new(_PRF_PFX + nameof(SetDirty));
        private static readonly ProfilerMarker _PRF_SetDirtyAndSave = new(_PRF_PFX + nameof(SetDirtyAndSave));

        private static readonly ProfilerMarker _PRF_Ping = new(_PRF_PFX + nameof(Ping));
        private static readonly ProfilerMarker _PRF_Select = new(_PRF_PFX + nameof(Select));
        private static readonly ProfilerMarker _PRF_Duplicate = new(_PRF_PFX + nameof(Duplicate));
        private static readonly ProfilerMarker _PRF_AssetPath = new(_PRF_PFX + nameof(AssetPath));
        private static readonly ProfilerMarker _PRF_DirectoryPath = new(_PRF_PFX + nameof(DirectoryPath));
        private static readonly ProfilerMarker _PRF_HasAssetPath = new(_PRF_PFX + nameof(HasAssetPath));
        private static readonly ProfilerMarker _PRF_HasSubAssets = new(_PRF_PFX + nameof(HasSubAssets));

        private static readonly ProfilerMarker _PRF_UpdateNameAndMove =
            new(_PRF_PFX + nameof(UpdateNameAndMove));

        private static readonly ProfilerMarker _PRF_GetAllOfType = new(_PRF_PFX + nameof(GetAllOfType));
        private static readonly ProfilerMarker _PRF_CreateNew = new(_PRF_PFX + nameof(CreateNew));
        private static readonly ProfilerMarker _PRF_LoadOrCreateNew = new(_PRF_PFX + nameof(LoadOrCreateNew));
        private static readonly ProfilerMarker _PRF_Rename = new(_PRF_PFX + nameof(Rename));

        private static readonly ProfilerMarker _PRF_LoadOrCreateNewIfNull =
            new ProfilerMarker(_PRF_PFX + nameof(LoadOrCreateNewIfNull));

        #endregion

        [SerializeField]
        [HideInInspector]
        private string _cachedName;

        [SerializeField]
        [HideInInspector]
        private string _niceName;
        
        public string NiceName
        {
            get
            {
                if ((_niceName == null) || (name != _cachedName))
                {
                    _cachedName = name;
                    _niceName = UnityEditor.ObjectNames.NicifyVariableName(name);
                    SetDirty();
                }

                return _niceName;
            }
            set => _niceName = value;
        }

        public void Ping()
        {
            using (_PRF_Ping.Auto())
            {
                UnityEditor.EditorGUIUtility.PingObject(this);
            }
        }

#endif
        
#if UNITY_EDITOR
        [FoldoutGroup("Metadata", false)]
        [ShowIf(nameof(ShowMetadata))]
        [PropertyOrder(-1000)]
        [Button]
#endif
        public new void SetDirty()
        {
#if UNITY_EDITOR
            using (_PRF_SetDirty.Auto())
            {
                UnityEditor.EditorUtility.SetDirty(this);
            }
#endif
        }

#if UNITY_EDITOR

        [FoldoutGroup("Metadata", false)]
        [PropertyOrder(-1000)]
        [Button]
        public void SetDirtyAndSave()
        {
            using (_PRF_SetDirtyAndSave.Auto())
            {
                SetDirty();
                
                if (!Application.isPlaying)
                {
                    AssetDatabaseSaveManager.SaveAssetsNextFrame();
                }
                
            }
        }
        
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

        [HorizontalGroup("$ShowWorkflow/Workflow/Productivity")]
        [Button]
        [PropertyOrder(-40000)]
        [ShowIf(nameof(ShowWorkflow))]
        public void Duplicate()
        {
            using (_PRF_Duplicate.Auto())
            {
                var path = AssetDatabaseManager.GenerateUniqueAssetPath(
                    AssetDatabaseManager.GetAssetPath(this)
                );
                var newInstance = Instantiate(this);
                AssetDatabaseManager.CreateAsset(newInstance, path);
                UnityEditor.Selection.activeObject = newInstance;
            }
        }

        public string AssetPath
        {
            get
            {
                using (_PRF_AssetPath.Auto())
                {
                    return AssetDatabaseManager.GetAssetPath(this);
                }
            }
        }

        public string DirectoryPath
        {
            get
            {
                using (_PRF_DirectoryPath.Auto())
                {
                    return AppaPath.GetDirectoryName(AssetPath);
                }
            }
        }

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

        public bool HasSubAssets(out Object[] subAssets)
        {
            using (_PRF_HasSubAssets.Auto())
            {
                subAssets = null;

                if (HasAssetPath(out var path))
                {
                    subAssets = AssetDatabaseManager.LoadAllAssetsAtPath(path);

                    if ((subAssets == null) || (subAssets.Length == 0))
                    {
                        return false;
                    }

                    return true;
                }

                return false;
            }
        }

        public bool UpdateNameAndMove(string newName)
        {
            using (_PRF_UpdateNameAndMove.Auto())
            {
                var assetPath = AssetDatabaseManager.GetAssetPath(this).Replace("\\", "/");
                var basePath = AppaPath.GetDirectoryName(assetPath);

                var newPath = AppaPath.Combine(basePath, newName);

                var newPath_name = AppaPath.GetFileNameWithoutExtension(newPath);
                var newPath_extension = AppaPath.GetExtension(newPath);

                newPath_name = newPath_name.TrimEnd('.', '-', '_', ',');

                if (string.IsNullOrWhiteSpace(newPath_extension))
                {
                    newPath_extension = ".asset";
                }

                var finalPath = AppaPath.Combine(basePath, $"{newPath_name}{newPath_extension}")
                                        .Replace("\\", "/");

                name = newPath_name;

                var successful = true;

                if (finalPath != assetPath)
                {
                    var landedAt = AssetDatabaseManager.MoveAsset(assetPath, finalPath);

                    if (landedAt != finalPath)
                    {
                        successful = false;
                    }

                    AssetDatabaseManager.Refresh();
                }

                return successful;
            }
        }

        public virtual void OnCreate()
        {
        }

        public static T[] GetAllOfType()
        {
            using (_PRF_GetAllOfType.Auto())
            {
                var all = AssetDatabaseManager.FindAssets($"t: {typeof(T).Name}");

                var results = new T[all.Length];

                for (var i = 0; i < all.Length; i++)
                {
                    var path = AssetDatabaseManager.GUIDToAssetPath(all[i]);

                    results[i] = AssetDatabaseManager.LoadAssetAtPath<T>(path);
                }

                return results;
            }
        }

        public static List<T> GetAllOfType(Predicate<T> where)
        {
            using (_PRF_GetAllOfType.Auto())
            {
                var all = AssetDatabaseManager.FindAssets($"t: {typeof(T).Name}");

                var results = new List<T>(all.Length);

                for (var i = 0; i < all.Length; i++)
                {
                    var path = AssetDatabaseManager.GUIDToAssetPath(all[i]);

                    var loaded = AssetDatabaseManager.LoadAssetAtPath<T>(path);

                    if (where(loaded))
                    {
                        results.Add(loaded);
                    }
                }

                return results;
            }
        }

        public static T CreateNew()
        {
            using (_PRF_CreateNew.Auto())
            {
                return AppalachiaObjectFactory.CreateNew<T>();
            }
        }

        public static T LoadOrCreateNew(string name, string dataFolder = null)
        {
            using (_PRF_LoadOrCreateNew.Auto())
            {
                return AppalachiaObjectFactory.LoadOrCreateNew<T>(name, dataFolder);
            }
        }

        public static void LoadOrCreateNewIfNull(ref T assignment, string name, string dataFolder = null)
        {
            using (_PRF_LoadOrCreateNewIfNull.Auto())
            {
                if (assignment == null)
                {
                    assignment = AppalachiaObjectFactory.LoadOrCreateNew<T>(name, dataFolder);
                }
            }
        }

        public static T LoadOrCreateNew(
            string name,
            bool prependType,
            bool appendType,
            string dataFolder = null)
        {
            using (_PRF_LoadOrCreateNew.Auto())
            {
                return AppalachiaObjectFactory.LoadOrCreateNew<T>(name, prependType, appendType, dataFolder);
            }
        }

        public static T CreateNew(string name, T i = null, string dataFolder = null)
        {
            using (_PRF_CreateNew.Auto())
            {
                if (i == null)
                {
                    i = CreateInstance<T>();
                }

                return AppalachiaObjectFactory.CreateNew(name, i, dataFolder);
            }
        }

        public void Rename(string newName)
        {
            using (_PRF_Rename.Auto())
            {
                AppalachiaObjectFactory.Rename(this as T, newName);
            }
        }

#endif
    }
}