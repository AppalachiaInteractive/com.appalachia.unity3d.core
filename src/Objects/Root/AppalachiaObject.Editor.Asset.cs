#if UNITY_EDITOR
using Appalachia.CI.Integration.Assets;
using Appalachia.CI.Integration.Core;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Root
{
    public abstract partial class AppalachiaObject
    {
        #region Fields and Autoproperties

        #endregion

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

        public void Rename(string newName)
        {
            using (_PRF_Rename.Auto())
            {
                AppalachiaObjectFactory.RenameAsset(this, newName);
                MarkAsModified();
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

                var finalPath = AppaPath.Combine(
                                             basePath,
                                             ZString.Format("{0}{1}", newPath_name, newPath_extension)
                                         )
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

        #region Profiling

        private static readonly ProfilerMarker _PRF_DirectoryPath =
            new ProfilerMarker(_PRF_PFX + nameof(DirectoryPath));

        private static readonly ProfilerMarker _PRF_AssetPath =
            new ProfilerMarker(_PRF_PFX + nameof(AssetPath));

        private static readonly ProfilerMarker _PRF_HasAssetPath =
            new ProfilerMarker(_PRF_PFX + nameof(HasAssetPath));

        private static readonly ProfilerMarker _PRF_HasSubAssets =
            new ProfilerMarker(_PRF_PFX + nameof(HasSubAssets));

        private static readonly ProfilerMarker _PRF_UpdateNameAndMove =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateNameAndMove));

        private static readonly ProfilerMarker _PRF_Rename = new ProfilerMarker(_PRF_PFX + nameof(Rename));

        #endregion
    }
}

#endif
