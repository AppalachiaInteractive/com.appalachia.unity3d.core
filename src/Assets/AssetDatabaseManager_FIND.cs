using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Assets
{
    public static partial class AssetDatabaseManager
    {
        private static string[] _allAssetPaths;
        private static Dictionary<string, List<string>> _assetPathsByExtension;

        private static string CleanExtension(this string extension)
        {
            return extension?.ToLower().Trim().Trim('.');
        }
        
        private static void InitializeAssetPathData()
        {
            if (_allAssetPaths == null || _allAssetPaths.Length == 0)
            {
                _allAssetPaths = AssetDatabase.GetAllAssetPaths();

                _assetPathsByExtension = null;
            }

            if (_assetPathsByExtension == null || _assetPathsByExtension.Count == 0)
            {
                _assetPathsByExtension = new Dictionary<string, List<string>>();
                    
                foreach (var asset in _allAssetPaths)
                {
                    var extension = Path.GetExtension(asset).CleanExtension();

                    if (string.IsNullOrWhiteSpace(extension))
                    {
                        continue;
                    }

                    if (!_assetPathsByExtension.ContainsKey(extension))
                    {
                        _assetPathsByExtension.Add(extension, new List<string>());
                    }
                    
                    _assetPathsByExtension[extension].Add(asset);
                }
            }
        }
        

        public static List<T> FindAssets<T>(string searchString = null)
            where T : Object
        {
            var t = typeof(T);
            
            var paths = FindAssetPaths<T>(searchString);
            var results = new List<T>(paths.Length);

            for (var i = 0; i < paths.Length; i++)
            {
                var path = paths[i]; 
                
                var type = AssetDatabase.GetMainAssetTypeAtPath(path);

                if (t.IsAssignableFrom(type))
                {
                    var cast = AssetDatabase.LoadAssetAtPath<T>(path);
                    results.Add(cast);
                }
            }

            return results;
        }

        public static List<Object> FindAssets(Type t, string searchString = null)
        {
            var paths = FindAssetPaths(t, searchString);
            var results = new List<Object>(paths.Length);

            for (var i = 0; i < paths.Length; i++)
            {
                var path = paths[i]; 
                
                var type = AssetDatabase.GetMainAssetTypeAtPath(path);

                if (t.IsAssignableFrom(type))
                {
                    var cast = AssetDatabase.LoadAssetAtPath(path, t);
                    results.Add(cast);
                }
            }

            return results;
        }

        public static string[] FindAssetPaths<T>(string searchString = null)
        {
            return FindAssetPaths(typeof(T), searchString);
        }

        public static string[] FindAssetPaths(Type t, string searchString = null)
        {
            var guids = FindAssetGuids(t, searchString);
            var paths = new string[guids.Length];

            for (var index = 0; index < guids.Length; index++)
            {
                var guid = guids[index];
                var path = AssetDatabase.GUIDToAssetPath(guid);

                paths[index] = path;
            }

            return paths;
        }

        public static string[] FindAssetGuids<T>(string searchString = null)
        where T : UnityEngine.Object
        {
            return FindAssetGuids(typeof(T), searchString);
        }
        
        public static string[] FindAssetGuids(Type t, string searchString = null)
        {
            var guids = AssetDatabase.FindAssets(FormatSearchString(t, searchString));

            return guids;
        }

        private static string FormatSearchString(Type t, string searchString)
        {
            var typename = t.Name;
            return $"t:{typename} {searchString ?? string.Empty}";
        }

        public static List<string> GetAssetPathsByExtension(string extension)
        {
            InitializeAssetPathData();
            
            var cleanExtension = extension.CleanExtension();

            if (_assetPathsByExtension.ContainsKey(cleanExtension))
            {
                return _assetPathsByExtension[cleanExtension];
            }

            return null;
        }
    }
}
