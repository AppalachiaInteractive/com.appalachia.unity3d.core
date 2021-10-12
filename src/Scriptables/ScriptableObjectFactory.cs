using System;
using System.IO;
using Appalachia.CI.Integration;
using Appalachia.Core.Assets;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;

namespace Appalachia.Core.Scriptables
{
    internal static class ScriptableObjectFactory
    {
        private const string _PRF_PFX = nameof(ScriptableObjectFactory) + ".";
        private static readonly ProfilerMarker _PRF_CreateNew = new(_PRF_PFX + nameof(CreateNew));

        private static readonly ProfilerMarker _PRF_LoadOrCreateNew =
            new(_PRF_PFX + nameof(LoadOrCreateNew));

        private static readonly ProfilerMarker _PRF_Rename = new(_PRF_PFX + nameof(Rename));

        public static T CreateNew<T>()
            where T : AppalachiaScriptableObject<T>
        {
            using (_PRF_CreateNew.Auto())
            {
                return LoadOrCreateNew<T>(
                    $"{typeof(T).Name}_{DateTime.Now:yyyyMMdd-hhmmssfff}.asset",
                    false,
                    false
                );
            }
        }

        public static T LoadOrCreateNew<T>(string name)
            where T : AppalachiaScriptableObject<T>
        {
            using (_PRF_LoadOrCreateNew.Auto())
            {
                return LoadOrCreateNew<T>(name, false, false);
            }
        }

        public static T LoadOrCreateNew<T>(string name, bool prependType, bool appendType)
            where T : AppalachiaScriptableObject<T>
        {
            using (_PRF_LoadOrCreateNew.Auto())
            {
                var cleanFileName = name;
                var hasDot = name.Contains(".");
                var lastIsDot = name.EndsWith(".");

                if (lastIsDot)
                {
                    name += "asset";
                    cleanFileName = name.TrimEnd('.');
                }
                else if (!hasDot)
                {
                    name += ".asset";
                }
                else
                {
                    cleanFileName = Path.GetFileNameWithoutExtension(name);
                }

                var extension = Path.GetExtension(name);

                var t = typeof(T).Name;

                if (prependType)
                {
                    cleanFileName = $"{t}_{cleanFileName}";
                }

                if (appendType)
                {
                    cleanFileName = $"{cleanFileName}_{t}";
                }

                name = $"{cleanFileName}{extension}";

                var any = AssetDatabase.FindAssets($"t: {t} {cleanFileName}");

                for (var i = 0; i < any.Length; i++)
                {
                    var path = AssetDatabase.GUIDToAssetPath(any[i]);
                    var existingName = Path.GetFileNameWithoutExtension(path);

                    if ((existingName != null) &&
                        string.Equals(cleanFileName.ToLower(), existingName.ToLower()))
                    {
                        return AssetDatabase.LoadAssetAtPath<T>(path);
                    }
                }

                var instance = ScriptableObject.CreateInstance(typeof(T)) as T;

                return CreateNew(name, instance);
            }
        }
        
        public static T CreateNew<T>(string name, T i)
            where T : AppalachiaScriptableObject<T>
        {
            using (_PRF_CreateNew.Auto())
            {
                var ext = Path.GetExtension(name);

                if (string.IsNullOrWhiteSpace(ext))
                {
                    name += ".asset";
                }

                var dataFolder = AssetDatabaseManager.GetSaveLocationForScriptableObject<T>()
                                                     .relativePath;

                var assetPath = Path.Combine(dataFolder, name);

                if (File.Exists(assetPath))
                {
                    throw new AccessViolationException(assetPath);
                }

                assetPath = assetPath.Replace(ProjectLocations.GetAssetsDirectoryPath(), "Assets");

                AssetDatabase.CreateAsset(i, assetPath);

                i.OnCreate();

                return i;
            }
        }

        public static void Rename<T>(T instance, string newName)
            where T : AppalachiaScriptableObject<T>
        {
            using (_PRF_Rename.Auto())
            {
                var path = AssetDatabase.GetAssetPath(instance);
                instance.name = newName;

                AssetDatabase.RenameAsset(path, newName);
            }
        }
    }
}
