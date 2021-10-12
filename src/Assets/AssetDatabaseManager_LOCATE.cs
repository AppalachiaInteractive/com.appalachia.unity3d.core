using System;
using System.Collections.Generic;
using System.IO;
using Appalachia.CI.Integration;
using Appalachia.CI.Integration.Paths;
using Appalachia.Utility.Reflection.Extensions;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.Presets;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;
using UnityEngine.TextCore.Text;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Assets
{
    public static partial class AssetDatabaseManager
    {
        
        private static Dictionary<Type, MonoScript> _typeScriptLookup;
        private static Dictionary<MonoScript, Type> _scriptTypeLookup;
        private static List<MonoScript> _allMonoScripts;
        

        public static AssetPathMetadata GetSaveLocationForOwnedAsset<TOwner, TAsset>(
            string fileName)
            where TOwner : MonoBehaviour
            where TAsset : Object
        {
            var ownerType = typeof(TOwner);
            var assetType = typeof(TAsset);

            var ownerScript = GetScriptFromType(ownerType);
            var ownerPath = AssetDatabase.GetAssetPath(ownerScript);

            return GetSaveLocationMetadataInternal(ownerPath, fileName, assetType);
        }

        public static AssetPathMetadata GetSaveLocationForAsset(Type assetType, string assetPath)
        {
            var assetName = Path.GetFileName(assetPath);

            return GetSaveLocationMetadataInternal(assetPath, assetName, assetType);
        }

        public static AssetPathMetadata GetSaveLocationForScriptableObject<T>()
            where T : ScriptableObject
        {
            var scriptType = typeof(T);

            return GetSaveLocationForScriptableObject(scriptType);
        }

        public static AssetPathMetadata GetSaveLocationForScriptableObject(Type scriptType)
        {
            var script = GetScriptFromType(scriptType);
            var scriptPath = AssetDatabase.GetAssetPath(script);

            return GetSaveLocationMetadataInternal(scriptPath, null, scriptType);
        }

        private static AssetPathMetadata GetSaveLocationMetadataInternal(
            string relativePathToRepositoryMember,
            string saveFileName,
            Type saveFiletype)
        {
            var assetBasePath = ProjectLocations.GetAssetsDirectoryPath();

            string baseDataFolder;

            var repository = ProjectLocations.GetAssetRepository(relativePathToRepositoryMember);

            AssetPathMetadataType pathType;

            if (repository == null)
            {
                baseDataFolder = Path.Combine(assetBasePath, "Appalachia", "Data");
                pathType = AssetPathMetadataType.ProjectDataFolder;
            }
            else
            {
                baseDataFolder = repository.dataDirectory.FullName;
                pathType = AssetPathMetadataType.RepositoryDataFolder;
            }

            var isLibrary = baseDataFolder.StartsWith("Library");
            var isPackage = baseDataFolder.StartsWith("Packages");

            if (isLibrary)
            {
                pathType = AssetPathMetadataType.LibraryResource;
            }
            else if (isPackage)
            {
                pathType = AssetPathMetadataType.PackageResource;
            }

            string finalFolderName;

            
            var assetFolderName = GetAssetFolderByType(saveFiletype, saveFileName);
            var typeFolderName = saveFiletype.GetSimpleReadableName();

            if (assetFolderName != "Other")
            {
                finalFolderName = assetFolderName;
            }
            else
            {
                finalFolderName = typeFolderName;
            }

            var finalFolder = Path.Combine(baseDataFolder, finalFolderName);

            var output = new AssetPathMetadata(finalFolder, true);

            output.pathType = pathType;
            return output;
        }


        private static Dictionary<Type, Func<Type, string, string>> _assetTypeFolderLookup;

        private static void PopulateAssetTypeFolderLookup()
        {
            _assetTypeFolderLookup = new Dictionary<Type, Func<Type, string, string>>();

            var atfl = _assetTypeFolderLookup;

            atfl.Add(typeof(AnimationClip),              (_, _) => "Animations");
            atfl.Add(typeof(AnimatorOverrideController), (_, _) => "Animations");
            atfl.Add(typeof(BlendTree),                  (_, _) => "Animations");

            atfl.Add(typeof(AudioClip),  (_, _) => "Audio");
            atfl.Add(typeof(AudioMixer), (_, _) => "Audio");

            atfl.Add(typeof(ComputeShader), (_, _) => "ComputeShaders");

            atfl.Add(typeof(Cubemap), (_, _) => "Cubemaps");

            atfl.Add(typeof(FontAsset), (_, _) => "Fonts");

            atfl.Add(typeof(GUISkin), (_, _) => "GUISkins");

            atfl.Add(typeof(GameObject), (_, s) => s != "prefab" ? "Models" : "Prefabs");

            atfl.Add(typeof(Material), (_, _) => "Materials");

            atfl.Add(typeof(Mesh), (_, _) => "Meshes");

            atfl.Add(typeof(PhysicMaterial), (_, _) => "PhysicsMaterials");

            atfl.Add(typeof(PlayableAsset), (_, _) => "Playables");

            atfl.Add(typeof(Preset), (_, _) => "Presets");
            
            atfl.Add(typeof(SceneAsset), (_, _) => "Scenes");

            atfl.Add(typeof(Shader),                  (_, _) => "Shaders");
            atfl.Add(typeof(ShaderInclude),           (_, _) => "Shaders");
            atfl.Add(typeof(ShaderVariantCollection), (_, _) => "Shaders");

            atfl.Add(typeof(TerrainData),  (_, _) => "Terrains");
            atfl.Add(typeof(TerrainLayer), (_, _) => "Terrains");

            atfl.Add(typeof(Texture), (_, _) => "Textures");

            atfl.Add(typeof(Sprite),           (_, _) => "Sprites");
            atfl.Add(typeof(SpriteAsset),      (_, _) => "Sprites");
            atfl.Add(typeof(SpriteAtlas),      (_, _) => "Sprites");
            atfl.Add(typeof(SpriteAtlasAsset), (_, _) => "Sprites");
        }

        private static string GetAssetFolderByType(Type t, string fileName)
        {
            string extension = null;
            
            if (fileName != null)
            {
                extension = Path.GetExtension(fileName.Trim('.')).ToLowerInvariant();
            }

            if (_assetTypeFolderLookup == null)
            {
                PopulateAssetTypeFolderLookup();
            }

            foreach (var assetType in _assetTypeFolderLookup.Keys)
            {
                if (assetType.IsAssignableFrom(t))
                {
                    var typeFunction = _assetTypeFolderLookup[assetType];
                    
                    return typeFunction(assetType, extension);
                }
            }

            return "Other";
        }
        
        
        private static void InitializeTypeScriptLookups()
        {
            var initialize = false;
            if (_typeScriptLookup == null)
            {
                _typeScriptLookup = new Dictionary<Type, MonoScript>();
                initialize = true;
            }
            
            if (_scriptTypeLookup == null)
            {
                _scriptTypeLookup = new Dictionary<MonoScript, Type>();
                initialize = true;
            }

            if (initialize)
            {
                var scripts = GetAllMonoScripts();

                for (var index = 0; index < scripts.Count; index++)
                {
                    var script = scripts[index];

                    var scriptType = script.GetClass();

                    _scriptTypeLookup.Add(script, scriptType);
                    
                    if (scriptType == null)
                    {
                        continue;
                    }

                    if (_typeScriptLookup.ContainsKey(scriptType))
                    {
                        _typeScriptLookup[scriptType] = script;
                    }
                    else
                    {
                        _typeScriptLookup.Add(scriptType, script);                        
                    }
                }
            }
        }

        public static Type GetTypeFromScript(MonoScript t)
        {
            InitializeTypeScriptLookups();

            if (!_scriptTypeLookup.ContainsKey(t))
            {
                return null;
            }

            return _scriptTypeLookup[t];
        }

        public static MonoScript GetScriptFromType(Type t)
        {
            InitializeTypeScriptLookups();

            if (!_typeScriptLookup.ContainsKey(t))
            {
                return null;
            }

            return _typeScriptLookup[t];
        }
        
        
        public static List<MonoScript> GetAllMonoScripts()
        {
            using (_PRF_GetAllMonoScripts.Auto())
            {
                if (_allMonoScripts == null || _allMonoScripts.Count == 0)
                {
                    _allMonoScripts = new List<MonoScript>();
                    
                    var monoScriptPaths = GetAssetPathsByExtension(".cs");

                    foreach (var monoscriptPath in monoScriptPaths)
                    {
                        var importer = AssetImporter.GetAtPath(monoscriptPath);

                        if (importer is MonoImporter mi)
                        {
                            var script = mi.GetScript();

                            _allMonoScripts.Add(script);
                        }
                    }
                }

                return _allMonoScripts;
            }
        }
    }
}
