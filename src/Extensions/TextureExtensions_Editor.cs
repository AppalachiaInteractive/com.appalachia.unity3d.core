#if UNITY_EDITOR

#region

using Appalachia.CI.Integration.Assets;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Reflection.Extensions;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;

#endregion

namespace Appalachia.Core.Extensions
{
    public static partial class TextureExtensions
    {
        public static Vector2[][] GenerateOutline(
            this Texture texture,
            Rect rect,
            float detail,
            byte alphaTolerance,
            bool holeDetection)
        {
            using (_PRF_GenerateOutline.Auto())
            {
                var opaths = new Vector2[0][];
                object[] parameters = { texture, rect, detail, alphaTolerance, holeDetection, opaths };
                var method = spriteType.GetMethod("GenerateOutline", ReflectionExtensions.PrivateStatic);
                method.Invoke(null, parameters);
                var paths = (Vector2[][])parameters[5];

                return paths;
            }
        }

        public static TextureImporter GetTextureImporter(this Texture2D texture)
        {
            using (_PRF_GetTextureImporter.Auto())
            {
                var path = AssetDatabaseManager.GetAssetPath(texture);
                return (TextureImporter)AssetImporter.GetAtPath(path);
            }
        }

        public static bool HasCrunchCompression(this Texture2D texture)
        {
            using (_PRF_HasCrunchCompression.Auto())
            {
                if (null == texture)
                {
                    return false;
                }

                var assetPath = AssetDatabaseManager.GetAssetPath(texture);
                var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                if (tImporter != null)
                {
                    return tImporter.crunchedCompression;
                }

                return false;
            }
        }

        public static bool HasRgbaFormat(this Texture2D texture)
        {
            using (_PRF_HasRgbaFormat.Auto())
            {
                if (null == texture)
                {
                    return false;
                }

                var assetPath = AssetDatabaseManager.GetAssetPath(texture);
                var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                if (tImporter != null)
                {
                    var pts = tImporter.GetDefaultPlatformTextureSettings();
                    if ((pts.format == TextureImporterFormat.ARGB32) ||
                        (pts.format == TextureImporterFormat.RGBA32))
                    {
                        return true;
                    }

                    return false;
                }

                return false;
            }
        }

        public static void RemoveCrunchCompression(this Texture2D texture)
        {
            using (_PRF_RemoveCrunchCompression.Auto())
            {
                if (null == texture)
                {
                    return;
                }

                var assetPath = AssetDatabaseManager.GetAssetPath(texture);
                var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                if (tImporter != null)
                {
                    tImporter.crunchedCompression = false;
                    tImporter.SaveAndReimport();
                }
            }
        }

        public static void SetReadable(this Texture2D texture)
        {
            using (_PRF_SetReadable.Auto())
            {
                if (AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    return;
                }

                var importer = texture.GetTextureImporter();

                if (importer == null)
                {
                    return;
                }

                if (importer.isReadable)
                {
                    return;
                }

                importer.isReadable = true;

                importer.SaveAndReimport();
            }
        }

        public static void SetReadable(this Texture2D texture, bool normalTexture)
        {
            if (null == texture)
            {
                return;
            }

            var assetPath = AssetDatabaseManager.GetAssetPath(texture);
            var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (tImporter != null)
            {
                tImporter.textureType = normalTexture
                    ? TextureImporterType.NormalMap
                    : TextureImporterType.Default;

                if (tImporter.isReadable)
                {
                    return;
                }

                tImporter.isReadable = true;
                tImporter.SaveAndReimport();
            }
        }

        public static void SetRgba32Format(this Texture2D texture)
        {
            using (_PRF_SetRgba32Format.Auto())
            {
                if (null == texture)
                {
                    return;
                }

                var assetPath = AssetDatabaseManager.GetAssetPath(texture);
                var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                if (tImporter != null)
                {
                    var pts = tImporter.GetDefaultPlatformTextureSettings();
                    pts.format = TextureImporterFormat.RGBA32;
                    tImporter.SetPlatformTextureSettings(pts);
                    tImporter.SaveAndReimport();
                }
            }
        }

        public static void SetTextureSGBA(this Texture2D texture, bool value)
        {
            using (_PRF_SetTextureSGBA.Auto())
            {
                if (null == texture)
                {
                    return;
                }

                var assetPath = AssetDatabaseManager.GetAssetPath(texture);
                var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                if (tImporter != null)
                {
                    tImporter.sRGBTexture = value;
                    tImporter.SaveAndReimport();
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_SetReadable =
            new ProfilerMarker(_PRF_PFX + nameof(SetReadable));

        private static readonly ProfilerMarker _PRF_SetRgba32Format =
            new ProfilerMarker(_PRF_PFX + nameof(SetRgba32Format));

        private static readonly ProfilerMarker _PRF_SetTextureSGBA =
            new ProfilerMarker(_PRF_PFX + nameof(SetTextureSGBA));

        private static readonly ProfilerMarker _PRF_GenerateOutline =
            new ProfilerMarker(_PRF_PFX + nameof(GenerateOutline));

        private static readonly ProfilerMarker _PRF_GetTextureImporter =
            new ProfilerMarker(_PRF_PFX + nameof(GetTextureImporter));

        private static readonly ProfilerMarker _PRF_HasCrunchCompression =
            new ProfilerMarker(_PRF_PFX + nameof(HasCrunchCompression));

        private static readonly ProfilerMarker _PRF_HasRgbaFormat =
            new ProfilerMarker(_PRF_PFX + nameof(HasRgbaFormat));

        private static readonly ProfilerMarker _PRF_RemoveCrunchCompression =
            new ProfilerMarker(_PRF_PFX + nameof(RemoveCrunchCompression));

        #endregion
    }
}

#endif
