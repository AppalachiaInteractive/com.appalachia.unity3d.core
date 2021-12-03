#region

using System;
using System.Collections.Generic;
using System.Linq;
using Appalachia.CI.Constants;
using Appalachia.CI.Integration.Assets;
using Appalachia.CI.Integration.Attributes;
using Appalachia.CI.Integration.Core;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Core.Assets;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

namespace Appalachia.Core.Scriptables
{
    [DoNotReorderFields]
    [InspectorIcon(Icons.Squirrel.Tan)]
    [SmartLabelChildren]
    public abstract class AppalachiaObject : ScriptableObject, IAppalachiaObject, IInitializable
    {
        #region Constants and Static Readonly

#if UNITY_EDITOR
        protected const string GROUP = BASE + "/" + APPASTR.Internal;
        protected const string GROUP_BUTTONS = GROUP + "/" + APPASTR.Buttons;
        protected const string GROUP_WORKFLOW = SHOW_WORKFLOW + "/" + APPASTR.Workflow;
        protected const string GROUP_WORKFLOW_PROD = GROUP_WORKFLOW + "/" + APPASTR.Productivity;
        protected const string SHOW_WORKFLOW = GROUP + "/$ShowWorkflow";
        protected const string BASE = "BASE";
        
        public const string TITLE = APPASTR.SCRIPTABLE;
        public const string TITLECOLOR = Utility.Colors.Colors.Appalachia.HEX.Tan;
        public const string TITLEICON = "";
        
        protected const string SUBTITLE = APPASTR.APPALACHIA_INTERACTIVE;
        protected const string SUBTITLECOLOR = Utility.Colors.Colors.Appalachia.HEX.Tan;
        protected const string SUBTITLEICON = "";
        
        protected const bool TITLE_BOLD = false;
        
        protected const string TITLEFONT = APPASTR.Fonts.Montserrat.Medium;
        protected const string SUBTITLEFONT = APPASTR.Fonts.Montserrat.Medium;
        
        protected const int TITLESIZE = 13;
        protected const int SUBTITLESIZE = 13;
        protected const int TITLEHEIGHT = 24;
        
        protected const string LABELCOLOR = Utility.Colors.Colors.Appalachia.HEX.Tan;
        protected const string GROUPBACKGROUNDCOLOR = Utility.Colors.Colors.Appalachia.HEX.Tan;
        protected const string CHILDCOLOR = Utility.Colors.Colors.Appalachia.HEX.LightYellow;
        protected const string BANNERBACKGROUNDCOLOR = Utility.Colors.Colors.Appalachia.HEX.Black;
#endif
        
        #endregion
        
        #region Fields and Autoproperties

#if UNITY_EDITOR
        [SerializeField, HideLabel, InlineProperty, LabelWidth(0)]
        [SmartTitleGroup(
            BASE,
            "$"+nameof(GetTitle),
            "$"+nameof(GetSubtitle),
            true,
            TITLE_BOLD,
            false,
            titleColor: "$"+nameof(GetTitleColor),
            titleFont: TITLEFONT,
            subtitleColor: "$"+nameof(GetSubtitleColor),
            subtitleFont: SUBTITLEFONT,
            titleSize: TITLESIZE,
            subtitleSize: SUBTITLESIZE,
            titleIcon: "$"+nameof(GetTitleIcon),
            subtitleIcon: "$"+nameof(GetSubtitleIcon),
            backgroundColor: "$"+nameof(GetBackgroundColor),
            titleHeight: TITLEHEIGHT
        )]
        [SmartFoldoutGroup(GROUP, false, GROUPBACKGROUNDCOLOR, true, LABELCOLOR, true, CHILDCOLOR)]
#endif
        private Initializer _initializationData;
        
#if UNITY_EDITOR
            [SerializeField]
            [HideInInspector]
            private string _cachedName;

            [SerializeField]
            [HideInInspector]
            private string _niceName;
#endif
        
        #endregion

        protected virtual bool ShowMetadata => true;
        protected virtual bool ShowWorkflow => false;

        protected Initializer initializer => _initializationData;


#if UNITY_EDITOR
        [ButtonGroup(GROUP_BUTTONS), HideInInlineEditors]
        private void ReInitialize()
        {
            initializer.Reset(this, DateTime.Now.ToString("O"));
            
            Initialize();
        }
#endif
        
#if UNITY_EDITOR

        protected virtual string GetTitle()
        {
            return TITLE;
        }

        protected virtual string GetSubtitle()
        {
            return SUBTITLE;
        }

        protected virtual string GetTitleColor()
        {
            return TITLECOLOR;
        }

        protected virtual string GetSubtitleColor()
        {
            return SUBTITLECOLOR;
        }

        protected virtual string GetTitleIcon()
        {
            return TITLEICON;
        }

        protected virtual string GetSubtitleIcon()
        {
            return SUBTITLEICON;
        }

        protected virtual string GetBackgroundColor()
        {
            return BANNERBACKGROUNDCOLOR;
        }
        
#endif
        
#if UNITY_EDITOR

        public static AppalachiaObject CreateNew(Type t, Type ownerType = null)
        {
            using (_PRF_CreateNew.Auto())
            {
                return AppalachiaObjectFactory.CreateNewAsset(t, ownerType: ownerType, overwriteExisting: false) as AppalachiaObject;
            }
        }

        public static AppalachiaObject CreateNew(
            Type t,
            string name,
            AppalachiaObject i = null,
            string dataFolder = null, Type ownerType = null)
        {
            using (_PRF_CreateNew.Auto())
            {
                if (i == null)
                {
                    i = CreateInstance(t) as AppalachiaObject;
                }

                return AppalachiaObjectFactory.SaveInstanceToAsset(t, name, i, dataFolder, ownerType) as AppalachiaObject;
                ;
            }
        }

        public static T CreateNew<T>(Type ownerType = null)
            where T : AppalachiaObject
        {
            using (_PRF_CreateNew.Auto())
            {
                return CreateNew(typeof(T), ownerType) as T;
            }
        }

        public static T CreateNew<T>(string name, T i = null, string dataFolder = null, Type ownerType = null)
            where T : AppalachiaObject
        {
            using (_PRF_CreateNew.Auto())
            {
                return CreateNew(typeof(T), name, i, dataFolder, ownerType) as T;
            }
        }

        public static AppalachiaObject[] GetAllOfType(Type t)
        {
            using (_PRF_GetAllOfType.Auto())
            {
                var all = AssetDatabaseManager.FindAssets($"t: {t.Name}");

                var results = new AppalachiaObject[all.Length];

                for (var i = 0; i < all.Length; i++)
                {
                    var path = AssetDatabaseManager.GUIDToAssetPath(all[i]);

                    results[i] = AssetDatabaseManager.LoadAssetAtPath(path, t) as AppalachiaObject;
                }

                return results;
            }
        }

        public static List<AppalachiaObject> GetAllOfType(Type t, Predicate<AppalachiaObject> where)
        {
            using (_PRF_GetAllOfType.Auto())
            {
                return GetAllOfType(t).Where(v => where(v)).ToList();
            }
        }

        public static T[] GetAllOfType<T>()
        {
            using (_PRF_GetAllOfType.Auto())
            {
                return GetAllOfType(typeof(T)).Cast<T>().ToArray();
            }
        }

        public static List<T> GetAllOfType<T>(Predicate<T> where)
            where T : AppalachiaObject
        {
            using (_PRF_GetAllOfType.Auto())
            {
                var predicate = new Predicate<AppalachiaObject>(obj => where(obj as T));

                return GetAllOfType(typeof(T), predicate).Cast<T>().ToList();
            }
        }

        public static AppalachiaObject LoadOrCreateNew(Type t, string name, string dataFolder = null, Type ownerType = null)
        {
            using (_PRF_LoadOrCreateNew.Auto())
            {
                return AppalachiaObjectFactory.LoadExistingOrCreateNewAsset(t, name, dataFolder, ownerType) as AppalachiaObject;
            }
        }

        public static AppalachiaObject LoadOrCreateNew(
            Type t,
            string name,
            bool prependType,
            bool appendType,
            string dataFolder = null, Type ownerType = null)
        {
            using (_PRF_LoadOrCreateNew.Auto())
            {
                return AppalachiaObjectFactory.LoadExistingOrCreateNewAsset(t, name, prependType, appendType, dataFolder, ownerType)
                    as AppalachiaObject;
            }
        }

        public static T LoadOrCreateNew<T>(string name, string dataFolder = null, Type ownerType = null)
            where T : AppalachiaObject
        {
            using (_PRF_LoadOrCreateNew.Auto())
            {
                return LoadOrCreateNew(typeof(T), name, dataFolder, ownerType) as T;
            }
        }

        public static T LoadOrCreateNew<T>(
            string name,
            bool prependType,
            bool appendType,
            string dataFolder = null, Type ownerType = null)
            where T : AppalachiaObject
        {
            using (_PRF_LoadOrCreateNew.Auto())
            {
                return LoadOrCreateNew(typeof(T), name, prependType, appendType, dataFolder, ownerType) as T;
            }
        }

        public static void LoadOrCreateNewIfNull(
            Type t,
            ref AppalachiaObject assignment,
            string name,
            string dataFolder = null, Type ownerType = null)
        {
            using (_PRF_LoadOrCreateNewIfNull.Auto())
            {
                if (assignment == null)
                {
                    assignment =
                        AppalachiaObjectFactory.LoadExistingOrCreateNewAsset(t, name, dataFolder, ownerType) as AppalachiaObject;
                    ;
                }
            }
        }

        public static void LoadOrCreateNewIfNull<T>(ref T assignment, string name, string dataFolder = null, Type ownerType = null)
            where T : AppalachiaObject
        {
            using (_PRF_LoadOrCreateNewIfNull.Auto())
            {
                AppalachiaObject obj = assignment;

                LoadOrCreateNewIfNull(typeof(T), ref obj, name, dataFolder, ownerType);

                assignment = obj as T;
            }
        }
#endif

#if UNITY_EDITOR

        #region IAppalachiaObject Members

        public string NiceName
        {
            get
            {
                if ((_niceName == null) || (name != _cachedName))
                {
                    _cachedName = name;
                    _niceName = UnityEditor.ObjectNames.NicifyVariableName(name);
                    this.MarkAsModified();
                }

                return _niceName;
            }
            set => _niceName = value;
        }

        [ButtonGroup(GROUP_BUTTONS)]
        public void Ping()
        {
            using (_PRF_Ping.Auto())
            {
                UnityEditor.EditorGUIUtility.PingObject(this);
            }
        }

        [ButtonGroup(GROUP_BUTTONS)]
        [PropertyOrder(-1000)]
        public void SaveNow()
        {
            using (_PRF_SaveNow.Auto())
            {
               this.MarkAsModified();

                if (!Application.isPlaying)
                {
                    AssetDatabaseSaveManager.SaveAssetsNextFrame();
                }
            }
        }

        [ShowIfGroup(SHOW_WORKFLOW)]
        [FoldoutGroup(GROUP_WORKFLOW, Order = -50000)]
        [ButtonGroup(GROUP_WORKFLOW_PROD)]
        [PropertyOrder(-40000)]
        [ShowIf(nameof(ShowWorkflow))]
        public void Select()
        {
            using (_PRF_Select.Auto())
            {
                AssetDatabaseManager.SetSelection(this);
            }
        }

        [ButtonGroup(GROUP_WORKFLOW_PROD)]
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

        public void Rename(string newName)
        {
            using (_PRF_Rename.Auto())
            {
                AppalachiaObjectFactory.RenameAsset(this, newName);
            }
        }

        #endregion

#endif

        protected virtual bool InitializeOnEnable => false;
        protected virtual bool InitializeOnAwake => false;
        protected virtual bool InitializeAlways => false;
        
        private static readonly ProfilerMarker _PRF_Awake = new ProfilerMarker(_PRF_PFX + nameof(Awake));
        private static readonly ProfilerMarker _PRF_OnEnable = new ProfilerMarker(_PRF_PFX + nameof(OnEnable));
        
        protected virtual void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                if (InitializeAlways || InitializeOnAwake)
                {
                    Initialize();
                }
            }
        }
        
        protected virtual void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                if (InitializeAlways || InitializeOnEnable)
                {
                    Initialize();
                }
            }
        }

#if UNITY_EDITOR
        private const string _PRF_PFX = nameof(AppalachiaObject) + ".";
        private static readonly ProfilerMarker _PRF_SaveNow = new(_PRF_PFX + nameof(SaveNow));
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
#endif

        protected virtual void Initialize()
        {
        }

        public void InitializeExternal()
        {
            Initialize();
        }
    }
}
